
using System.Drawing;
using System.Net.Http.Json;
using System.Numerics;
using AIWorldLibrary;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworkLibrary;

namespace WarehouseAgent;


public class BufferState(WarehouseAgentState currentState, int action, WarehouseAgentState? next = null, int reward = 0)
{
    public WarehouseAgentState CurrentState { get; set; } = currentState;
    public int Action { get; set; } = action;
    public WarehouseAgentState? NextState { get; set; } = next;
    public int Reward { get; set; } = reward;
    public bool IsTerminalMove => NextState == null;
}
public class BufferWrapper
{
    public BufferState[] bufferStates = new BufferState[10000];
    public int Count { get; set; } = 0;
    public void AddToBuffer(BufferState state)
    {
        if (Count >= bufferStates.Length)
        {
            Count = 0;
        }
        bufferStates[Count] = state;
        Count++;
    }
    public List<BufferState> GrabMiniBatch(int miniBatchSize)
    {
        HashSet<int> indexes = [];
        List<BufferState> result = new List<BufferState>(miniBatchSize);
        while (indexes.Count < miniBatchSize)
        {
            int index = Random.Shared.Next(bufferStates.Length);

            if (indexes.Contains(index) || bufferStates[index] == null) continue;

            indexes.Add(index);
            result.Add(bufferStates[index]);
        }
        return result;
    }
}
public class WarehouseAgentDeepLearning(NeuralNet net, string envAPIUrl, double iterationSwitch = 1000, double epsilon = 0.200, double epsilonDecay = 1.0, double learningRate = 0.001, double momentum = 0, double discount = 0.95, double costOfLiving = -0.1, int miniBatchSize = 64)
    : Agent(envAPIUrl)
{
    public NeuralNet liveNet = net;
    public NeuralNet stableNet = net;
    public WarehouseAgentState? CurrentState = null;
    public double Epsilon = epsilon;
    private readonly double LearningRate = learningRate;
    private readonly double Discount = discount;
    private readonly double CostOfLiving = costOfLiving;
    private readonly int MiniBatchSize = miniBatchSize;
    private readonly double EpsilonDecay = epsilonDecay; //tweak to rmeove randomnes as time goes on (handled it manually but can be changed later to not be)
    private readonly double Momentum = momentum;
    private readonly double IterationSwtich = iterationSwitch;
    private int iterationCounter = 0;
    private BufferWrapper buffer = new BufferWrapper();
    private record LocationInfo(Point Location) : ISensorData;
    private record SpotStateInfo : ISensorData
    {
        public double SpotScore { get; set; }
    }
    public class SensorPerms : BetterEnum
    {
        public static readonly SensorPerms LocationSensor = new() { Value = 1 << 0 };
        public static readonly SensorPerms SpotStateSensor = new() { Value = 1 << 1 };
        public override int Value { get; set; }

        public override BetterEnum And(int val)
        {
            var mP = new SensorPerms
            {
                Value = Value & val
            };
            return mP;
        }
    }
    public class MovementPerms : BetterEnum
    {

        public static readonly MovementPerms Directional = new() { Value = 1 << 0 };
        public override int Value { get; set; }

        public override BetterEnum And(int val)
        {
            var mP = new MovementPerms
            {
                Value = Value & val
            };
            return mP;
        }
    }
    public void BatchTrain(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!MakingMove)
            {
                var p = Task.Run(MakeMove);
                p.Wait();
            }
        } 
        Epsilon = 0;
    }
    public override async Task RegisterMovements()
    {
        while (ID == -1)
        {
            Thread.Sleep(10);
        }
        if (!await RegisterMovementPermission(MovementPerms.Directional))
        {
            throw new Exception("Couldn't register movement");
        }
        await base.RegisterMovements();
    }
    public override async Task RegisterSensors()
    {
        while (ID == -1)
        {
            Thread.Sleep(10);
        }
        if (!await RegisterSensorPermission(SensorPerms.LocationSensor))
        {
            throw new Exception("Couldn't register sensor locaiton");
        }
        if (!await RegisterSensorPermission(SensorPerms.SpotStateSensor))
        {
            throw new Exception("Couldn't register spot state sensor");
        }
        await base.RegisterSensors();
    }
    public override async Task<int> SelectMoveID()
    {
        var moves = await GetAgentCurrentMoves();

        if (moves == null || moves.Count == 0)
        {
            //backprop to update q table
            return -1;
        }

        if (Random.Shared.NextDouble() < Epsilon)
        {
            int spot = Random.Shared.Next(0, moves.Count);
            return moves[spot].MoveID;
        }
        else
        {
            if (CurrentState == null) await Sense();

            if (CurrentState == null) throw new Exception("How is it null ????");

            double[] input = [CurrentState.Location.X, CurrentState.Location.Y];
            var r = liveNet.Compute(input);//qTable.GetValue((CurrentState.Location.X, CurrentState.Location.Y)) ?? throw new Exception("R can't be null");

            int movement = r.ToList().IndexOf(r.Max()) + 1;

            return movement;
        }
    }
    public override async Task<bool> MakeMove()
    {
        iterationCounter++;
        if(iterationCounter % IterationSwtich == 0)
        {
            stableNet = liveNet.Clone();
        }
        MakingMove = true;
        int moveID = await SelectMoveID();
        var model = new
        {
            AgentID = ID,
            MoveID = moveID
        };

        var response = await Client.PostAsJsonAsync(EnvironmentAPIUrl + "/MakeMove", model);

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception($"failed on this: ({CurrentState?.Location.X}, {CurrentState?.Location.Y}) AgentDI: {model.AgentID}, Move: {model.MoveID}.");
        }
        await Sense();

        if (CurrentState == null) throw new Exception("CurrentState can't be null");

        if (!await response.Content.ReadFromJsonAsync<bool>())
        {
            if(CurrentState.Prev == null) throw new NotImplementedException("you spawn on a terminal???");

            BufferState newBufferState = new BufferState(CurrentState.Prev, moveID, null, CurrentState.Prev.Score);
            buffer.AddToBuffer(newBufferState);

            if (buffer.Count >= MiniBatchSize)
            {
                Train();
            }
            CurrentState.Prev = null;
            MakingMove = false;
            return false;
        }

        if (CurrentState.Prev != null) //might be an unneccessary check
        {
            BufferState newBufferState = new (CurrentState.Prev, moveID, CurrentState, CurrentState.Score);

            buffer.AddToBuffer(newBufferState);
            if (buffer.Count >= MiniBatchSize)
            {
                Train();
            }

        }

        return await base.MakeMove();
    }
    private void Train()
    {
        List<BufferState> bufferStates = buffer.GrabMiniBatch(MiniBatchSize);
        liveNet.ClearUpdates();
        foreach (BufferState state in bufferStates)
        {
            double output = 0;

            if(!state.IsTerminalMove)
            {
                double[] input2 = [state.NextState.Location.X, state.NextState.Location.Y];
                
                output = stableNet.Compute(MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(input2)).Max();
            }
            else
            {
                ;
            }

            double target = state.Reward - CostOfLiving + (Discount * output);


            var inputVector = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense([state.CurrentState.Location.X, state.CurrentState.Location.Y]); 
            var computeResult = liveNet.Compute(inputVector); 
        
            double actual = computeResult[state.Action - 1];
            double[] error = new double[4];
            error[state.Action-1] =  2 * (actual - target);
            

            var v = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(error); 
            
            liveNet.AccumulateUpdates(v);
            liveNet.ApplyUpdates(LearningRate, Momentum);
        }
    }

    public override async Task Sense()
    {
        var location = await GetCurrentLocation();
        var score = await GetSpotState();

        if (location == null || score == null) throw new Exception("Couldn't sense ???");


        CurrentState = new WarehouseAgentState(location.Location, (int)score.SpotScore, CurrentState);
        //var pair = (location.Location.X, location.Location.Y);
    }
    private async Task<LocationInfo?> GetCurrentLocation()
    {
        var model = new
        {
            AgentID = ID,
            SensorPermissions = SensorPerms.LocationSensor
        };

        var result = await Client.PostAsJsonAsync(EnvironmentAPIUrl + "/GetSensorData", model);

        if (result.StatusCode != System.Net.HttpStatusCode.OK) return null;

        return result.Content.ReadFromJsonAsync<LocationInfo>().Result;
    }

    private async Task<SpotStateInfo?> GetSpotState()
    {
        var model = new
        {
            AgentID = ID,
            SensorPermissions = SensorPerms.SpotStateSensor
        };

        var result = await Client.PostAsJsonAsync(EnvironmentAPIUrl + "/GetSensorData", model);

        if (result.StatusCode != System.Net.HttpStatusCode.OK) return null;

        return result.Content.ReadFromJsonAsync<SpotStateInfo>().Result;
    }

}
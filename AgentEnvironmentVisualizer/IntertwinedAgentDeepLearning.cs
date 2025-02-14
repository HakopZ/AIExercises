
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Numerics;
using AIWorldLibrary;
using Environments;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworkLibrary;
using WarehouseAgent;

namespace AgentEnvironmentVisualizer;

public class IntertwinedWarehouseAgentDeepLearning(NeuralNet net, double iterationSwitch = 1000, double epsilon = 0.200, double epsilonDecay = 1.0, double learningRate = 0.001, double momentum = 0, double discount = 0.95, double costOfLiving = -0.1, int miniBatchSize = 64)
{
    public bool isReadyToDisplay = false;
    public NeuralNet liveNet = net;
    public NeuralNet stableNet = net;
    public WarehouseAgentState? CurrentState = null;
    public double Epsilon = epsilon;
    private readonly double LearningRate = learningRate;
    private readonly double Discount = discount;
    private readonly double CostOfLiving = costOfLiving;
    private readonly int MiniBatchSize = miniBatchSize;
    private readonly double EpsilonDecay = epsilonDecay;
    private readonly double Momentum = momentum;
    private readonly double IterationSwtich = iterationSwitch;
    private int iterationCounter = 0;
    public int ID = -1;
    public bool SensorRegistered = false;
    public bool MovementRegistered = false;
    public bool MakingMove = false;

    private BufferWrapper buffer = new BufferWrapper();
    // public class SensorPerms : BetterEnum
    // {
    //     public static readonly SensorPerms LocationSensor = new() { Value = 1 << 0 };
    //     public static readonly SensorPerms SpotStateSensor = new() { Value = 1 << 1 };
    //     public override int Value { get; set; }

    //     public override BetterEnum And(int val)
    //     {
    //         var mP = new SensorPerms
    //         {
    //             Value = Value & val
    //         };
    //         return mP;
    //     }
    // }
    // public class MovementPerms : BetterEnum
    // {

    //     public static readonly MovementPerms Directional = new() { Value = 1 << 0 };
    //     public override int Value { get; set; }

    //     public override BetterEnum And(int val)
    //     {
    //         var mP = new MovementPerms
    //         {
    //             Value = Value & val
    //         };
    //         return mP;
    //     }
    // }
    public void BatchTrain(int amount, WarehouseEnvironment environment)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!MakingMove)
            {
                if (MakeMove(environment))
                {

                }
                else
                {

                }
            }
        }
        isReadyToDisplay = true;
    }
    public void Register(WarehouseEnvironment environment)
    {
        ID = environment.RegisterAgent();
    }
    public void RegisterMovements(WarehouseEnvironment environment)
    {

        if (!RegisterMovementPermission(environment, new WarehouseMovementPermissions() { Value = 1 }))
        {
            throw new Exception("Couldn't register movement");
        }

    }
    public void RegisterSensors(WarehouseEnvironment environment)
    {

        if (!RegisterSensorPermission(environment, new WarehouseSensorPermissions() { Value = 1 }))
        {
            throw new Exception("Couldn't register sensor locaiton");
        }
        if (!RegisterSensorPermission(environment, new WarehouseSensorPermissions() { Value = 1 << 1 }))
        {
            throw new Exception("Couldn't register spot state sensor");
        }
    }
    private List<MoveReturn>? GetAgentCurrentMoves(WarehouseEnvironment environment)
    {
        var result = environment.GetMoves(ID);
        return result;
    }

    public int SelectMoveID(WarehouseEnvironment environment)
    {
        var moves = GetAgentCurrentMoves(environment);

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
            if (CurrentState == null) Sense(environment);

            if (CurrentState == null) throw new Exception("How is it null ????");

            double[] input = [CurrentState.Location.X, CurrentState.Location.Y];
            var r = liveNet.Compute(input);//qTable.GetValue((CurrentState.Location.X, CurrentState.Location.Y)) ?? throw new Exception("R can't be null");

            int movement = r.ToList().IndexOf(r.Max()) + 1;

            return movement;
        }
    }
    public bool MakeMove(WarehouseEnvironment environment)
    {
        iterationCounter++;
        if (iterationCounter % IterationSwtich == 0)
        {
            stableNet = liveNet.Clone();
        }
        MakingMove = true;
        int moveID = SelectMoveID(environment);
        var model = new
        {
            AgentID = ID,
            MoveID = moveID
        };

        var response = environment.MakeMove(model.MoveID, model.AgentID);
        Sense(environment);

        if (CurrentState == null) throw new Exception("CurrentState can't be null");

        if (!response)
        {
            if (CurrentState.Prev == null) throw new NotImplementedException("you spawn on a terminal???");

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
            BufferState newBufferState = new(CurrentState.Prev, moveID, CurrentState, CurrentState.Score);

            buffer.AddToBuffer(newBufferState);
            if (buffer.Count >= MiniBatchSize)
            {
                Train();
            }

        }
        MakingMove = false;
        return true;
    }
    private bool RegisterSensorPermission(WarehouseEnvironment environment, WarehouseSensorPermissions permission)
    {
        environment.RegisterAgentSensorPermission(ID, permission);
        return true;
    }
    private bool RegisterMovementPermission(WarehouseEnvironment environment, WarehouseMovementPermissions permission)
    {
        environment.RegisterAgentMovementPermission(ID, permission);
        return true;
    }
    private void Train()
    {
        List<BufferState> bufferStates = buffer.GrabMiniBatch(MiniBatchSize);
        liveNet.ClearUpdates();
        foreach (BufferState state in bufferStates)
        {
            double output = 0;

            if (!state.IsTerminalMove)
            {
                double[] input2 = [state.NextState.Location.X, state.NextState.Location.Y];

                output = stableNet.Compute(MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(input2)).Max();
            }
            else
            {
                ;
            }

            double target = state.Reward + (Discount * output);


            var inputVector = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense([state.CurrentState.Location.X, state.CurrentState.Location.Y]);
            var computeResult = liveNet.Compute(inputVector);

            double actual = computeResult[state.Action - 1];
            double[] error = new double[4];
            error[state.Action - 1] = 2 * (actual - target);


            var v = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(error);

            liveNet.AccumulateUpdates(v);
            liveNet.ApplyUpdates(LearningRate, Momentum);
        }
    }

    public void Sense(WarehouseEnvironment environment)
    {
        var location = GetCurrentLocation(environment);
        var score = GetSpotState(environment);

        if (location == null || score == null) throw new Exception("Couldn't sense ???");


        CurrentState = new WarehouseAgentState(location.Location, (int)score.SpotScore, CurrentState);
        //var pair = (location.Location.X, location.Location.Y);
    }
    private PointInfo? GetCurrentLocation(WarehouseEnvironment environment)
    {
        var model = new
        {
            AgentID = ID,
            SensorPermissions = new WarehouseSensorPermissions() { Value = 1 }
        };

        var result = environment.GetSensorData(model.AgentID, model.SensorPermissions);

        return (PointInfo)result;
    }

    private SpotStateInfo? GetSpotState(WarehouseEnvironment environment)
    {
        var model = new
        {
            AgentID = ID,
            SensorPermissions = new WarehouseSensorPermissions() { Value = 1 << 1 }
        };

        var result = environment.GetSensorData(model.AgentID, model.SensorPermissions);

        return (SpotStateInfo)result;
    }

}
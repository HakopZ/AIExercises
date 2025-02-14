
using System.ComponentModel;
using System.Drawing;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using AIWorldLibrary;

namespace WarehouseAgent;


public class DictionaryWrapper
{
    public Dictionary<(int x, int y), Dictionary<int, double>> backingType = [];
    public DictionaryWrapper(Dictionary<(int x, int y), Dictionary<int, double>>? collection = null)
    {
        if (collection != null) backingType = collection;
    }

    public Dictionary<int, double>? GetValue((int x, int y) pos)
    {
        if (backingType.TryGetValue(pos, out Dictionary<int, double>? val))
        {
            return val;
        }
        return null;
    }
    public void AddPair((int x, int y) pos, Dictionary<int, double> val) => backingType.Add(pos, val);

    public bool ContainsKey((int x, int y) pos) => backingType.ContainsKey(pos);

}
public enum SpotState
{
    Empty = 200,
    Obstacle = -200,
    Item = 400,
    Robot = -400
};
//mdp
public class MyWarehouseAgentQLearning(string envAPIUrl, double epsilon = 0.2, double learningRate = 0.1, double discount = 1, int costOfLiving = -1) 
    : Agent(envAPIUrl)
    
{
    public DictionaryWrapper qTable = new();
    public double Epsilon = epsilon;
    private readonly double LearningRate = learningRate;
    private readonly double Discount = discount;
    private readonly int CostOfLiving = costOfLiving;
    public WarehouseAgentState? CurrentState = null;
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
    public override async Task Sense()
    {
        var location = await GetCurrentLocation();
        var score = await GetSpotState();
        
        if (location == null || score == null) throw new Exception("Couldn't sense ???");


        CurrentState = new WarehouseAgentState(location.Location, (int)score.SpotScore, CurrentState);
        var pair = (location.Location.X, location.Location.Y);

        if (!qTable.ContainsKey(pair))
        {
            Dictionary<int, double> temp = new()
            {
                {1, 0},
                {2, 0},
                {3, 0},
                {4, 0}
            };
            qTable.AddPair(pair, temp);
        }
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

            var r = qTable.GetValue((CurrentState.Location.X, CurrentState.Location.Y)) ?? throw new Exception("R can't be null");

            var sorted = r.OrderByDescending(x => x.Value);
            var best = sorted.Where(x => x.Value == sorted.First().Value).ToArray();
            var chosen = best[Random.Shared.Next(0, best.Length)];

            return chosen.Key;
        }
    }

    public override async Task<bool> MakeMove()
    {
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
            throw new Exception("failed");
        }
        await Sense();

        if (CurrentState == null) throw new Exception("CurrentState can't be null");
        if (!await response.Content.ReadFromJsonAsync<bool>())
        {
            CurrentState.Prev = null;
            MakingMove = false;
            return false;
        }

        if (CurrentState.Prev != null) //might be an unneccessary check
        {
            var actions = qTable.GetValue((CurrentState.Prev.Location.X, CurrentState.Prev.Location.Y));
            var nextAction = qTable.GetValue((CurrentState.Location.X, CurrentState.Location.Y));

            if (actions == null || nextAction == null) throw new Exception("How are either null???");

            actions[moveID] = (1-LearningRate)*actions[moveID] + LearningRate * (CurrentState.Score - CostOfLiving
                + Discount * nextAction.Max(x => x.Value));
        }

       return await base.MakeMove();
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

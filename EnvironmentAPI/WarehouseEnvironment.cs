using System.Drawing;
using AIWorldLibrary;
using EnvironmentAPI;

namespace EnvironmentAPI;


public class MovementPermissions : BetterEnum
{

    public static readonly MovementPermissions None = new() { Value = 0 };
    public static readonly MovementPermissions Directional = new() { Value = 1 << 0 };
    public static readonly MovementPermissions Diagonal = new() { Value = 1 << 1 };

    private int _value = None.Value;
    public override int Value
    {
        get => _value;
        set
        {
            if (value >= 0 && value <= Diagonal.Value)
            {
                _value = value;
            }
        }
    }

    public override BetterEnum And(int val)
    {
        var mP = new MovementPermissions
        {
            Value = Value & val
        };
        return mP;
    }
}


public class SensorPermissions : BetterEnum
{
    public static readonly SensorPermissions None = new() { Value = 0 };
    public static readonly SensorPermissions Position = new() { Value = 1 << 0 };
    private int _value = None.Value;
    public override int Value
    {
        get => _value;
        set
        {
            if (value >= 0 && value <= Position.Value)
            {
                _value = value;
            }
        }
    }

    public override BetterEnum And(int val)
    {
        var mP = new MovementPermissions
        {
            Value = Value & val
        };
        return mP;
    }
}
public enum SpotState
{
    Empty = 200,
    Obstacle = -200,
    Item = 400,
    Robot = -400
};
public class Vertex(Point location, SpotState state)
{
    public Point Location { get; set; } = location;
    public SpotState State { get; set; } = state;
    public List<Edge> Edges { get; set; } = [];
    public void AddEdge(Vertex to, int cost) => Edges.Add(new(this, to, cost));
}
public class Edge(Vertex from, Vertex to, int cost = 1)
{
    public Vertex From { get; set; } = from;
    public Vertex To { get; set; } = to;
    public int Cost { get; set; } = cost;
}

public record PointInfo(Point Location) : ISensorData;

public record Nothing : ISensorData;

public class WarehouseEnvironment : AIEnvironment<EnvironmentState, MovementPermissions, SensorPermissions>
{
    public override bool IsStatic => false;
    public override bool IsDeterministic => true;
    public List<Vertex> Vertices { get; set; } = [];
    public List<Edge> Edges { get; set; } = [];

    public override Dictionary<SensorPermissions, Func<EnvironmentState, ISensorData>> SensorCapabilities => sensorCapabilties;

    public override Dictionary<MovementPermissions, Func<EnvironmentState, List<EnvironmentState>>> MovementCapabilities => movementCapabilities;

    

    public Dictionary<SensorPermissions, Func<EnvironmentState, ISensorData>> sensorCapabilties = new()
    {
        { SensorPermissions.None, (state) => new Nothing() },
        { SensorPermissions.Position, (state) => new PointInfo(state.RobotLocation) } //issue with this ask stan, techincally agent already has this, but I don't want them to, so should make move be null?
    };

    public Dictionary<MovementPermissions, Func<EnvironmentState, List<EnvironmentState>>> movementCapabilities = new()
    {
        { MovementPermissions.None, (state) => [state] },

        { MovementPermissions.Directional, (state) => [
            new EnvironmentState (new Point (state.RobotLocation.X - 1, state.RobotLocation.Y)),
            new EnvironmentState (new Point (state.RobotLocation.X + 1, state.RobotLocation.Y)),
            new EnvironmentState (new Point (state.RobotLocation.X, state.RobotLocation.Y - 1)),
            new EnvironmentState (new Point (state.RobotLocation.X, state.RobotLocation.Y + 1)),
        ]},

        { MovementPermissions.Diagonal, (state) => [
            new EnvironmentState (new Point (state.RobotLocation.X - 1, state.RobotLocation.Y - 1)),
            new EnvironmentState (new Point (state.RobotLocation.X + 1, state.RobotLocation.Y - 1)),
            new EnvironmentState (new Point (state.RobotLocation.X - 1, state.RobotLocation.Y + 1)),
            new EnvironmentState (new Point (state.RobotLocation.X + 1, state.RobotLocation.Y + 1)),

        ]}
    };
    public void ClearEnvironment()
    {
        Vertices.Clear();
        Edges.Clear();
    }
    public void ClearAgentPermissions()
    {
        AgentIDToMovementCapabilities.Clear();
        AgentIDToSensorCapabilities.Clear();
    }
    public bool TryAddVertex(Point location, SpotState state)
    {
        if (Search(location) != null)
        {
            return false;
        }
        Vertices.Add(new(location, state));
        return true;
    }
    public bool TryAddUndirectedEdge(Point from, Point end, int cost = 1)
    {
        var v1 = Search(from);
        var v2 = Search(end);
        if (v1 == null || v2 == null)
        {
            return false;
        }
        v1.AddEdge(v2, cost);
        v2.AddEdge(v1, cost);
        return true;
    }

    public bool TryAddDirectedEdge(Point from, Point end, int cost = 1)
    {
        var v1 = Search(from);
        var v2 = Search(end);
        if (v1 == null || v2 == null)
        {
            return false;
        }
        v1.AddEdge(v2, cost);
        return true;
    }

    public Vertex? Search(Point location) => Vertices.Find(x => x.Location == location);

    public override List<(int moveId, double chance)> GetMoves(EnvironmentState state, int agentID)
    {
        if (!IDToAgent.ContainsKey(agentID)) throw new ArgumentException("Agent is not registered");

        if (!AgentIDToMovementCapabilities.ContainsKey(agentID)) throw new ArgumentException("Movements are not registered for the ID");

        var movementPermission = AgentIDToMovementCapabilities[agentID];

         
    }



    /*
        public override List<Succesor<EnvironmentState>> G(EnvironmentState state, int agentID)
        {
            Vertex robotVertex = Search(state.RobotLocation) ?? throw new NullReferenceException("Robot has to be in the environment");

            List<Succesor<EnvironmentState>> succesors = [];

            var movementPermissions = AgentIDToMovementCapabilities[agentID];
            for (int i = 1; i < 2; i <<= 1)
            {
                MovementPermissions permission = (MovementPermissions)i;
                if (movementPermissions.HasFlag(permission))
                {
                    var movement = MovementCapabilities[movementPermissions];
                    var newStates = movement(state);
                    foreach (var nextState in newStates)
                    {
                        Dictionary<EnvironmentState, double> chance = new()
                        {
                            { nextState, 1 },
                        };
                        Succesor<EnvironmentState> succesor = new(state, chance);
                        succesors.Add(succesor);
                    }
                }
            }
            return succesors;


        }*/


    //no chance for environment right now (will add later


}
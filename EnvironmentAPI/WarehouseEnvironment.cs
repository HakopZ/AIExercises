using System.Drawing;
using AIWorldLibrary;
using EnvironmentAPI;

namespace EnvironmentAPI;

[Flags]
public enum MovementPermissions
{
    None = 0,
    Directional = 1 << 0,
    Diagonal = 1 << 1,
}

[Flags]
public enum SensorPermissions
{
    None = 0,
    Position = 1 << 0
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

public class WarehouseEnvironment : AIEnvironment<EnvironmentState, MovementPermissions, SensorPermissions>
{
    public override bool IsStatic => false;
    public override bool IsDeterministic => true;
    public List<Vertex> Vertices { get; set; } = [];
    public List<Edge> Edges { get; set; } = [];

    public override Dictionary<SensorPermissions, Func<EnvironmentState, object?>> SensorCapabilities => sensorCapabilties;

    public override Dictionary<MovementPermissions, Func<EnvironmentState, List<EnvironmentState>>> MovementCapabilities => movementCapabilities;
    public Dictionary<SensorPermissions, Func<EnvironmentState, object?>> sensorCapabilties = new()
    {
        { SensorPermissions.None, (state) => null },
        { SensorPermissions.Position, (state) => state.RobotLocation } //issue with this ask stan, techincally agent already has this, but I don't want them to, so should make move be null?
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
    public override int FindLookAheadAmount(EnvironmentState state)
    {
        throw new NotImplementedException();
    }
    
    public override List<Succesor<EnvironmentState>> GetSuccesors(EnvironmentState state, int agentID)
    {
        Vertex robotVertex = Search(state.RobotLocation) ?? throw new NullReferenceException("Robot has to be in the environment");

        List<Succesor<EnvironmentState>> succesors = [];
        
        var movementPermissions = AgentIDToMovementCapabilities[agentID];
        for (int i = 1; i < 2; i <<= 1)
        {
            MovementPermissions permission = (MovementPermissions)i;
            if(movementPermissions.HasFlag(permission))
            {
                var movement = MovementCapabilities[movementPermissions];
                var newStates = movement(state);
                foreach(var nextState in newStates)
                {
                    Dictionary<EnvironmentState, double> chance = new()
                    {
                        { nextState, 1 },
                    };
                    Succesor<EnvironmentState> succesor = new (state, chance);
                    succesors.Add(succesor);
                }
            }
        } 
        return succesors;

        /*foreach (var edge in robotVertex.Edges)
        {
            EnvironmentState newState = new(edge.To.Location);
            Dictionary<EnvironmentState, double> map = new()
            {
                {newState, 1}
            };
            Succesor<EnvironmentState> succesor = new(state, map);
            succesors.Add(succesor);
        }

        return succesors;*/
    }


    //no chance for environment right now (will add later)
    public override EnvironmentState MakeMove(EnvironmentState state)
    {
        return state;
    }
}
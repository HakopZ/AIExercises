using System.Drawing;
using AIWorldLibrary;
namespace Environments;


public class WarehouseEnvironmentState(Point robotLocation, int score) : IState
{
    public Point RobotLocation { get; set; } = robotLocation;
    public int Score { get; set; } = score;

}
public class WarehouseMovementPermissions : BetterEnum
{
    private int _value = 0;
    public override int Value
    {
        get => _value;
        set
        {
            if (value >= 0 && value < 1 << 2)
            {
                _value = value;
            }
        }
    }

    public override BetterEnum And(int val)
    {
        var mP = new WarehouseMovementPermissions
        {
            Value = Value & val
        };
        return mP;
    }
    public override bool Equals(object? obj)
    {
        var t = obj as WarehouseMovementPermissions;
        return t?._value == _value;
    }

    public override int GetHashCode()
    {
        return _value;
    }
}
public class WarehouseSensorPermissions : BetterEnum
{
    private int _value;
    public WarehouseSensorPermissions()
    {
        _value = 0;
    }
    public override int Value
    {
        get => _value;
        set
        {
            if (value >= 0 && value < 1 << 2)
            {
                _value = value;
            }
        }
    }

    public override BetterEnum And(int val)
    {
        var mP = new WarehouseMovementPermissions
        {
            Value = Value & val
        };
        return mP;
    }

    public override bool Equals(object? obj)
    {
        var t = obj as WarehouseSensorPermissions;
        return t?._value == _value;
    }

    public override int GetHashCode()
    {
        return _value;
    }
}
public enum SpotState
{
    Empty = 0,
    Obstacle = -1,
    Item = 1,
    Robot = -1
};
public class WarehouseVertex()
{
    public SpotState SpotState { get; set; }
}
public class Edge(WarehouseVertex from, WarehouseVertex to, int cost = 1)
{
    public WarehouseVertex From { get; set; } = from;
    public WarehouseVertex To { get; set; } = to;
    public int Cost { get; set; } = cost;
}



public record SpotStateInfo(double SpotScore) : ISensorData;
public record PointInfo(Point Location) : ISensorData;
public record Nothing : ISensorData;

public class WarehouseEnvironment : AIEnvironment<WarehouseEnvironmentState, WarehouseMovementPermissions, WarehouseSensorPermissions>
{

    public override bool IsStatic => true;
    public override bool IsDeterministic => true;
    private static WarehouseVertex[,] Vertices = new WarehouseVertex[6, 6];
    public Size GraphSize;
    public List<Point> GoalSpots = [];
    public List<Point> FireSpots = [];
    private readonly static AIMove Up = new(new WarehouseMovementPermissions() { Value = 1 << 0 }, 1, (state) => new WarehouseEnvironmentState(new(state.RobotLocation.X, state.RobotLocation.Y - 1), (int)Vertices[state.RobotLocation.X, state.RobotLocation.Y - 1].SpotState));
    private readonly static AIMove Down = new(new WarehouseMovementPermissions() { Value = 1 << 1 }, 1, (state) => new WarehouseEnvironmentState(new Point(state.RobotLocation.X, state.RobotLocation.Y + 1), (int)Vertices[state.RobotLocation.X, state.RobotLocation.Y + 1].SpotState));
    private readonly static AIMove Left = new(new WarehouseMovementPermissions() { Value = 1 << 2 }, 1, (state) => new WarehouseEnvironmentState(new Point(state.RobotLocation.X - 1, state.RobotLocation.Y), (int)Vertices[state.RobotLocation.X - 1, state.RobotLocation.Y].SpotState));
    private readonly static AIMove Right = new(new WarehouseMovementPermissions() { Value = 1 << 3 }, 1, (state) => new WarehouseEnvironmentState(new Point(state.RobotLocation.X + 1, state.RobotLocation.Y), (int)Vertices[state.RobotLocation.X + 1, state.RobotLocation.Y].SpotState));
    private readonly static AIMove TL = new(new WarehouseMovementPermissions() { Value = 1 << 4 }, 1, (state) => new WarehouseEnvironmentState(new Point(state.RobotLocation.X - 1, state.RobotLocation.Y - 1), (int)Vertices[state.RobotLocation.X - 1, state.RobotLocation.Y - 1].SpotState));
    private readonly static AIMove TR = new(new WarehouseMovementPermissions() { Value = 1 << 5 }, 1, (state) => new WarehouseEnvironmentState(new Point(state.RobotLocation.X + 1, state.RobotLocation.Y - 1), (int)Vertices[state.RobotLocation.X + 1, state.RobotLocation.Y - 1].SpotState));
    private readonly static AIMove BL = new(new WarehouseMovementPermissions() { Value = 1 << 6 }, 1, (state) => new WarehouseEnvironmentState(new Point(state.RobotLocation.X - 1, state.RobotLocation.Y + 1), (int)Vertices[state.RobotLocation.X - 1, state.RobotLocation.Y + 1].SpotState));
    private readonly static AIMove BR = new(new WarehouseMovementPermissions() { Value = 1 << 7 }, 1, (state) => new WarehouseEnvironmentState(new Point(state.RobotLocation.X + 1, state.RobotLocation.Y + 1), (int)Vertices[state.RobotLocation.X + 1, state.RobotLocation.Y + 1].SpotState));



    public override Dictionary<WarehouseSensorPermissions, Func<WarehouseEnvironmentState, ISensorData>> SensorCapabilities => new()
    {
        { new WarehouseSensorPermissions() { Value = 0 }, (state) => new Nothing() },

        { new WarehouseSensorPermissions() { Value = 1 << 0 }, (state) => new PointInfo(state.RobotLocation) },

        { new WarehouseSensorPermissions() {Value = 1 << 1 }, (state) => new SpotStateInfo(state.Score) }
    };

    public override Dictionary<WarehouseMovementPermissions, List<AIMove>> MovementCapabilities => new()
    {
        { new WarehouseMovementPermissions() { Value = 0 }, [null] },

        { new WarehouseMovementPermissions() { Value = 1 }, [Up, Down, Left, Right]},

        { new WarehouseMovementPermissions() { Value = 1 << 1 }, [TL, TR, BL, BR] }
    };

    public override Dictionary<int, AIMove> IdToMove => new()
    {
        { 1, Up },
        { 2, Down },
        { 3, Left },
        { 4, Right },
        { 5, TL },
        { 6, TR },
        { 7, BL },
        { 8, BR },
    };
    public override Dictionary<AIMove, int> MoveToID => new()
    {
        { Up, 1 },
        { Down, 2 },
        { Left, 3 },
        { Right, 4},
        { TL, 5 },
        { TR, 6 },
        { BL, 7 },
        { BR, 8 }
    };

    public override WarehouseEnvironmentState DefaultState => new(new(1, 1), 0);

    public override WarehouseEnvironmentState ResetState 
    {
        get
        {
            var point = new Point(Random.Shared.Next(1, GraphSize.Width+1), Random.Shared.Next(1, GraphSize.Height+1));
            return new(point, (int)Vertices[point.X, point.Y].SpotState);
        }
    }   

    public WarehouseEnvironment(List<Point> goalSpots, int width = 4, int height = 4)
    {
        GraphSize = new Size(width, height);
        Vertices = new WarehouseVertex[width+2, height+2];
        GoalSpots = goalSpots;
        BuildGraph(width + 2, height + 2);
    }
    private void BuildGraph(int width, int height)
    {
        ClearEnvironment();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || j == 0 || i == GraphSize.Width + 1 || j == GraphSize.Height + 1)
                {
                    TryAddVertex(new(i, j), SpotState.Obstacle);
                }
                else if (!GoalSpots.Contains(new Point(i, j)))
                {
                    TryAddVertex(new(i, j), SpotState.Empty);
                }
                else
                {
                    TryAddVertex(new(i, j), SpotState.Item);
                }
            }
        }
    }
    public void ClearEnvironment()
    {
        Vertices = new WarehouseVertex[GraphSize.Width+2, GraphSize.Height+2];
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
        Vertices[location.X, location.Y] = new() { SpotState = state };
        return true;
    }


    public WarehouseVertex? Search(Point location)
    {
        if (location.X >= Vertices.GetLength(0) || location.X < 0 || location.Y < 0 || location.Y >= Vertices.GetLength(1)) return null;

        return Vertices[location.X, location.Y];
    }

    public override List<AIMove> GetMovesFromState(WarehouseEnvironmentState state)
    {
        List<AIMove> moves = [];

        if (state.RobotLocation.X > 0) moves.Add(Left);

        if (state.RobotLocation.X < GraphSize.Width - 1) moves.Add(Right);

        if (state.RobotLocation.Y > 0) moves.Add(Up);

        if (state.RobotLocation.Y < GraphSize.Height - 1) moves.Add(Down);

        return moves;
    }

    public override bool IsTerminalMove(WarehouseEnvironmentState state)
    {
        
        if (Vertices[state.RobotLocation.X, state.RobotLocation.Y].SpotState == SpotState.Obstacle)
        {
            return true;
        }
        if(Vertices[state.RobotLocation.X, state.RobotLocation.Y].SpotState == SpotState.Item)
        {
            return true;
        }
        return false;
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
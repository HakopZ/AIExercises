using System.Drawing;
using AIWorldLibrary;

namespace Environments;

public class GhostEnvironmentState : IState
{
    public Point RobotLocation { get; set; }
}
public class GhostVertex
{
    public bool IsDot { get; }
}

public class GhostMovementPermissions : BetterEnum
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
        var mP = new GhostMovementPermissions
        {
            Value = Value & val
        };
        return mP;
    }
    public override bool Equals(object? obj)
    {
        var t = obj as GhostMovementPermissions;
        return t?._value == _value;
    }

    public override int GetHashCode()
    {
        return _value;
    }
}

public class GhostSensorPermissions : BetterEnum
{
    private int _value;
    public GhostSensorPermissions()
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
        var mP = new GhostSensorPermissions
        {
            Value = Value & val
        };
        return mP;
    }

    public override bool Equals(object? obj)
    {
        var t = obj as GhostSensorPermissions;
        return t?._value == _value;
    }

    public override int GetHashCode()
    {
        return _value;
    }
}
public record DotInfo(bool IsDot) : ISensorData;
//public record SpotState
public class GhostEnvironment : AIEnvironment<GhostEnvironmentState, GhostMovementPermissions, GhostSensorPermissions>
{
    public override Dictionary<GhostSensorPermissions, Func<GhostEnvironmentState, ISensorData>> SensorCapabilities => new()
     {
        { new GhostSensorPermissions() { Value = 0 }, (state) => new Nothing() },

        { new GhostSensorPermissions() {Value = 1 << 1 }, (state) => new DotInfo(verticies[state.RobotLocation.X, state.RobotLocation.Y].IsDot) }
    };

    public override Dictionary<GhostMovementPermissions, List<AIMove>> MovementCapabilities => throw new NotImplementedException();
    public GhostVertex[,] verticies = new GhostVertex[5, 5];
    public override Dictionary<int, AIMove> IdToMove => throw new NotImplementedException();

    public override Dictionary<AIMove, int> MoveToID => throw new NotImplementedException();

    public override bool IsStatic => throw new NotImplementedException();

    public override bool IsDeterministic => throw new NotImplementedException();

    public override GhostEnvironmentState DefaultState => throw new NotImplementedException();

    public override GhostEnvironmentState ResetState => throw new NotImplementedException();

    public override List<AIMove> GetMovesFromState(GhostEnvironmentState state)
    {
        throw new NotImplementedException();
    }

    public override bool IsTerminalMove(GhostEnvironmentState state)
    {
        throw new NotImplementedException();
    }
}


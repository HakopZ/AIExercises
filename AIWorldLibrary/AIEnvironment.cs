using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace AIWorldLibrary;
public interface IState
{
}

public interface ISensorData
{

}

public abstract class BetterEnum
{
    public abstract int Value { get; set; }

    public abstract BetterEnum And(int val);

    public virtual bool HasFlag(BetterEnum v)
    {
        return (Value & v.Value) == v.Value;
    }

}
[Serializable]
public struct MoveReturn(int moveID, double chance)
{
    public int MoveID { get; set; } = moveID;
    public double Chance { get; set; } = chance;
}

public abstract class AIEnvironment<TEnvironmentState, TMovementPermissions, TSensorPermissions>
    where TEnvironmentState : IState
    where TMovementPermissions : BetterEnum
    where TSensorPermissions : BetterEnum
{

    public class AIMove(BetterEnum fromPermission, double chance, Func<TEnvironmentState, TEnvironmentState> move)
    {
        public BetterEnum FromPermissions { get; } = fromPermission;
        public double Chance { get; } = chance;
        public Func<TEnvironmentState, TEnvironmentState> InvokeMove => move;
    }
    public Dictionary<int, TSensorPermissions> AgentIDToSensorCapabilities = [];
    public Dictionary<int, TMovementPermissions> AgentIDToMovementCapabilities = [];
    public HashSet<int> AgentIDs = [];
    public Dictionary<int, TEnvironmentState> AgentIDToState = [];
    public abstract Dictionary<TSensorPermissions, Func<TEnvironmentState, ISensorData>> SensorCapabilities { get; }
    public abstract Dictionary<TMovementPermissions, List<AIMove>> MovementCapabilities { get; }
    public abstract Dictionary<int, AIMove> IdToMove { get; }
    public abstract Dictionary<AIMove, int> MoveToID { get; }
    public abstract bool IsStatic { get; }
    public abstract bool IsDeterministic { get; }
    public abstract TEnvironmentState DefaultState { get; }
    public abstract TEnvironmentState ResetState { get; }
    public int SensorCount;
    public int MovementCount;
    public AIEnvironment(int movementCount = 3, int sensorCount = 3)
    {
        MovementCount = movementCount;
        SensorCount = sensorCount;

    }
    public abstract List<AIMove> GetMovesFromState(TEnvironmentState state);
    public virtual List<MoveReturn> GetMoves(TEnvironmentState state, int agentID) //might change cause different agents with different choices could do different things when it comes to succesors at a state 
    {

        if (!AgentIDToMovementCapabilities.TryGetValue(agentID, out TMovementPermissions? movementPermissions)) throw new ArgumentException("Movements are not registered for the ID");

        List<MoveReturn> result = [];
        List<AIMove> permissionMoves = [];
        for (int i = 0; i < MovementCount; i++)
        {
            var permission = (TMovementPermissions)movementPermissions.And(1 << i);
            if (permission.Value != 0)
            {
                permissionMoves.AddRange(MovementCapabilities[permission]);
            }
        }
        var allMoves = GetMovesFromState(state);
        var r = allMoves.Intersect(permissionMoves);
        result.AddRange(r.Select(x => new MoveReturn(MoveToID[x], x.Chance)));

        return result;
    }
    public virtual List<MoveReturn> GetMoves(int agentID)
    {
        if (AgentIDToState.TryGetValue(agentID, out var result))
            return GetMoves(result, agentID);
        throw new Exception("agentID not in AgentIDToState");
    }
    public virtual ISensorData? GetSensorData(int agentID, TSensorPermissions sensorPermission)
        => SensorCapabilities[sensorPermission].Invoke(AgentIDToState[agentID]);
    public abstract bool IsTerminalMove(TEnvironmentState state);
    public virtual bool MakeMove(int moveID, int agentID)
    {
        if (IsTerminalMove(AgentIDToState[agentID]))
        {

            AgentIDToState[agentID] = ResetState;
            return false;
        }

        var move = IdToMove[moveID];
        var currentState = move.InvokeMove(AgentIDToState[agentID]);

        AgentIDToState[agentID] = currentState;


        return true;
    }


    public virtual int RegisterAgent()
    {
        //cheat for now
        int id = AgentIDs.Count == 0 ? 0 : AgentIDs.Last() + 1;
        AgentIDs.Add(id);
        AgentIDToState.Add(id, DefaultState);
        return id;

    }
    public virtual void RegisterAgentSensorPermission(int agentID, TSensorPermissions sensorPermissions)
    {
        if (AgentIDToSensorCapabilities.TryGetValue(agentID, out TSensorPermissions? value))
            value.Value |= sensorPermissions.Value;
        else
        {
            AgentIDToSensorCapabilities.Add(agentID, sensorPermissions);
        }
    }

    public virtual void RegisterAgentMovementPermission(int agentID, TMovementPermissions movementPermissions)
    {
        if (AgentIDToMovementCapabilities.TryGetValue(agentID, out TMovementPermissions? value))
            value.Value |= movementPermissions.Value;
        else
        {
            AgentIDToMovementCapabilities.Add(agentID, movementPermissions);
        }
    }

    public TMovementPermissions GetMovementPermissions(int agentID)
        => AgentIDToMovementCapabilities[agentID];
    public TSensorPermissions GetSensorPermissions(int agentID)
        => AgentIDToSensorCapabilities[agentID];


}

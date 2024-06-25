using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace AIWorldLibrary;
public interface IState
{
    //TAgentData Data { get; set; } //might get rid of
    //public int Score { get; set; }
}

public interface ISensorData
{

}

public abstract class BetterEnum
{
    public abstract int Value { get; set; }

    public abstract BetterEnum And(int val);
    
}
// public interface IVertex<TState>
//     where TState : IState
// {
//     public TState State { get; set; }
//     public List<Succesor<TState>> Successors { get; set; }
// }

public abstract class AIEnvironment<TEnvironmentState, TMovementPermissions, TSensorPermissions>
    where TEnvironmentState : IState
    where TMovementPermissions : BetterEnum
    where TSensorPermissions : BetterEnum
{

    public Dictionary<int, TSensorPermissions> AgentIDToSensorCapabilities = [];
    public Dictionary<int, TMovementPermissions> AgentIDToMovementCapabilities = [];
    public Dictionary<Agent, int> AgentToID = [];
    public Dictionary<int, Agent> IDToAgent = [];
    public Dictionary<int, TEnvironmentState> AgentToState = [];
    public abstract Dictionary<TSensorPermissions, Func<TEnvironmentState, ISensorData>> SensorCapabilities { get; }
    public abstract Dictionary<TMovementPermissions, Func<TEnvironmentState, List<TEnvironmentState>>> MovementCapabilities { get; }
    
    public abstract bool IsStatic { get; }
    public abstract bool IsDeterministic { get; }
    public int SensorCount; 
    public int MovementCount;
    public AIEnvironment()
    {
        MovementCount = typeof(TMovementPermissions).GetFields(BindingFlags.Static).Count(x => x.FieldType == typeof(TMovementPermissions));
        SensorCount = typeof(TSensorPermissions).GetFields(BindingFlags.Static).Count(x => x.FieldType == typeof(TSensorPermissions));

    }
    public abstract List<(int moveId, double chance)> GetMoves(TEnvironmentState state, int agentID); //might change cause different agents with different choices could do different things when it comes to succesors at a state 
    public virtual List<ISensorData> GetSensorData(int agentID) 
    {
        if (!IDToAgent.ContainsKey(agentID)) throw new ArgumentException("Agent has not been registered");

        if (!AgentIDToSensorCapabilities.ContainsKey(agentID)) throw new ArgumentException("Agent has not registered sensor capabilities");
        
        List<ISensorData> sensorData = [];
        var sensor = AgentIDToSensorCapabilities[agentID];
        for(int i = 0; i < SensorCount; i++)
        {
            TSensorPermissions permission = (TSensorPermissions)sensor.And(1 << i);
            if (permission.Value != 0)
            {
                var data = SensorCapabilities[permission](AgentToState[agentID]);
                sensorData.Add(data);
            }
        }
        return sensorData;
    }
    public virtual void MakeMove(TEnvironmentState state, int agentID)
    {
        AgentToState[agentID] = state;
    }  
        

    public virtual int RegisterAgent(Agent agent)
    {
        if (AgentToID.ContainsKey(agent)) throw new ArgumentException("Agent already has registered");

        //cheat for now
        int id = AgentToID.Last().Value + 1;
        AgentToID.Add(agent, id);
        IDToAgent.Add(id, agent);
        return id;

    }
    public virtual void RegisterAgentSensorPermissions(int agentID, TSensorPermissions sensorPermissions)
    {
        if (!SensorCapabilities.ContainsKey(sensorPermissions)) throw new ArgumentException("Sensor Capability does not exist");

        if (!IDToAgent.ContainsKey(agentID)) throw new ArgumentException("Agent is not registered");

        if (AgentIDToSensorCapabilities.ContainsKey(agentID)) throw new ArgumentException("Agnet already has registered sensor permissions");

        AgentIDToSensorCapabilities[agentID] = sensorPermissions;

    }

    public virtual void RegisterAgentMovementPermissions(int agentID, TMovementPermissions movementPermissions)
    {
        if (!MovementCapabilities.ContainsKey(movementPermissions)) throw new ArgumentException("Movement Capability does not exist");

        if (!IDToAgent.ContainsKey(agentID)) throw new ArgumentException("Agent is not registered");

        if (AgentIDToMovementCapabilities.ContainsKey(agentID)) throw new ArgumentException("Agent already has registered movement permissions");

        AgentIDToMovementCapabilities[agentID] = movementPermissions;
    }
    public (TMovementPermissions, TSensorPermissions) GetPermissions(int agentID)
    {
        if (!IDToAgent.ContainsKey(agentID)) throw new ArgumentException("Agent is not registered");

        if (!AgentIDToMovementCapabilities.ContainsKey(agentID)) throw new ArgumentException("Agent movement capabilities is not available");

        if (!AgentIDToSensorCapabilities.ContainsKey(agentID)) throw new ArgumentException("Agent sensor capabilities is not available");

        return (AgentIDToMovementCapabilities[agentID], AgentIDToSensorCapabilities[agentID]);
    }


    // public TState RunAgent(Agent<TStateValue, TCost> agent,
    //                                             AIEnvironment<TStateValue> environment)
    // {

    //     if (agent.CurrentState.EnvironmentState.Equals(environment.GoalState)) return agent.CurrentState.EnvironmentState;

    //     int lookAhead = Math.Min(agent.LookAhead, environment.FindLookAheadAmount(agent.CurrentState.EnvironmentState));

    //     var nextBestState = agent.SelectState(lookAhead);

    //     var newEnvironmentState = environment.MakeMove(nextBestState.EnvironmentState);

    //     var actualState = agent.Move(newEnvironmentState); //agent move is pointless
    //     TCost totalCost = agent.CurrentState.CumulativeCost + agent.GetCost(agent.CurrentState.EnvironmentState, actualState);

    //     var agentState = new AgentState<TStateValue, TCost>(actualState, totalCost, agent.CurrentState);

    //     var sucessors = environment.GetSuccesors(actualState);

    //     agent.AddSuccessors(sucessors, environment.GoalState);

    //     agent.Frontier.Clear(); //this might change due to the fact that maybe you don't wan't frontier clear on planning agent

    //     agent.CurrentState = agentState;

    //     return newEnvironmentState;
    // }
}

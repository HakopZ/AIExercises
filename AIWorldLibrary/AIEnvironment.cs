using System.Numerics;
using System.Reflection;

namespace AIWorldLibrary;
public interface IState
{
    //TAgentData Data { get; set; } //might get rid of
    public int Score { get; set; }
}
// public interface IVertex<TState>
//     where TState : IState
// {
//     public TState State { get; set; }
//     public List<Succesor<TState>> Successors { get; set; }
// }

public abstract class AIEnvironment<TEnvironmentState>
    where TEnvironmentState : IState
{
    public abstract bool isStatic { get; }
    public abstract bool isDeterministic { get; }
    public abstract TEnvironmentState MakeMove(TEnvironmentState state);  //not sure if needed
    public abstract List<Succesor<TEnvironmentState>> GetSuccesors(TEnvironmentState state); //might change cause different agents with different choices could do different things when it comes to succesors at a state
    public abstract int FindLookAheadAmount(TEnvironmentState state);


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

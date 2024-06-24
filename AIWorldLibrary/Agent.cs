using System.Data.SqlTypes;
using System.Numerics;
using AIWorldLibrary;

namespace AIWorldLibrary;
public abstract class Agent<TEnvironmentState, TCost>
    where TEnvironmentState : IState
    where TCost : INumber<TCost>, INullable
{
    public int LookAhead;
    public AgentState<TEnvironmentState, TCost> CurrentState { get; set; }
    public IFrontier<AgentState<TEnvironmentState, TCost>, TCost> Frontier { get; set; }
    public Queue<AgentState<TEnvironmentState, TCost>> pathToGo = [];
    public Dictionary<TEnvironmentState, TCost> Visited { get; set; }
    public abstract TCost GetCost(TEnvironmentState state, TEnvironmentState state2);
    public abstract void AddSuccessors(List<Succesor<TEnvironmentState>> succesors, TEnvironmentState GoalState);
    public abstract AgentState<TEnvironmentState, TCost> SelectState(int lookAhead);
    public Agent(int lookAhead = 1)
    {
        LookAhead = lookAhead;
    }

}
using System.Data.SqlTypes;
using System.Numerics;
using AIWorldLibrary;

namespace AIWorldLibrary;

public class AgentState<TEnvironmentState, TCost>
    where TEnvironmentState : IState
    where TCost : INumber<TCost>
{
    public TEnvironmentState EnvironmentState { get; set; } = default;
    public AgentState<TEnvironmentState, TCost> Previous { get; set; } = default;
    public TCost CumulativeCost { get; set; }
    public AgentState(TEnvironmentState envState, TCost cumulativeCost, AgentState<TEnvironmentState, TCost> prev)
    {
        EnvironmentState = envState;
        Previous = prev;
        CumulativeCost = cumulativeCost;
    }
}
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
    public abstract void AddSuccessors(List<Succesor<TEnvironmentState>> succesors);
    public abstract TEnvironmentState Move(TEnvironmentState state);

    public abstract Queue<AgentState<TEnvironmentState, TCost>> Plan(TEnvironmentState state, int lookAmount);
    public virtual AgentState<TEnvironmentState, TCost> SelectState(int lookAhead)
    {
        if (pathToGo.Count == 0 || pathToGo.Peek().Previous != CurrentState)
        {
            pathToGo = Plan(CurrentState.EnvironmentState, lookAhead);
        }

        return pathToGo.Dequeue();
    }
    public Agent(int lookAhead = 1)
    {
        LookAhead = lookAhead;
    }

}
using System.Data.SqlTypes;
using System.Numerics;

namespace AIWorldLibrary;

public abstract class PlannerAgent<TEnvironmentState, TCost> : Agent<TEnvironmentState, TCost>
    where TEnvironmentState : IState
    where TCost : INumber<TCost>, INullable
{
    public abstract TCost GetHeuristic(TEnvironmentState state, TEnvironmentState goalState);
    public abstract Queue<AgentState<TEnvironmentState, TCost>> Plan(TEnvironmentState state, int lookAmount);

    public override AgentState<TEnvironmentState, TCost> SelectState(int lookAhead)
    {
        if (pathToGo.Count == 0 || pathToGo.Peek().Previous != CurrentState)
        {
            pathToGo = Plan(CurrentState.EnvironmentState, lookAhead);
        }

        return pathToGo.Dequeue();
    }
}

using System.Numerics;

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

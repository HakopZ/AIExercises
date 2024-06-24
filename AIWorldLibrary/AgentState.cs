using System.Numerics;

namespace AIWorldLibrary;

public class AgentState<TEnvironmentState, TCost> (TEnvironmentState envState, TCost cumulativeCost, AgentState<TEnvironmentState, TCost> prev)
    where TEnvironmentState : IState
    where TCost : INumber<TCost>
{
    public TEnvironmentState EnvironmentState { get; set; } = envState;
    public AgentState<TEnvironmentState, TCost> Previous { get; set; } = prev;
    public TCost CumulativeCost { get; set; } = cumulativeCost;
}

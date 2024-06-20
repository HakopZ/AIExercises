using System.Numerics;
using AIWorldLibrary;

public class AgentState<TEnvironmentState, TScore>
    where TEnvironmentState : IState
    where TScore : IComparable<TScore>
{

    public TEnvironmentState EnvironmentState { get; set; } = default;
    public AgentState<TEnvironmentState, TScore> Previous { get; set; } = default;
    //public float Priority { get; set; }
    private object CumulativeDistance { get;  set; }
    public TCost GetCumulativeDistance<TCost>() where TCost : INumber<TCost> => (TCost)CumulativeDistance;
    public void SetCumulativeDistnace<TCost>(TCost cost) where TCost : INumber<TCost> => CumulativeDistance = cost;
    public void AddCumulativeDistance<TCost>(TCost cost) where TCost : INumber<TCost> => SetCumulativeDistnace(cost + GetCumulativeDistance<TCost>());
    public TScore Score { get; set; } = default;
}
public abstract class Agent<TEnvironmentState, TCost, TScore>
    where TEnvironmentState : IState
    where TScore : IComparable<TScore>
{
    public AgentState<TEnvironmentState, TScore> CurrentState { get; set; }
    public IFrontier<TEnvironmentState> Frontier { get; set; }
    public abstract float Heursitic(TEnvironmentState state, TEnvironmentState goal);
}
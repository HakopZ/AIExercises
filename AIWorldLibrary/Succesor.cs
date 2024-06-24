namespace AIWorldLibrary;

public class Succesor<TState>
    where TState : IState
{
    public TState FromState { get; set; }
    public Dictionary<TState, double> ToStates { get; set; }
    public double Value => ToStates.Sum(x => x.Key.Score * x.Value);
    public Succesor(TState from, Dictionary<TState, double> movements = default)
    {
        FromState = from;
        ToStates = movements;
    }
    public void AddEndState(TState end, double chance) => ToStates.Add(end, chance);

}

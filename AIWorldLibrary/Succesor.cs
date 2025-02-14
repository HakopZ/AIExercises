namespace AIWorldLibrary;

public class Succesor<TState>
    where TState : IState
{
    public TState FromState { get; set; }
    public Dictionary<TState, double>? ToStates { get; set; }
    public bool isTerminal => ToStates == null || ToStates.Count == 0;
    public double Value => 0;
    public Succesor(TState from, Dictionary<TState, double>? movements = null)
    {
        FromState = from;
        ToStates = movements;
    }
    public void AddEndState(TState end, double chance) => ToStates?.Add(end, chance);

}

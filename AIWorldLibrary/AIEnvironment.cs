using System.Numerics;
using System.Reflection;

namespace AIWorldLibrary;



public class Succesor<TState>
    where TState : IState
{
    public IVertex<TState> FromVertex { get; set; }
    public IVertex<TState> ToVertex { get; set; }
    public double Chance { get; set; }
    public Succesor(IVertex<TState> from, IVertex<TState> to, double chance)
    {
        FromVertex = from;
        ToVertex = to;
    }

}
public interface IState { }
public interface IVertex<TState>
    where TState : IState
{
    public TState State { get; set; }
    public List<Succesor<TState>> Successors { get; set; }
}

public abstract class AIEnvironment<TState>
    where TState : IState
{
    public abstract bool isStatic { get; }
    public abstract bool isDeterministic { get; }
    public abstract IVertex<TState> GoalState { get; }
    public abstract TState MakeMove(TState state);  //not sure if needed
    public abstract List<Succesor<TState>> GetSuccesors(TState state); //might change cause different agents with different choices could do different things when it comes to succesors at a state
    public abstract int FindLookAheadAmount(TState state);
}

using System.Numerics;
using System.Reflection;

namespace AIWorldLibrary;



public class Succesor<TState>
    where TState : IState
{
    public IVertex<TState> FromVertex { get; set; }
    public IVertex<TState> ToVertex { get; set; }
    public Dictionary<Type, object> CostGenerator { get; private set; }
    public Succesor(IVertex<TState> from, IVertex<TState> to)
    {
        FromVertex = from;
        ToVertex = to;
    }
    public TCost GetCost<TCost>(Type agentType)
            where TCost : INumber<TCost>
    {
        return (TCost)CostGenerator[agentType];
    }

}
public interface IState { }
public interface IVertex<TState>
    where TState : IState
{
    public TState Value { get; set; }
    public List<Succesor<TState>> Successors { get; set; }
}

public abstract class AIEnvironment<TState>
    where TState : IState
{
    public abstract bool isFullyObservable { get; }
    public abstract bool isStatic { get; }
    public abstract bool isDeterministic { get; }

    public abstract IVertex<TState> GoalState { get; }
    public Dictionary<Type, Type> AgentTypesToCostType = [];
    public void RegisterCostType<T>(Type agentType)
        where T : INumber<T>
    {
        if (AgentTypesToCostType.ContainsKey(agentType))
        {
            throw new InvalidOperationException("Type already registered");
        }

        AgentTypesToCostType.Add(agentType, typeof(T));
    }

    public abstract IVertex<TState> MakeMove(IVertex<TState> state);  //not sure if needed
    public abstract List<Succesor<TState>> GetSuccesors(TState state, Type agentType); //based on the agentType and the state, get successors, either knowing the whole graph or not

    public void AddEdge<TCost>(IVertex<TState> from, IVertex<TState> to, Type agentType, TCost cost)
        where TCost : INumber<TCost>
    {
        if (!AgentTypesToCostType.TryGetValue(agentType, out var c) || c != cost.GetType())
        {
            throw new InvalidOperationException("This agent type is not registered with this cost type");
        }
        //check if a succesor exists
        var result = from.Successors.Where(x => x.ToVertex == to);
        if (result.Any())
        {
            result.First().CostGenerator.Add(agentType, cost);
        }
        else
        {
            Succesor<TState> succesor = new Succesor<TState>(from, to);
            succesor.CostGenerator.Add(agentType, cost);
            from.Successors.Add(succesor);
        }
    }


}

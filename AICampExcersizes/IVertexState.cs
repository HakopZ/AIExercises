using System.Dynamic;
using System.Numerics;


public interface ITransitionCreator
{
    T Create<T>(T input)
        where T : INumber<T>;
}
public class Succesor<TValueType>
    where TValueType : IVertexState<TValueType>
{
    public INode<TValueType> FromVertex { get; set; }
    public INode<TValueType> ToVertex { get; set; }
    public Dictionary<Type, ITransitionCreator> Edges { get; set; }
    public Succesor(INode<TValueType> from, INode<TValueType> to, IDictionary<Type, ITransitionCreator> edges = default)
    {
        Edges = edges.ToDictionary();
    }
    // public void AddEdge(IAgentType type, ITransitionCreator edge) => Edges.Add(type, edge);
}
public class IAgentType
{
    public string TypeValue { get; set; }
}
public interface IVertexState<TStateValue>
{
    TStateValue Value { get; set; }

}

public interface INode<TValueType>
    where TValueType : IVertexState<TValueType>
{

    TValueType Value { get; set; }
    public List<Succesor<TValueType>> Successors { get; set; }
}

using System.Numerics;



public abstract class Environment<TState>
    where TState : IVertexState<TState>, IEquatable<TState>
{

    protected List<INode<TState>> nodes { get; }
    public abstract IVertexState<TState> MakeMove(IVertexState<TState> state);  //not sure if needed
    public abstract List<IVertexState<TState>> GetSuccesors(IAgent agent);
    //could be multiple goal states
    public IVertexState<TState> GoalState { get; }
    public Dictionary<Type, ITransitionCreator> TransitionCreators { get; set; }
    public abstract void BuildOutGrid();

    public void Register<TCostType>(IAgentType agentType, ITransitionCreator cost)
    {
        TransitionCreators.Add(agentType.GetType(), cost);    
    }

    public void AddNode(INode<TState> node) => nodes.Add(node);

    public void AddEdge(INode<TState> start, INode<TState> end, ITransitionCreator transitionCreator)
    {
        //implement this
    }
    //public float FindPriority(IVertexState<TValue> curr, IVertexState<TValue> next, List<IVertexState<TValue>> visited);
}
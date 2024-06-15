public interface IFrontier<TState, TValue>
    where TState : IVertexState<TValue>
    where TValue : IComparable<TValue>
{
    public void AddVertexState(TState node, float priority);
    public TState GiveNextVertexState();
    public TState Peek();
    public bool Contains(TState node);
    public int Count { get; }
}
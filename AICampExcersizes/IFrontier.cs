public interface IFrontier<TVertexState, TStateType>
    where TVertexState : IVertexState<TStateType>
{
    void AddVertexState(TVertexState node, float priority);
    TVertexState GiveNextVertexState();
    TVertexState Peek();
    bool Contains(TVertexState node);
    int Count { get; }
}
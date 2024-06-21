namespace AIWorldLibrary;

public interface IFrontier<TAgentState, TPriority>
    where TPriority : IComparable<TPriority>
{
    void AddVertexState(TAgentState state, TPriority priority);
    TAgentState GiveNextVertexState();
    TAgentState Peek();
    bool Contains(TAgentState state);
    bool TyGetFindPriority(TAgentState state, out TPriority priority);
    bool RemoveState(TAgentState state);
    void Clear();
    int Count { get; }
}
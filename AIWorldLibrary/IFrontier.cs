
public interface IFrontier<TAgentState>
    
{
    void AddVertexState(TAgentState node, float priority);
    TAgentState GiveNextVertexState();
    TAgentState Peek();
    bool Contains(TAgentState node);
    int Count { get; }
}
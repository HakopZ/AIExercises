using System.Numerics;


public interface IAgentState<TVertexState, TStateValue, TCost>
    where TVertexState : IVertexState<TStateValue>
    where TCost : INumber<TCost>
{
    TVertexState CurrentState { get; set; }
    IAgentState<TVertexState, TStateValue, TCost> Previous { get; set; }
    //public float Priority { get; set; }
    public TCost Score { get; set; }

}

public interface IAgent { }
public interface IAgent<TVertexState, TStateValue, TCost> : IAgent
    where TVertexState : IVertexState<TStateValue>
        where TCost : INumber<TCost>
{
    IAgentState<TVertexState, TStateValue, TCost> CurrentAgentState { get; set; }
    IFrontier<TVertexState, TStateValue> Frontier { get; set; }
    float FindPriority(IVertexState<TStateValue> curr, IVertexState<TStateValue> next, List<IVertexState<TStateValue>> visited);
    float Heursitic(IVertexState<TStateValue> state, IVertexState<TStateValue> goal);
    // public Agent(IVertexState<TStateValue> currState)
    // {
    //     CurrentState = currState;
    // }

}
/*
public static List<Vertex<Point>> Search(Vertex<Point> start,
                                         Vertex<Point> end,
                                         IFrontier frontier,
                                         Action<IFrontier, Dictionary<Vertex<Point>, (Vertex<Point> parent, int distance)>, List<Vertex<Point>>, VertexState<Point>> addToFrontier)
{
    VertexState<Point> startState = new(start, null, 0);

    Dictionary<Vertex<Point>, (Vertex<Point>, int dis)> founder = [];
    frontier.AddNode(startState, 0); //figure it out
    VertexState<Point> curr = null;
    List<Vertex<Point>> nodes = [];


    while (frontier.Count > 0)
    {

        curr = frontier.GiveMeNode();

        if (curr.Current == end)
        {
            break;
        }
        addToFrontier(frontier, founder, nodes, curr);

    }
    Stack<Vertex<Point>> stack = [];
    while (curr != null)
    {
        stack.Push(curr.Current);
        curr = curr.Previous;
    }
    return stack.ToList();
}

public static void BfsAndDfs(IFrontier frontier, Dictionary<Vertex<Point>, (Vertex<Point>, int distance)> founder, List<Vertex<Point>> visited, VertexState<Point> curr)
{
    if (visited.Contains(curr.Current)) return;
    foreach (var edge in curr.Current.Edges)
    {
        VertexState<Point> state = new(edge.ToVertex, curr, 0);
        frontier.AddNode(state, 0);
        founder[edge.ToVertex] = (curr.Current, 0);
    }
    visited.Add(curr.Current);
}
public static void UCS(IFrontier frontier, Dictionary<Vertex<Point>, (Vertex<Point>, int distance)> founder, List<Vertex<Point>> visited, VertexState<Point> curr)
{
    foreach (var edge in curr.Current.Edges)
    {
        int tentative = edge.Weight + founder[curr.Current].distance;
        if (tentative < founder[edge.ToVertex].distance)
        {
            founder[edge.ToVertex] = (curr.Current, tentative);
            visited.Remove(edge.ToVertex);

        }
        VertexState<Point> state = new VertexState<Point>(edge.ToVertex, curr, tentative);
        if (!frontier.Contains(state) && !visited.Contains(edge.ToVertex))
        {
            frontier.AddNode(state, tentative);
        }
    }
}
public static void AStar()
{

}*?
}*/




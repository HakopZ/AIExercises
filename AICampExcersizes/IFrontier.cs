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




//public
//interface IVertexState<TValue>
//    where TValue : struct, IComparable<TValue>;

//public
//interface IFrontier<TState, TValue>
//    where TState : IVertexState<TValue>
//    where TValue : struct, IComparable<TValue>
//{
//    bool AddNode(TState state);
//}

//class GameState : IVertexState<int>;

//class GameStateFrontier : IFrontier<GameState, int>
//{
//    public bool AddNode(GameState state)
//    {
//        Console.WriteLine("Added!");
//        return true;
//    }
//}

//public class Searcher<TValue>
//    where TValue : struct, IComparable<TValue>
//{
//    public
//    void Search<TState, TFrontier>(TState start, TState end, TFrontier frontier, Func<TState, TState, float> selector)
//        where TState : IVertexState<TValue>
//        where TFrontier : IFrontier<TState, TValue>
//    {

//    }
//}


//public class C
//{

//    static void Main()
//    {

//        GameStateFrontier frontier = new();
//        GameState start = new();
//        GameState end = new();

//        Searcher<int> searcher = new();

//        searcher.Search(start, end, frontier, (a, b) => 0f);

//    }
//}
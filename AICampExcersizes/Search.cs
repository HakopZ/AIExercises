using System.Drawing;


public interface IFrontier<T, U>
    where T : IVertexState<T, U>
{
    public void AddNode(T node, float priority);
    public T GiveMeNode();
    public T Peek();
    public bool Contains(T node);
    public int Count { get; }
}

public interface IVertexState<TSelf, TValueType > where TSelf : IVertexState<TSelf, TValueType> 
{
    public TValueType Value { get; set; }
    public TSelf Previous { get; set; }
    public float TotalDistance { get; set; }
    public int Weight { get; set; }

    public List<TSelf> GetNeighbors();

}
public class MySearch
{
    //figure out a search 

    //bfs 
    public static List<IVertexState<T, U>> NewSearch<T, U>(T start,
                                                T end,
                                                IFrontier<T, U> frontier,
                                                Func<T, T, List<T>, float> findPriority)
        where T : IVertexState<T, U>

    {
        T currentState = start;
        frontier.AddNode(currentState, 0);
        List<T> visited = [];

        while (frontier.Count > 0)
        {
            currentState = frontier.GiveMeNode();

            if (visited.Contains(currentState)) continue;

            if (currentState.Equals(end)) break;


            foreach (var val in currentState.GetNeighbors())
            {
                var nextState = val;
                float priority = findPriority(currentState, nextState, visited);

                if (priority == int.MinValue) continue;

                if (!frontier.Contains(nextState))
                {
                    frontier.AddNode(nextState, priority);
                }
            }
        }

        Stack<T> stack = [];
        while (currentState != null)
        {
            stack.Push(currentState);
            currentState = currentState.Previous;
        }
        return stack.ToList();

    }

    public static int BFSAndDFSPriority<T, U>(T curr, T next, List<T> visited)
        where T : IVertexState<T, U>
    {
        return 0;
    }

    public static float UCSPriority<T, U>(T curr, T next, List<T> visited)
        where T : IVertexState<T, U>
    {
        float tentative = next.Weight + curr.Previous.TotalDistance;
        if (tentative < curr.TotalDistance)
        {
            curr.TotalDistance = tentative;
            visited.Remove(next);

        }
        next.TotalDistance = tentative;

        if (!visited.Contains(next))
        {
            return tentative;
        }
        return int.MinValue;
    }
    //public static float AStarPriority<T>(IVertexState<T> curr, IVertexState<T> next, List<IVertexState<T>> visited)
    //{
    //    var neighbor = next;
    //    float tentative = neighbor.Weight + curr.Previous.TotalDistance;

    //    //If we are revisiting a vertex
    //    //Check if the new tentative distance is smaller than the current tentative distance
    //    //If so update the tentative distance and set visited to false for that neighbor
    //    //By making visited false we are then allowing it to be added to the queue if it is not already in there
    //    float h = float.MinValue;
    //    if (tentative < curr.TotalDistance)
    //    {
    //        visited.Remove(curr);
    //        curr.TotalDistance = tentative;

    //        h = Heuristics(curr.Value, neighbor.Value);
    //    }

    //    if (visited.Contains(next))
    //    {
    //        return float.MinValue;
    //    }
    //    return h;
    //}

    private static float Heuristics<T>(T value1, T value2)
    {
        throw new NotImplementedException();
    }

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
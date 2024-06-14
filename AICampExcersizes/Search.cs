using System.Drawing;


public interface IFrontier
{
    public void AddNode(VertexState<Point> node, float priority);
    public VertexState<Point> GiveMeNode();
    public VertexState<Point> Peek();
    public bool Contains(VertexState<Point> node);
    public int Count { get; }
}

public class VertexState<T>
{
    public Vertex<T> Current { get; set; }
    public VertexState<T> Previous { get; set; }
    public float TotalDistance { get; set; }
    public VertexState(Vertex<T> curr, VertexState<T> prev, float totalDistance = 0)
    {
        Current = curr;
        Previous = prev;
        TotalDistance = totalDistance;
    }
}
public class MySearch
{
    //figure out a search 

    //bfs 
    public static List<Vertex<Point>> NewSearch(Vertex<Point> start,
                                                Vertex<Point> end,
                                                IFrontier frontier,
                                                Func<VertexState<Point>, Edge<Point>, List<Vertex<Point>>, float> findPriority)
    {
        Dictionary<VertexState<Point>, int> info = new();
        VertexState<Point> currentState = new(start, null, 0);
        frontier.AddNode(currentState, 0);
        List<Vertex<Point>> visited = [];

        while (frontier.Count > 0)
        {
            currentState = frontier.GiveMeNode();

            if (visited.Contains(currentState.Current)) continue;

            if (currentState.Current == end) break;

            foreach (var val in currentState.Current.Edges)
            {
                var temp = new VertexState<Point>(val.ToVertex, currentState, val.Weight);
                float priority = findPriority(temp, val, visited);

                if (priority == int.MinValue) continue;

                if (!frontier.Contains(temp))
                {
                    frontier.AddNode(temp, priority);
                }
            }
        }
        Stack<Vertex<Point>> stack = [];
        while (currentState != null)
        {
            stack.Push(currentState.Current);
            currentState = currentState.Previous;
        }
        return stack.ToList();

    }

    public static int BFSAndDFSPriority(VertexState<Point> curr, Edge<Point> edge, List<Vertex<Point>> visited)
    {
        return 0;
    }

    public static float UCSPriority(VertexState<Point> curr, Edge<Point> edge, List<Vertex<Point>> visited)
    {
        float tentative = edge.Weight + curr.Previous.TotalDistance;
        if (tentative < curr.TotalDistance)
        {
            curr.TotalDistance = tentative;
            visited.Remove(edge.ToVertex);

        }
        VertexState<Point> state = new VertexState<Point>(edge.ToVertex, curr, tentative);

        if (!visited.Contains(edge.ToVertex))
        {
            return tentative;
        }
        return int.MinValue;
    }
    public static float AStarPriority(VertexState<Point> curr, Edge<Point> edge, List<Vertex<Point>> visited)
    {
        var neighbor = edge.ToVertex;
        float tentative = edge.Weight + curr.Previous.TotalDistance;

        //If we are revisiting a vertex
        //Check if the new tentative distance is smaller than the current tentative distance
        //If so update the tentative distance and set visited to false for that neighbor
        //By making visited false we are then allowing it to be added to the queue if it is not already in there
        float h = float.MinValue;
        if (tentative < curr.TotalDistance)
        {
            visited.Remove(curr.Current);
            curr.TotalDistance = tentative;
            
            h = Heuristics(curr.Current.Value, neighbor.Value);
        }

        if (visited.Contains(edge.ToVertex))
        {
            return float.MinValue;
        }
        return h;
    }

    private static float Heuristics(Point value1, Point value2)
    {
        throw new NotImplementedException();
    }

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

    }
}
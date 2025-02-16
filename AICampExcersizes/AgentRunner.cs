using System.Drawing;
public class AgentRunner<TStateValue>
    where TStateValue : IEquatable<TStateValue>
{

/*
    public static List<IVertexState<TStateValue>> RunAgent(IAgent agent,
                                                IEnvironment environment)
    {
        IVertexState<TStateValue> currentState = agent.CurrentState;
        currentState.TotalDistance = 0;
        
        agent.Frontier.AddVertexState(currentState, 0);
        List<IVertexState<TStateValue>> visited = [];
        currentState.Distance = 0;

        while (agent.Frontier.Count > 0)
        {
            currentState = agent.Frontier.GiveNextVertexState();

            if (visited.Any(x => x.Value.Equals(currentState.Value))) continue;

            if (currentState.Value.Equals(environment.GoalState.Value)) break;

            visited.Add(currentState);

            var sucessors = environment.GetSuccesors(currentState);
            foreach (var nextState in sucessors)
            {
                float priority = agent.FindPriority(currentState, nextState, visited);

                if (priority == int.MinValue) continue;

                if (!agent.Frontier.Contains(nextState))
                {
                    agent.Frontier.AddVertexState(nextState, priority);
                }
            }
        }

        Stack<IVertexState<TStateValue>> stack = [];
        while (currentState != null)
        {
            stack.Push(currentState);
            currentState = currentState.Previous;
        }
        return stack.ToList();

    }
*/
    public static int BFSAndDFSPriority(IVertexState<TStateValue> curr, IVertexState<TStateValue> next, List<IVertexState<TStateValue>> visited)
    {
        return 0;
    }

    public static float UCSPriority(IVertexState<TStateValue> curr, IVertexState<TStateValue> next, List<IVertexState<TStateValue>> visited)
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
    public static float AStarPriority(IVertexState<TStateValue> curr, IVertexState<TStateValue> next, List<IVertexState<TStateValue>> visited)
    {
        var neighbor = next;
        float tentative = neighbor.Weight + curr.Previous.TotalDistance;

        //If we are revisiting a vertex
        //Check if the new tentative distance is smaller than the current tentative distance
        //If so update the tentative distance and set visited to false for that neighbor
        //By making visited false we are then allowing it to be added to the queue if it is not already in there
        float h = float.MinValue;
        if (tentative < curr.TotalDistance)
        {
            visited.Remove(curr);
            curr.TotalDistance = tentative;

            h = Heuristics(curr.Value, neighbor.Value);
        }

        if (visited.Contains(next))
        {
            return float.MinValue;
        }
        return h;
    }

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




using System.Drawing;
using AIWorldLibrary;

namespace EnvironmentAPI;
public enum SpotState
{
    Empty = 200,
    Obstacle = -200,
    Item = 400,
    Robot = -400
};
public class Vertex(Point location, SpotState state)
{
    public Point Location { get; set; } = location;
    public SpotState State { get; set; } = state;
    public List<Edge> Edges { get; set; } = [];
    public void AddEdge(Vertex to, int cost) => Edges.Add(new(this, to, cost));
}
public class Edge(Vertex from, Vertex to, int cost = 1)
{
    public Vertex From { get; set; } = from;
    public Vertex To { get; set; } = to;
    public int Cost { get; set; } = cost;
}
public class WarehouseEnvironment : AIEnvironment<EnvironmentState>
{
    public override bool isStatic => false;
    public override bool isDeterministic => true;
    public List<Vertex> Vertices { get; set; } = [];
    public List<Edge> Edges { get; set; } = [];

    public void Clear()
    {
        Vertices.Clear();
        Edges.Clear();
    }
    public bool TryAddVertex(Point location, SpotState state)
    {
        if (Search(location) != null)
        {
            return false;
        }
        Vertices.Add(new(location, state));
        return true;
    }
    public bool TryAddUndirectedEdge(Point from, Point end, int cost = 1)
    {
        var v1 = Search(from);
        var v2 = Search(end);
        if (v1 == null || v2 == null)
        {
            return false;
        }
        v1.AddEdge(v2, cost);
        v2.AddEdge(v1, cost);
        return true;
    }

    public bool TryAddDirectedEdge(Point from, Point end, int cost = 1)
    {
        var v1 = Search(from);
        var v2 = Search(end);
        if (v1 == null || v2 == null)
        {
            return false;
        }
        v1.AddEdge(v2, cost);
        return true;
    }

    public Vertex? Search(Point location) => Vertices.Find(x => x.Location == location);
    public override int FindLookAheadAmount(EnvironmentState state)
    {
        throw new NotImplementedException();
    }

    public override List<Succesor<EnvironmentState>> GetSuccesors(EnvironmentState state)
    {
        Vertex robotVertex = Search(state.RobotLocation) ?? throw new NullReferenceException("Robot has to be in the environment");

        List<Succesor<EnvironmentState>> succesors = [];

        foreach (var edge in robotVertex.Edges)
        {
            EnvironmentState newState = new(edge.To.Location, (int)edge.To.State);
            Dictionary<EnvironmentState, double> map = new()
            {
                {newState, 1}
            };
            Succesor<EnvironmentState> succesor = new(state, map);
            succesors.Add(succesor);
        }

        return succesors;
    }


    //no chance for environment right now (will add later)
    public override EnvironmentState MakeMove(EnvironmentState state)
    {
        return state;
    }
}
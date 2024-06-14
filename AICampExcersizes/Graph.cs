public class Graph<T>
{
    public List<Vertex<T>> Vertices = [];
    public List<Edge<T>> Edges = [];

    int Count => Vertices.Count;

    public void AddEdge(T val, T val2, int weight)
    {
        var v = FindVertex(val);
        if(v == null)
        {
            Vertex<T> temp = new Vertex<T>(val);
            Vertices.Add(temp);
            v = temp;
        }

        var v2 = FindVertex(val2);
        if(v2 == null)
        {
            Vertex<T> temp = new Vertex<T>(val2);
            Vertices.Add(temp);
            v2 = temp;
        }
        Edge<T> edge = new Edge<T>(v, v2, weight);
        v.Edges.Add(edge);
    }
    public Vertex<T>? FindVertex(T val) => Vertices.Find(x => x.Value.Equals(val));
}
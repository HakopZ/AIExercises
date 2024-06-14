public class Edge<T>
{
    public Vertex<T> FromVertex { get; set; }
    public Vertex<T> ToVertex { get; set; }
    public int Weight { get; set; }
    public Edge(Vertex<T> from, Vertex<T> to, int weight = 1)
    {
        FromVertex = from;
        ToVertex = to;
        Weight = weight;
    }
}
public class Vertex<T>
{
    public T Value { get; set; }
    public List<Edge<T>> Edges { get; set; } = [];
    public Vertex(T val)
    {
        Value = val;
    }
}
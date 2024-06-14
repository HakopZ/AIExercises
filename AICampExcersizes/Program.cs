//graph belongs to env
//dijkstra - uniform cost search
//bfs, dfs, ucs


using System.Drawing;

class MyQueue : IFrontier
{
    
    public Queue<VertexState<Point>> backed = [];
    public int Count => backed.Count;

    public void AddNode(VertexState<Point> node, float priority)
    {
        backed.Enqueue(node);
    }

    public VertexState<Point> GiveMeNode()
    {
        return backed.Dequeue();
    }

    public VertexState<Point> Peek()
    {
        return backed.Peek();
    }

    public bool Contains(VertexState<Point> node)
    {
        return backed.Contains(node);
    }

}
class MyStack : IFrontier
{
    public Stack<VertexState<Point>> backed = [];

    public int Count => backed.Count;

    public void AddNode(VertexState<Point> node, float priority)
    {
        backed.Push(node);
    }

    public bool Contains(VertexState<Point> node)
    {
        return backed.Contains(node);
    }

    public VertexState<Point> GiveMeNode()
    {
        return backed.Pop();
    }

    public VertexState<Point> Peek()
    {
        return backed.Peek();
    }
}
class Program
{
    
    public static void Main(string[] args)
    {
        Graph<Point> graph = new Graph<Point>();
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {

               
                graph.AddEdge(new Point(x, y), new Point(x, y + 1), 1);
               
                graph.AddEdge(new Point(x, y), new Point(x + 1, y), 1);
               
                graph.AddEdge(new Point(x, y), new Point(x + 1, y + 1), 1);
               
                
                
                
                
            }
        }
        MyQueue myQueue = new MyQueue();
        var path = MySearch.NewSearch(graph.FindVertex(new Point(0, 0)), graph.FindVertex(new Point(3, 3)), myQueue, MySearch.UCSPriority);
        ;
    }
}
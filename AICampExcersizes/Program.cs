//graph belongs to env
//dijkstra - uniform cost search
//bfs, dfs, ucs


using System.Drawing;

// class MyQueue<T> : IFrontier<T>
// {
    
//     public Queue<T> backed = [];
//     public int Count => backed.Count;

//     public void AddNode(T node, float priority)
//     {
//         backed.Enqueue(node);
//     }

//     public T GiveMeNode()
//     {
//         return backed.Dequeue();
//     }

//     public T Peek()
//     {
//         return backed.Peek();
//     }

//     public bool Contains(T node)
//     {
//         return backed.Contains(node);
//     }

// }
// class MyStack<T> : IFrontier<T>
// {
//     public Stack<T> backed = [];

//     public int Count => backed.Count;

//     public void AddNode(T node, float priority)
//     {
//         backed.Push(node);
//     }

//     public bool Contains(T node)
//     {
//         return backed.Contains(node);
//     }

//     public T GiveMeNode()
//     {
//         return backed.Pop();
//     }

//     public T Peek()
//     {
//         return backed.Peek();
//     }
// }
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
        //MyQueue<Point> myQueue = new MyQueue<Point>();
        //var path = MySearch.NewSearch<Point>(graph.FindVertex(new Point(0, 0)), graph.FindVertex(new Point(3, 3)), myQueue, MySearch.UCSPriority);
        ;
    }
}
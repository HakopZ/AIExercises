public interface IVertexState<TValue> 
    where TValue : IComparable<TValue>
    {
    public TValue Value { get; set; }
    public IVertexState<TValue> Previous { get; set; }
    public float TotalDistance { get; set; }
    public float Distance { get; set; }
    public int Weight { get; set; }

    public List<IVertexState<TValue>> GetNeighbors();

}
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
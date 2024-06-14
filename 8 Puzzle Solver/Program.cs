
namespace _8_Puzzle_Solver
{

    public class GameController
    {
        public GameState CurrentState;
        
        public List<GameState> Solve(int board)
        {
            CurrentState = new GameState(board);
            GameState endGameState = new GameState(123456789);
            PriorityQueueWrapper<GameState> priorityQueue = new PriorityQueueWrapper<GameState>();
            
            MySearch.NewSearch<GameState>(CurrentState, endGameState, priorityQueue, Heursitic);
            
        }

        public float Heursitic(IVertexState<int> currentState, IVertexState<int> nextState, List<IVertexState<int>> visited)
        {
            int score = 0;
            int index = 9;
            int board = nextState.Value;
            while(board != 0)
            {
                int val = board % 10;
                board /= 10;
                score += val - index;
                index--;
            }
            return score;
        }
    }

    internal class Program
    {
        public void Solve(int[,] board)
        {

        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}

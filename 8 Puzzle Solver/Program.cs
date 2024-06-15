
using System.Formats.Asn1;

namespace _8_Puzzle_Solver
{

    public class GameController
    {
        public static List<GameState> Solve(int board, int zeroSpot)
        {
            GameState CurrentState = new GameState(board, zeroSpot, null);
            GameState endGameState = new GameState(123456789, 0, null);
            GameStateFrontier priorityQueue = new();

            var states = MySearch<int>.NewSearch(CurrentState, endGameState, priorityQueue, StatePriority);

            return states.Select(x => (GameState)x).ToList();
        }
        public static void PrintPath(List<GameState> path)
        {
            foreach (var state in path)
            {
                PrintBoard(state.Value);
                Console.WriteLine();
            }
        }
        public static void PrintBoard(int board)
        {
            for (int i = 8; i > -1; i--)
            {
                if ((i + 1) % 3 == 0)
                {
                    Console.WriteLine();
                }

                int val = board / (int)Math.Pow(10, i) % 10;
                Console.Write(val);
                Console.Write(' ');
            }
        }
        public static float StatePriority(IVertexState<int> currentState, IVertexState<int> nextState, List<IVertexState<int>> visited)
        {
            var neighbor = nextState;
            float tentative = neighbor.Weight + currentState.Previous.TotalDistance;

            //If we are revisiting a vertex
            //Check if the new tentative distance is smaller than the current tentative distance
            //If so update the tentative distance and set visited to false for that neighbor
            //By making visited false we are then allowing it to be added to the queue if it is not already in there
            float h = float.MinValue;
            if (tentative < currentState.TotalDistance)
            {
                visited.Remove(currentState);
                currentState.TotalDistance = tentative;

                h = Heursitic(neighbor);
            }

            if (visited.Contains(nextState))
            {
                return float.MinValue;
            }
            return h;
        }
        public static float Heursitic(IVertexState<int> state)
        {
            int score = 0;
            int index = 9;
            int board = state.Value;
            while (board != 0)
            {
                int val = board % 10;
                board /= 10;
                score += Math.Abs(val - index);
                index--;
            }
            return score;
        }
    }

    internal class Program
    {
        public static bool CheckSolvable(int board)
        {
            var pList = board.ToString().Select(x => int.Parse(x.ToString())).ToList();
            int inversions = 0;

            for (int i = 0; i < pList.Count; i++)
            {
                for (int j = i + 1; j < pList.Count; j++)
                {
                    if (pList[j] > pList[i])
                    {
                        inversions++;
                    }
                }
            }

            if (inversions % 2 == 1)
            {
                Console.WriteLine("It's Unsolvable");
                return false;
            }
            else
            {
                Console.WriteLine("It's Solvable");
                return true;
            }
        }
        static void Main(string[] args)
        {
            //int board = 692345781;
            int board = 123456798;
            CheckSolvable(board);

            var path = GameController.Solve(board, 1);
            GameController.PrintPath(path);
            Console.WriteLine("Hello, World!");
        }
    }
}

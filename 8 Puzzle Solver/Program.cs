
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
            float tentative = neighbor.Weight + currentState.Distance;

            float h = float.MinValue;
            if (tentative < nextState.Distance)
            {
                visited.Remove(neighbor);
                nextState.Distance = tentative;
                nextState.TotalDistance = Heursitic(neighbor);
                h = nextState.TotalDistance;
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
                //score += val != index ? 1 : 0;
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
            var pList = board.ToString().Select(x => {
                int val = int.Parse(x.ToString());
                val = val == 9 ? 0 : val;
                return val;
                }).ToArray();
            int inversions = 0;

            inversions = getInvCount(pList);

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
        static int getInvCount(int[] arr)
        {
            int inv_count = 0;
            for (int i = 0; i < 9; i++)
                for (int j = i + 1; j < 9; j++)

                    // Value 0 is used for empty space
                    if (arr[i] > 0 && arr[j] > 0 && arr[i] > arr[j])
                        inv_count++;
            return inv_count;
        }

        
       


        static void Main(string[] args)
        {
            int board = 193456782;
            //int board = 123456798;
            if (!CheckSolvable(board)) return;


            var path = GameController.Solve(board, 7);
            Console.WriteLine(path.Count);
            GameController.PrintPath(path);
            Console.WriteLine("Hello, World!");
        }
    }
}

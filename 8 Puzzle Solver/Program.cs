
using System.Formats.Asn1;

namespace _8_Puzzle_Solver
{

    internal class Program
    {
        public static bool CheckSolvable(int board)
        {
            var pList = board.ToString().Select(x =>
            {
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
            int board = 123456798;
            //int board = 123456798;
            if (!CheckSolvable(board)) return;


            var path = GameController.Solve(board, 1);
            Console.WriteLine(path.Count);
            GameController.PrintPath(path);
            Console.WriteLine("Hello, World!");
        }
    }
}

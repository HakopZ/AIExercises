
using AIWorldLibrary;

namespace _8_Puzzle_Solver
{
    public class EightPuzzleState : IState
    {
        public int Board { get; set; }
        public int ZeroSpot { get; set; }
        public EightPuzzleState(int board, int zeroSpot)
        {
            Board = board;
            ZeroSpot = zeroSpot;
        }
    }
}

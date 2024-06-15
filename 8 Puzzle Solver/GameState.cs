
namespace _8_Puzzle_Solver
{
    public class GameState : IVertexState<int>
    {

        public int Value { get; set; }
        public int ZeroSpot { get; set; }
        public float TotalDistance { get; set; } = 0;
        public int Weight { get; set; } = 0;
        public IVertexState<int> Previous { get; set; }

        public GameState(int val, int zeroSpot, IVertexState<int> prev)
        {
            Value = val;
            Previous = prev;
            ZeroSpot = zeroSpot;
        }

        public List<IVertexState<int>> GetNeighbors()
        {
            List<IVertexState<int>> moves = [];

            for (int i = -3; i <= 3; i += 2)
            {
                if (ZeroSpot + i < 0 || ZeroSpot + i > 8) continue;

                int shiftAmount = 9 - (Value / (int)Math.Pow(10, ZeroSpot + i) % 10);
                int val = Value + ((int)Math.Pow(10, ZeroSpot + i) * shiftAmount);
                val -= shiftAmount * (int)Math.Pow(10, ZeroSpot);

                GameState gState = new GameState(val, ZeroSpot + i, this);
                moves.Add(gState);
            }

            return moves;
        }


    }
}

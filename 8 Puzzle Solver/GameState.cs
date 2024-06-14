
namespace _8_Puzzle_Solver
{
    public class GameState<TValueType> : IVertexState<GameState, TValueType>
    {

        public int Value { get; set; }
        public int ZeroSpot { get; set; }
        public IVertexState<GameState<TValueType>, TValueType> Previous { get; set; } = null;
        public float TotalDistance { get; set; } = 0;
        public int Weight { get; set; } = 0;
        
        
        
        public GameState(int val)
        {
            Value = val;
        }

        public List<IVertexState<GameState<TValueType>, TValueType>> GetNeighbors()
        {
            List<IVertexState<GameState<TValueType>, TValueType>> moves = [];

            for (int i = -3; i <= 3; i += 2)
            {
                if (ZeroSpot + i < 0 || ZeroSpot + i > 8) continue;

                int val = Value - (int)Math.Pow(10, ZeroSpot - 1) * (Value / (int)Math.Pow(10, (ZeroSpot - 1)) % 10);
                GameState gState = new GameState(val);
                moves.Add(gState);
            }
            
            return moves;
        }

        List<GameState> IVertexState<GameState>.GetNeighbors()
        {
            throw new NotImplementedException();
        }
    }
}

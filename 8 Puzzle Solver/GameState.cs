
namespace _8_Puzzle_Solver
{
    public class AgentGameState : IAgentState<BoardState, int, int>
    {
        public BoardState CurrentState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IAgentState<BoardState, int, int> Previous { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Score { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public AgentGameState(int current, IAgentState<BoardState, int, int> previous, int score)
        {

            core = score;   
        }
    }
}

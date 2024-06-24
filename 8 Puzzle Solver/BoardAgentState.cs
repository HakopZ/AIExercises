using _8_Puzzle_Solver;
using AIWorldLibrary;

public class BoardAgentState : AgentState<EightPuzzleState, float>
{
    public BoardAgentState(EightPuzzleState envState, float cumulativeCost, AgentState<EightPuzzleState, float> prev) 
    : base(envState, cumulativeCost, prev)
    {
    }
}
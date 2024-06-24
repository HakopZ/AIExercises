using _8_Puzzle_Solver;
using AIWorldLibrary;


public class PuzzleEnvironment : AIEnvironment<EightPuzzleState>
{
    public override bool isStatic => true;

    public override bool isDeterministic => true;

    public override IVertex<EightPuzzleState> GoalState => throw new NotImplementedException();

    public override int FindLookAheadAmount(EightPuzzleState state)
    {
        return -1;
    }

    public override List<Succesor<EightPuzzleState>> GetSuccesors(EightPuzzleState state)
    {
        List<Succesor<EightPuzzleState>> succesors = [];
        for (int i = -3; i <= 3; i += 2)
        {
            if (state.ZeroSpot + i < 0 || state.ZeroSpot + i > 8) continue;
            int shiftAmount = 9 - (state.Board / (int)Math.Pow(10, state.ZeroSpot + i) % 10);
            int val = state.Board + ((int)Math.Pow(10, state.ZeroSpot + i) * shiftAmount);
            val -= shiftAmount * (int)Math.Pow(10, state.ZeroSpot);
            EightPuzzleState gState = new EightPuzzleState(val, state.ZeroSpot + i);
            Succesor<EightPuzzleState> succesor = new Succesor<EightPuzzleState>(state, gState, 1);
            succesors.Add(succesor);
        }
        return succesors;
    }

    public override EightPuzzleState MakeMove(EightPuzzleState state)
    {
        return state;
    }
}
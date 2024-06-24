using AIWorldLibrary;

namespace _8_Puzzle_Solver
{
    public class BoardAgent : PlannerAgent<EightPuzzleState, float>

    {
        // public IVertexState<BoardState> CurrentState { get; set; }
        // public IFrontier<BoardState> Frontier { get; set; }
        // public IAgentState<BoardState, int> CurrentAgentState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        // public BoardAgent(IVertexState<BoardState> currentState, IFrontier<IVertexState<BoardState>, BoardState> front)
        // {
        //     CurrentState = currentState;
        //     Frontier = front;
        // }
        // public float FindPriority(IVertexState<BoardState> curr, IVertexState<BoardState> next, List<IVertexState<BoardState>> visited)
        // {

        //     float tentative = next.Weight + curr.Distance;

        //     float h = float.MinValue;
        //     if (tentative < next.Distance)
        //     {
        //         visited.Remove(next);
        //         next.Distance = tentative;
        //         next.TotalDistance = Heursitic(next, null);
        //         h = next.TotalDistance;
        //     }

        //     if (visited.Contains(next))
        //     {
        //         return float.MinValue;
        //     }
        //     return h;
        // }
        // public float Heursitic(IVertexState<BoardState> state, IVertexState<BoardState> goal)
        // {
        //     int score = 0;
        //     int index = 9;
        //     int board = state.Value.BoardValue;
        //     while (board != 0)
        //     {
        //         int val = board % 10;
        //         board /= 10;
        //         //score += val != index ? 1 : 0;
        //         score += Math.Abs(val - index);
        //         index--;
        //     }
        //     return score;
        // }
        public override void AddSuccessors(List<Succesor<EightPuzzleState>> succesors, EightPuzzleState endState)
        {
            foreach (var succesor in succesors)
            {
                BoardAgentState state = new BoardAgentState(succesor.ToState, CurrentState.CumulativeCost + GetCost(succesor.ToState, succesor.ToState), CurrentState);
                
                Frontier.AddVertexState(state, state.CumulativeCost + GetHeuristic(state.EnvironmentState, endState));
            }
        }

        public override float GetCost(EightPuzzleState state, EightPuzzleState state2)
        {
            int score = 0;
            int index = 9;
            int board = state.Board;
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

        public override float GetHeuristic(EightPuzzleState state, EightPuzzleState goalState)
        {
            throw new NotImplementedException();
        }

        public override EightPuzzleState Move(EightPuzzleState state)
        {
            return state;
        }

        public override Queue<AgentState<EightPuzzleState, int>> Plan(EightPuzzleState state, int lookAmount)
        {

        }
    }
}

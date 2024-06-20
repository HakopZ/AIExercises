
using System.Numerics;
using _8_Puzzle_Solver;

public class PuzzleEnvironment : IEnvironment<BoardState>
{
    public IVertexState<BoardState> GoalState => new GameState(123456789, 0, null);

    public List<INode<BoardState>> nodes { get; set; } = new List<INode<BoardState>>();

    public List<IAgentType> AgentTypes { get; private set; }

    public Dictionary<Type, Type> AgentTypeToTransitionType { get; private set; }

    public IVertexState<BoardState> MakeMove(IVertexState<BoardState> state)
    {
        return state;
    }
    public List<IVertexState<BoardState>> GetSuccesors<TAgentType>(IAgent agent)
    {
        List<IVertexState<BoardState>> moves = [];

        /*
            for (int i = -3; i <= 3; i += 2)
            {
                if (state.Value.ZeroSpot + i < 0 || state.Value.ZeroSpot + i > 8) continue;

                int shiftAmount = 9 - (state.Value.BoardValue / (int)Math.Pow(10, agent.Value.ZeroSpot + i) % 10);
                int val = state.Value.BoardValue + ((int)Math.Pow(10, state.Value.ZeroSpot + i) * shiftAmount);
                val -= shiftAmount * (int)Math.Pow(10, state.Value.ZeroSpot);

                GameState gState = new GameState(val, state.Value.ZeroSpot + i, state);
                moves.Add(gState);
            }*/
        return moves;
    }



    public void BuildOutGrid()
    {
        throw new NotImplementedException();
    }
    public List<IVertexState<BoardState>> GetSuccesors(IAgent agent)
    {
        throw new NotImplementedException();
    }

    public void Register<TCostType>(IAgentType agentType, TCostType cost) where TCostType : INumber<TCostType>
    {
        throw new NotImplementedException();
    }

    public void AddNode(INode<BoardState> node)
    {
        nodes.Add(node);
    }

    public void AddEdge(INode<BoardState> start, INode<BoardState> end, Type agentType, ITransitionCreator transitionCreator)
    {
        var succesor = start.Successors.Where(x => x.ToVertex == end).First();

        if (succesor != null)
        {
            succesor.Edges.Add(agentType, transitionCreator);
        }
        else
        {

        }

    }

    public void AddEdge(INode<BoardState> start, INode<BoardState> end, ITransitionCreator transitionCreator)
    {
        throw new NotImplementedException();
    }
}
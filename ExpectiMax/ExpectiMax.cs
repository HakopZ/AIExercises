using System.Drawing;

public interface IState
{
    bool isTerminal { get; }
    int Value { get; }
    List<IState> Moves { get; }
    public Point Spot { get; set; }
}
public abstract class ChanceyNode : IState
{
    public abstract bool isTerminal { get; }
    public int Value => (int)ChanceMoves.Sum(x => x.prob * x.state.Value);
    public List<(double prob, IState state)> ChanceMoves { get; set; }
    public List<IState> Moves => ChanceMoves.Select(x => x.state).ToList();

    public Point Spot { get; set; }
}
public abstract class MiniMaxNode : IState
{
    public abstract bool isTerminal { get; }
    public int Value { get; set; }
    public List<IState> Moves { get; set; }

    public Point Spot { get; set; }

}
public class ExpectiMax
{
    public int Depth { get; set; }
    public ExpectiMax(int depth = -1)
    {

        Depth = depth;

    }

    public IState OptimalMove(IState currState, bool isMaximizer)
    {
        var ordered = isMaximizer ? currState.Moves.OrderByDescending(x => x.Value) : currState.Moves.OrderBy(x => x.Value);

        var set = ordered.Where(x => x.Value == ordered.First().Value).ToArray();

        return set[Random.Shared.Next(0, set.Length)];
    }
    public int FindScore(IState curr, bool isMaximizer, int depth)
    {
        if (curr.isTerminal)
        {
            return isMaximizer ? depth + curr.Value : -depth + curr.Value;
        }

        int score = isMaximizer ? int.MinValue : int.MaxValue;
        foreach (var state in curr.Moves)
        {
            if (isMaximizer)
            {
                score = Math.Max(FindScore(state, false, depth + 1), score);
            }
            else
            {
                score = Math.Min(FindScore(state, true, depth + 1), score);
            }
        }
        if (curr is MiniMaxNode)
        {
            var mNode = curr as MiniMaxNode;
            mNode.Value = score;
            return score;
        }
        else
        {
            var cNode = curr as ChanceyNode;
            return cNode.Value;
        }
    }
}
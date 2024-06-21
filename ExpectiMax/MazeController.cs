
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

public class MazeState : MiniMaxNode
{
    public override bool isTerminal => Board[Board.GetLength(0) - 1, Board.GetLength(0) - 1] != 0;
    public int[,] Board;
    public Point PrevSpot;
    public Point OppSpot;
    public Point PrevOppSpot;
    public bool Turn;
    public MazeState(int[,] board, Point s, Point prevS, Point oppenetS, Point prevOpponentSpot, bool turn)
    {
        Board = board;
        Spot = s;
        PrevSpot = prevS;
        OppSpot = oppenetS;
        PrevOppSpot = prevOpponentSpot;
        Turn = turn;
    }
    public void GenerateMoves()
    {
        Point p1 = new Point(Spot.X - 1, Spot.Y);
        Point p2 = new Point(Spot.X, Spot.Y - 1);
        Point p3 = new Point(Spot.X + 1, Spot.Y);
        Point p4 = new Point(Spot.X, Spot.Y + 1);
        List<IState> moves = [];
        if (p1 != PrevSpot && p1.X > 0 && Board[p1.X, p1.Y] != 1)
        {
            moves.Add(new ChanceyState(Board, OppSpot, PrevOppSpot, p1, Spot, !Turn));
        }
        if (p2 != PrevSpot && p2.Y > 0 && Board[p2.X, p2.Y] != 1)
        {
            moves.Add(new ChanceyState(Board, OppSpot, PrevSpot, p2, Spot, !Turn));
        }
        if (p3 != PrevSpot && p3.X < Board.GetLength(1) && Board[p3.X, p3.Y] != 1)
        {
            moves.Add(new ChanceyState(Board, OppSpot, PrevSpot, p3, Spot, !Turn));
        }
        if (p4 != PrevSpot && p4.X < Board.GetLength(0) && Board[p4.X, p4.Y] != 1)
        {
            moves.Add(new ChanceyState(Board, OppSpot, PrevSpot, p4, Spot, !Turn));
        }
        Moves = moves;
    }
}
public class ChanceyState : ChanceyNode
{
    public override bool isTerminal => Spot.X == Board.GetLength(1) - 1 && Spot.Y == Board.GetLength(0) - 1;
    public int[,] Board;
    public Point PrevSpot;
    public Point OppSpot;
    public Point PrevOppSpot;
    public bool Turn;
    public ChanceyState(int[,] board, Point s, Point prevS, Point oppenetS, Point prevOpponentSpot, bool turn)
    {
        Board = board;
        Spot = s;
        PrevSpot = prevS;
        OppSpot = oppenetS;
        PrevOppSpot = prevOpponentSpot;
        Turn = turn;
    }
    public void GenerateChanceMoves()
    {
        Point p1 = new Point(Spot.X - 1, Spot.Y);
        Point p2 = new Point(Spot.X, Spot.Y - 1);
        Point p3 = new Point(Spot.X + 1, Spot.Y);
        Point p4 = new Point(Spot.X, Spot.Y + 1);
        List<Point> x = [p1, p2, p3, p4];
        x = x.Where(x => x.X > 0 && x.X < Board.GetLength(1) && x.Y > 0 && x.Y < Board.GetLength(0) && PrevSpot != x && Board[x.X, x.Y] != 1).ToList();

        ChanceMoves.Add((0.2, new MazeState(Board, OppSpot, PrevSpot, x[Random.Shared.Next(0, x.Count)], Spot, !Turn)));

        ChanceMoves.Add((0.8, new MazeState(Board, OppSpot, PrevSpot, x.OrderByDescending(x => Distance(x, new Point(Board.GetLength(1) - 1, Board.GetLength(0) - 1))).First(), Spot, !Turn)));
    }
    private int Distance(Point start, Point goal) => (int)Math.Sqrt(Math.Pow(start.X - goal.X, 2) + Math.Pow(start.Y - goal.Y, 2));
}
public class MazeController<T>

{
    public int[,] Board;
    IState currState;
    public MazeController(int[,] board, Point user1)
    {
        Board = board;
        //MazeState state = new MazeState(board, user1, new Point(-1, -1));
        ExpectiMax expectiMax = new ExpectiMax();

    }
}
public class BoardState : IVertexState<int>, IEquatable<BoardState>
{
    public int Value { get; set; }
    public int ZeroSpot { get; set; }
    public BoardState(int boardValue, int zeroSpot)
    {
        Value = boardValue;
        ZeroSpot = zeroSpot;
    }

    public bool Equals(BoardState other)
    {
        throw new NotImplementedException();
    }
}
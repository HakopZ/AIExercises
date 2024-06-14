namespace Visualizer
{
    public class State
    {
        public int[,] board { get; set; }
        public State(int boardSize = 9)
        {
            int size = (int)Math.Sqrt(boardSize);
            board = new int[size, size];
        }
    }
}

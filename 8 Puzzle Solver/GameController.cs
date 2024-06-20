namespace _8_Puzzle_Solver
{
    public class GameController
    {
        public static List<AgentGameState> Solve(int board, int zeroSpot)
        {
            AgentGameState currentState = new GameState(board, zeroSpot, null);
            PuzzleEnvironment puzzleEnvironment = new();

            GameStateFrontier frontier = new ();
            BoardAgent agent = new(currentState, frontier);

            var states = AgentRunner<BoardState>.RunAgent(agent, puzzleEnvironment);

            return states.Select(x => (AgentGameState)x).ToList();
        }
        public static void PrintPath(List<AgentGameState> path)
        {
            foreach (var state in path)
            {
                PrintBoard(state.Value.BoardValue);
                Console.WriteLine();
            }
        }
        public static void PrintBoard(int board)
        {
            for (int i = 8; i > -1; i--)
            {
                if ((i + 1) % 3 == 0)
                {
                    Console.WriteLine();
                }

                int val = board / (int)Math.Pow(10, i) % 10;
                Console.Write(val);
                Console.Write(' ');
            }
        }    
    }
}

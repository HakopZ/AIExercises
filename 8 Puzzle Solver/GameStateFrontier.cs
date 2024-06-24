
using AIWorldLibrary;

namespace _8_Puzzle_Solver
{
    public class GameStateFrontier : IFrontier<
    {

        PriorityQueue<IVertexState<BoardState>, float> priorityQueue = new();
        public int Count => priorityQueue.Count;

        public void AddVertexState(IVertexState<BoardState> node, float priority)
        {
            priorityQueue.Enqueue(node, priority);
        }

        public IVertexState<BoardState> GiveNextVertexState()
        {
            return priorityQueue.Dequeue();
        }

        public IVertexState<BoardState> Peek()
        {
            return priorityQueue.Peek();    
        }

        public bool Contains(IVertexState<BoardState> node)
        {
            return priorityQueue.UnorderedItems.Any(x => x.Element.Value.Equals(node.Value));
        }

    }
}

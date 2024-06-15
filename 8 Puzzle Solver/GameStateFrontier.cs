
namespace _8_Puzzle_Solver
{
    public class GameStateFrontier : IFrontier<IVertexState<int>, int>
    {

        PriorityQueue<IVertexState<int>, float> priorityQueue = new();
        public int Count => priorityQueue.Count;

        public void AddVertexState(IVertexState<int> node, float priority)
        {
            priorityQueue.Enqueue(node, priority);
        }

        public IVertexState<int> GiveNextVertexState()
        {
            return priorityQueue.Dequeue();
        }

        public IVertexState<int> Peek()
        {
            return priorityQueue.Peek();    
        }

        public bool Contains(IVertexState<int> node)
        {
            return priorityQueue.UnorderedItems.Any(x => x.Element.Value.Equals(node.Value));
        }

    }
}

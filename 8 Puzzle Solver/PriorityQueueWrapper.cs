
namespace _8_Puzzle_Solver
{
    public class PriorityQueueWrapper<T> : IFrontier<T>
    {

        PriorityQueue<T, float> priorityQueue = new();
        public int Count => priorityQueue.Count;

        public void AddNode(T node, float priority)
        {
            priorityQueue.Enqueue(node, priority);
        }

        public T GiveMeNode()
        {
            return priorityQueue.Dequeue();
        }

        public T Peek()
        {
            return priorityQueue.Peek();    
        }

        public bool Contains(T node)
        {
            return priorityQueue.UnorderedItems.Any(x => x.Element.Equals(node));
        }

    }
}

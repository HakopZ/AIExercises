using System.Drawing;
using System.Numerics;
using AIWorldLibrary;
public class AgentRunner<TStateValue, TDistance, TScore>
    where TStateValue : IState
    where TDistance : INumber<TDistance>
    where TScore : INumber<TScore>
{

    public static List<TStateValue> RunAgent(Agent<TStateValue, TDistance, TScore> agent,
                                                AIEnvironment<TStateValue> environment)
    {
        AgentState<TStateValue, TDistance, TScore> currentState = agent.CurrentState;
        currentState.CumulativeDistance = default;

        agent.Frontier.AddVertexState(currentState.EnvironmentState, 0);
        List<TStateValue> visited = [];
        currentState.CumulativeDistance = default;

        while (agent.Frontier.Count > 0)
        {
            var envState = agent.Frontier.GiveNextVertexState();
            
            
            if (visited.Any(x => x.Equals(currentState.EnvironmentState))) continue;

            if (currentState.EnvironmentState.Equals(environment.GoalState.Value)) break;

            visited.Add(currentState.EnvironmentState);

            var sucessors = environment.GetSuccesors(currentState.EnvironmentState, agent.GetType());
            foreach (var nextState in sucessors)
            {
                var getCostMethod = nextState.GetType().GetMethod("GetCost");
                var genericMethod = getCostMethod.MakeGenericMethod(environment.AgentTypesToCostType[agent.GetType()]);
                TDistance cost = (TDistance)genericMethod.Invoke(agent, [agent.GetType()]);
                
                
                float tentative =  + curr.Previous.TotalDistance;
        
                float priority = agent.FindPriority(currentState, nextState, visited);

                if (priority == int.MinValue) continue;

                if (!agent.Frontier.Contains(nextState))
                {
                    agent.Frontier.AddVertexState(nextState, priority);
                }
            }
        }

        Stack<IVertexState<TStateValue>> stack = [];
        while (currentState != null)
        {
            stack.Push(currentState);
            currentState = currentState.Previous;
        }
        return stack.ToList();

    }

    // public static int BFSAndDFSPriority(IVertexState<TStateValue> curr, IVertexState<TStateValue> next, List<IVertexState<TStateValue>> visited)
    // {
    //     return 0;
    // }

    // public static float UCSPriority(IVertexState<TStateValue> curr, IVertexState<TStateValue> next, List<IVertexState<TStateValue>> visited)
    // {
    //     float tentative = next.Weight + curr.Previous.TotalDistance;
    //     if (tentative < curr.TotalDistance)
    //     {
    //         curr.TotalDistance = tentative;
    //         visited.Remove(next);

    //     }
    //     next.TotalDistance = tentative;

    //     if (!visited.Contains(next))
    //     {
    //         return tentative;
    //     }
    //     return int.MinValue;
    // }

    /*
    public static float AStarPriority(IVertexState<TStateValue> curr, IVertexState<TStateValue> next, List<IVertexState<TStateValue>> visited)
    {
        var neighbor = next;
        float tentative = neighbor.Weight + curr.Previous.TotalDistance;

        //If we are revisiting a vertex
        //Check if the new tentative distance is smaller than the current tentative distance
        //If so update the tentative distance and set visited to false for that neighbor
        //By making visited false we are then allowing it to be added to the queue if it is not already in there
        float h = float.MinValue;
        if (tentative < curr.TotalDistance)
        {
            visited.Remove(curr);
            curr.TotalDistance = tentative;

            h = Heuristics(curr.Value, neighbor.Value);
        }

        if (visited.Contains(next))
        {
            return float.MinValue;
        }
        return h;
    }

    private static float Heuristics<T>(T value1, T value2)
    {
        throw new NotImplementedException();
    }*/

}
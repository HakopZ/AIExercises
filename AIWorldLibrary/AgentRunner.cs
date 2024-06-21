using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Numerics;
using AIWorldLibrary;

namespace AIWorldLibrary;
public class AgentRunner<TStateValue, TCost, THeursitic>
    where TStateValue : IState
    where TCost : INumber<TCost>, INullable
{

    public static TStateValue RunAgent(Agent<TStateValue, TCost> agent,
                                                AIEnvironment<TStateValue> environment)
    {
        
        if (agent.CurrentState.EnvironmentState.Equals(environment.GoalState.State)) return agent.CurrentState.EnvironmentState;
        

        
        int lookAhead = Math.Min(agent.LookAhead, environment.FindLookAheadAmount(agent.CurrentState.EnvironmentState));
        
        var nextBestState = agent.SelectState(lookAhead);

        var newEnvironmentState = environment.MakeMove(nextBestState.EnvironmentState);
        
        var actualState = agent.Move(newEnvironmentState);
        
        TCost totalCost = agent.CurrentState.CumulativeCost + agent.GetCost(agent.CurrentState.EnvironmentState, actualState);
        
        var agentState = new AgentState<TStateValue, TCost>(actualState, totalCost, agent.CurrentState);

        var sucessors = environment.GetSuccesors(actualState);
        
        agent.AddSuccessors(sucessors);

        agent.Frontier.Clear();
        agent.CurrentState = agentState;

        return newEnvironmentState;
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
using System.Drawing;
using AIWorldLibrary;

namespace WarehouseAgent;

public class WarehouseAgentState(Point location = default, int score = 0, WarehouseAgentState? prev = null) : AgentState
{
    public WarehouseAgentState? Prev { get; set; } = prev;
    public Point Location { get; set; } = location;
    public int Score { get; set; } = score;
}
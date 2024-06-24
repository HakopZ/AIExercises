using System.Drawing;
using AIWorldLibrary;

namespace EnvironmentAPI;
public class EnvironmentState(Point robotLocation, int score = 0) : IState 
{
    public Point RobotLocation { get; set; } = robotLocation;
    public int Score { get; set; } = score;
    
}
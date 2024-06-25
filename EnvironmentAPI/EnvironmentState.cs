using System.Drawing;
using AIWorldLibrary;

namespace EnvironmentAPI;
public class EnvironmentState(Point robotLocation ) : IState 
{
    public Point RobotLocation { get; set; } = robotLocation;
    public int Score { get; set; }
    
    
}
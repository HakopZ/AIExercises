using System.Drawing;
using AddCSPSolver;

public class ColorVariable : IVariable<Point, Color>
{
    public override HashSet<Color> Domain { get; set; } = new HashSet<Color>() { Color.Red, Color.Green, Color.Blue };
    public ColorVariable(Point rep)
    {
        Representation = rep;
        
    }

}
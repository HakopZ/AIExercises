using System.Drawing;

public class Vertex<T>
{
    public T Value { get; set; }
    public Point Location { get;set; }
    public Rectangle Hitbox
    {
        get
        {
            return new Rectangle(Location, new Size(25, 25));
        }
    }
    public Color Color { get; set; }

    public List<Edge<T>> Edges { get; set; } = [];
    public Vertex(T val)
    {
        Value = val;
    }
}
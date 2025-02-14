using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;

namespace AgentEnvironmentVisualizer;

public class Game1 : Core
{
    public Game1()
        : base()
    {}
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
        Scene = new VisualizerScene();
    }

  
}

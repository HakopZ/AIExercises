using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Threading;
using System.Threading.Tasks;
using AIWorldLibrary;
using Environments;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeuralNetworkLibrary;
using Nez;
using Nez.AI.Pathfinding;
using Nez.UI;
using WarehouseAgent;
namespace AgentEnvironmentVisualizer;

public class VisualizerScene : Scene
{
    public VisualizerScene()
    {

    }
    IntertwinedWarehouseAgentDeepLearning agent;
    WarehouseEnvironment environment;

    public override void Initialize()
    {
        base.Initialize();

        // Add our grid renderer to the scene
        //var component = AddSceneComponent<EnvironmentComponent>();
        var map = CreateEntity("map");
        var env = map.AddComponent<EnvironmentComponent>();
        environment = env.warehouseEnvironment;
        var _agent = map.AddComponent<AgentComponent>();
        agent = _agent.agent;
        AddRenderer(new VisualizerRenderer([_agent], environment));

        ClearColor = Color.CornflowerBlue;

    }

    private float timeAccumulator = 0f;
    private float updateInterval = 0.1f; // 10 updates per second
    public override void Update()
    {

        // if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
        // {

        //     Time.TimeScale = 0.05f;
        // }
        // else if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
        // {
        //     Time.TimeScale = 1f;
        // }
        if (agent.ID != -1 && agent.isReadyToDisplay == false)
        {
            

            agent.BatchTrain(20000, environment);
            agent.StopUpdating = true;
            agent.Epsilon = 0;
            agent.isReadyToDisplay = true;
        }
        else if (agent.ID != -1)
        {
            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.P))
            {
                agent.Epsilon = 0;
            }
            timeAccumulator += Time.DeltaTime;
            if (timeAccumulator < updateInterval)
            {
                return;
            }
            else
            {
                timeAccumulator = 0f;
            }
            if (!agent.MakingMove)
            {
                
                var p = agent.MakeMove(environment);
            }
        }
    }
}
public class VisualizerRenderer : Renderer
{

    private const int GridSize = 32; // Size of each grid cell
    List<AgentComponent> Agents;
    WarehouseEnvironment Env;
    public VisualizerRenderer(List<AgentComponent> agents, WarehouseEnvironment env, int renderOrder = 0)
        : base(renderOrder)
    {
        Agents = agents;
        Env = env;
    }
    public double NormalizeData(double data, double unNormalizedMin, double unNormalizedMax, double normalizedMin, double normalizedMax)
    {
        return ((data - unNormalizedMin) / (unNormalizedMax - unNormalizedMin)) * (normalizedMax - normalizedMin) + normalizedMin;
    }
    public override void Render(Scene scene)
    {
        var spriteBatch = Graphics.Instance.Batcher;
        spriteBatch.Begin();
        for (int i = 0; i < Env.GraphSize.Height + 2; i++)
        {
            for (int j = 0; j < Env.GraphSize.Width + 2; j++)
            {
                DrawGridSpot(spriteBatch, new Vector2(j * 32, i * 32), new Vector2(GridSize, GridSize));
            }
        }


        foreach (var goal in Env.GoalSpots)
        {
            spriteBatch.DrawRect(new Rectangle((goal.X * 32) + 2, (goal.Y * 32) + 2, GridSize - 4, GridSize - 4), Color.Green);
        }

        foreach (var agent in Agents)
        {
            if (agent.agent.isReadyToDisplay)
                spriteBatch.DrawRect(new Rectangle((agent.agent.CurrentState.Location.X * 32) + 2, (agent.agent.CurrentState.Location.Y * 32) + 2, GridSize - 4, GridSize - 4), Color.Yellow);
        }

        //    spriteBatch.DrawRect(new Rectangle((Fire.X * 32) + 2, (Fire.Y * 32) + 2, GridSize - 4, GridSize - 4), Color.Red);

        if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
        {
            List<(int x, int y, double max, int direction)> points = [];
            for (int i = 0; i < Env.GraphSize.Height + 2; i++)
            {
                for (int j = 0; j < Env.GraphSize.Width + 2; j++)
                {
                    var res = Agents[0].agent.liveNet.Compute(MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense([i, j]));
                    var maxVal = res.Max();
                    int action = res.ToList().IndexOf(maxVal) + 1;
                    points.Add((i, j, maxVal, action));
                }
            }
            double min = points.Min(x => x.max);
            double max = points.Max(x => x.max);
            foreach (var point in points)
            {
                float norm = (float)NormalizeData(point.max, min, max, 0, 1);
                Color toDisplay = new Color(0, 0, 0);
                switch (point.direction)
                {
                    case 1:
                        toDisplay = Color.Orange;
                        break;
                    case 2:
                        toDisplay = Color.Blue;
                        break;
                    case 3:
                        toDisplay = Color.Purple;
                        break;
                    case 4:
                        toDisplay = Color.Pink;
                        break;
                }
                spriteBatch.DrawRect(new Rectangle((point.x * 32) + 2, (point.y * 32) + 2, GridSize - 4, GridSize - 4), toDisplay);
            }
        }
        spriteBatch.End();
    }
    private void DrawGridSpot(Batcher spriteBatch, Vector2 location, Vector2 size)
    {
        spriteBatch.DrawRect(location, size.X, size.Y, Color.Black);
        if (location.X == 0 || location.Y == 0 || location.X / 32 == Env.GraphSize.Width + 1 || location.Y / 32 == Env.GraphSize.Height + 1)
            spriteBatch.DrawRect(new Rectangle((int)location.X + 2, (int)location.Y + 2, (int)size.X - 4, (int)size.Y - 4), Color.Red);
        else
            spriteBatch.DrawRect(new Rectangle((int)location.X + 2, (int)location.Y + 2, (int)size.X - 4, (int)size.Y - 4), Color.White);

    }
}
public class EnvironmentComponent : Component
{
    public WarehouseEnvironment warehouseEnvironment = new WarehouseEnvironment([new(3, 3), new(4, 1)]);

}
public class AgentComponent : Component
{
    EnvironmentComponent environmentComponent;
    public IntertwinedWarehouseAgentDeepLearning agent = new IntertwinedWarehouseAgentDeepLearning
    (
        new NeuralNet(2, true).AddLayer(Activations.LeakyReLU, 15).AddLayer(Activations.LeakyReLU, 15).AddLayer(Activations.Identity, 4),
        //envAPIUrl: "http://localhost:5085",
        learningRate: 0.001 / 50
    );
    public override void Initialize()
    {
        environmentComponent = Entity.GetComponent<EnvironmentComponent>();

        agent.Register(environmentComponent.warehouseEnvironment);
        agent.RegisterMovements(environmentComponent.warehouseEnvironment);
        agent.RegisterSensors(environmentComponent.warehouseEnvironment);
        SetEnabled(true);
    }

}

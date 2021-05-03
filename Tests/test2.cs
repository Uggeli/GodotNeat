using Godot;
using System.Collections.Generic;
using Neat.Utils;
using Neat.Phenotypes;
using Agents;

public class test2 : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private int Generation = 0;
    private SpawnManager spawner;
    public Node2D SpawnContainer;
    private Camera2D camera;
    private RichTextLabel label;
    private Position2D Vpos;
    bool best_set = false;
    Phenotype best;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        label = GetNode("Camera2D/label") as RichTextLabel;
        camera = GetNode("Camera2D") as Camera2D;
        Vpos = GetNode("Camera2D/Pos") as Position2D;
        SpawnContainer = GetNode("SpawnContainer") as Node2D;
        spawner = new SpawnManager(50, 21, 5, this, SpawnContainer);
        AddChild(spawner);
        spawner.SpawnEntities();
    }

    public void getBest()
    {
        double bestScore = double.MinValue;
        ShootingEnemy bestAgent = null;
        foreach (ShootingEnemy agent in SpawnContainer.GetChildren())
        {
            if (agent.Score > bestScore)
            {
                bestAgent = agent;
                bestScore = agent.Score;
            }
        }
        camera.Position = bestAgent.GlobalPosition;
        label.Text = $"Generation:{Generation}\nBest Agent Network:";
        best = bestAgent.GetPhenotype();
        best_set = true;
        // VisualiseNetwork(bestAgent.GetPhenotype());
    }

    public void VisualiseNetwork(Phenotype genome)
    {
              
        
    }

    public override void _Process(float delta)
    {
        if (SpawnContainer.GetChildCount() == 0)
        {
            spawner.SpawnEntities();
            Generation++;
        }
        getBest();
        Update();

    }

    public override void _Draw()
    {
        Vector2 offset = new Vector2(0, 0); 
        if (best_set)
        {
            foreach (KeyValuePair<int, List<int>> keyValuePair in best.synapsePos)
            {
                foreach (int item in keyValuePair.Value)
                {
                    DrawLine(Vpos.GlobalPosition + best.neuronPos[keyValuePair.Key] + offset, Vpos.GlobalPosition + best.neuronPos[item] + offset, Color.ColorN("blue"));
                }
            }
                foreach (KeyValuePair<int, Vector2> keyValue in best.neuronPos)
            {
                DrawCircle(Vpos.GlobalPosition + keyValue.Value + offset, 5, Color.ColorN("green"));
            }
            
        }
        
    }
}

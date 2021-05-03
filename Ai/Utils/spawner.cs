using Godot;
using System.Collections.Generic;
using System;
using Neat.Phenotypes;
using Agents;

namespace Neat.Utils
{
    public class SpawnManager : Node2D
    {
        Node Parent;
        Node2D target;
        PackedScene mob;
        PopulationManager popManager;
        Dictionary<Phenotype, double> brains = new Dictionary<Phenotype, double>();
        int counter = 0;

        public SpawnManager(int pop_size, int n_inputs, int n_outputs, Node Parent, Node2D target)
        {
            this.target = target;
            this.Parent = Parent;
            this.popManager = new PopulationManager(pop_size, n_inputs, n_outputs);
            // this.mob = GD.Load("res://Characters/Simple_Ai.tscn") as PackedScene;
            this.mob = GD.Load("res://Characters/ShootingEnemy.tscn") as PackedScene;
        }

        private void GetBrains()
        {
            counter = 0;
            brains.Clear();
            foreach (Phenotype brain in popManager.GetPopulation)
            {
                brains.Add(brain, 0);
            }

        }

        public void Spawn_n_entities(int n)
        {

        }

        public void SpawnEntities()
        {
            GetBrains();
            foreach (KeyValuePair<Phenotype, double> kv in brains)
            {   
                Random rand = new Random();
                // Simple_Ai new_spawn = mob.Instance() as Simple_Ai;
                ShootingEnemy new_spawn = mob.Instance() as ShootingEnemy;
                new_spawn.Death += this.DeathEvent;
                new_spawn.Position = new Vector2(rand.Next(10, 2000),rand.Next(10, 2000));
                new_spawn.RotationDegrees = rand.Next(0, 360);
                // new_spawn.Position = new Vector2(0, 0);
                // new_spawn.AddBrain(kv.Key, this.target);
                new_spawn.AddBrain(kv.Key);
                target.AddChild(new_spawn);

                
            }
        }

        public void DeathEvent(object source, AgentEventArgs args)
        {
            // Brain target_brain;
            foreach(Phenotype brain in brains.Keys )
            {
                if (brain.Id == args.id)
                {
                    // target_brain = brain;
                    brains[brain] = args.score;
                    break;
                }
            }
            counter++;
            if (counter == brains.Count)
            {
                popManager.evaluateGeneration(brains);
                // SpawnEntities();
            }

            // brains[target_brain] = args.score;
        }

        
    }


}
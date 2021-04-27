using System.Collections.Generic;
using System;
using Neat.Phenotypes;

namespace Neat.Utils
{
    public class PopulationManager
    {
        int gen = 0;
        Counter counter = new Counter();
        Genome MasterGenome = new Genome();
        int popSize;

        float lastAvgScore;
        float AvgScore;

        List<Phenotype> Population = new List<Phenotype>();

        public PopulationManager(int popSize, int nInputs, int nOutputs)
        {
            this.popSize = popSize;
            GenesisPhenotype proto = new GenesisPhenotype(this.counter, this.MasterGenome, nInputs, nOutputs);
            Phenotype Adam = new Phenotype(MasterGenome, proto.Synapses, proto.InputNeurons, proto.OutputNeurons, new List<Neuron>(), counter);
            for (int i = 0; i < popSize; i++)
            {
                Phenotype newClone = Adam.Clone();
                // newClone.Mutate();
                Population.Add(newClone);                
            }

        }

        public List<Phenotype> GetPopulation
        {
            get
            {
                return this.Population;
            }
        }

        public void evaluateGeneration(Dictionary<Phenotype, double> pop)
        {
            gen++;
            
            double sum = 0;
            // if (gen % 20 == 0)
            // {
            //     Godot.VisualServer.RenderLoopEnabled = true;
            // }
            // else
            // {
            //     Godot.VisualServer.RenderLoopEnabled = false;
            // }
            foreach(KeyValuePair<Phenotype, double> kv in pop)
            {
                sum += kv.Value;
            }
            List<Phenotype> nextGeneration = new List<Phenotype>();
            Phenotype alpha = findTop(pop);
            nextGeneration.Add(alpha);
            // System.Console.WriteLine($"Gen {gen}:\t Avarage score:{sum / pop.Count:F2}\tBest Agent:{pop[alpha]}");
            Godot.GD.Print($"Gen {gen}:\t Avarage score:{sum / pop.Count:F2}\tBest Agent:{pop[alpha]}");


            pop.Remove(alpha);

            int elitePopSize = Convert.ToInt16(popSize * 0.5);

            List<Phenotype> topPhenotypes = new List<Phenotype>();
            for (int i = 0; i < elitePopSize; i++)
            {
                Phenotype top = findTop(pop);
                pop.Remove(top);
                topPhenotypes.Add(top);
                nextGeneration.Add(top);
            }

            foreach (Phenotype mate in topPhenotypes)
            {
                double distance = alpha.calculateDistance(mate);
                if (distance < 0.5)
                {
                    nextGeneration.Add(alpha.Mate(mate));
                }
                nextGeneration.Add(alpha.Mate(mate));

            }

            // for (int i = 0; i < popSize - elitePopSize; i++)
            // {
            //     Phenotype parent1 = topPhenotypes[new Random().Next(0, topPhenotypes.Count)];
            //     Phenotype parent2 = topPhenotypes[new Random().Next(0, topPhenotypes.Count)];

            //     nextGeneration.Add(parent1.Mate(parent2));                               
            // }

            while (nextGeneration.Count < popSize)
            {
                Phenotype parent1 = topPhenotypes[new Random().Next(0, topPhenotypes.Count)];
                Phenotype parent2 = topPhenotypes[new Random().Next(0, topPhenotypes.Count)];
                if (parent1.calculateDistance(parent2) < 0.1)
                    nextGeneration.Add(parent1.Mate(parent2));
            }
            this.Population = nextGeneration;
        }

        private Phenotype findTop(Dictionary<Phenotype, double> pop)
        {
            Phenotype alpha = null;
            double score = double.MinValue;
            foreach (KeyValuePair<Phenotype, double> kv in pop)
            {
                if (kv.Value > score)
                {
                    score = kv.Value;
                    alpha = kv.Key;
                }
            }
            return alpha;
        }

    }
}
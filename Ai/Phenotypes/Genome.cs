using Neat.Activations;
using Neat.Utils;
using System.Collections.Generic;

namespace Neat.Phenotypes
{
    public class GenesisPhenotype
    {
        List<Neuron> inputNeurons = new List<Neuron>();
        List<Neuron> outputNeurons = new List<Neuron>();
        List<Synapse> synapses = new List<Synapse>();
        public GenesisPhenotype(Counter counter, Genome genome, int inputs, int outputs)
        {
            for (int i = 0; i < inputs; i++)
            {
                Neuron n = new Neuron(new Linear(), counter.GetNeuronInnovation());
                genome.AddNeuron(n);
                inputNeurons.Add(n);
            }

            for (int i = 0; i < outputs; i++)
            {
                Neuron n = new Neuron(new Linear(), counter.GetNeuronInnovation());
                genome.AddNeuron(n);
                outputNeurons.Add(n);
            }

            foreach (Neuron inputN in inputNeurons)
            {
                foreach (Neuron outputN in outputNeurons)
                {
                    Synapse s = new Synapse(counter.GetSynapseInnovation(), inputN.GetId, outputN.GetId, true);
                    genome.AddSynapse(s);
                    synapses.Add(s);
                }
            }

        }
        public List<Neuron> InputNeurons { get => inputNeurons; }
        public List<Neuron> OutputNeurons { get => outputNeurons; }
        public List<Synapse> Synapses { get => synapses; }
    }

    public class Genome
    {
        List<Neuron> neurons = new List<Neuron>();
        List<Synapse> synapses = new List<Synapse>();
        public List<Neuron> Neurons { get => neurons; set => neurons = value; }
        public List<Synapse> Synapses { get => synapses; set => synapses = value; }

        public bool IfSynapseExists(Synapse gene)
        {
            if (Synapses.Contains(gene))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool IfNeuronExists(Neuron gene)
        {
            if (Neurons.Contains(gene))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddSynapse(Synapse gene)
        {
            if (!synapses.Contains(gene))
            {
                synapses.Add(gene);
            }
        }

        public void AddNeuron(Neuron gene)
        {
            if (!neurons.Contains(gene))
            {
                neurons.Add(gene);
            }
        }
    }




}
using Neat.Activations;
using Neat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neat.Phenotypes
{

    public class Phenotype : Genome
    {

        int id;
        Counter counter;
        Genome genome;
        List<Neuron> InputNeurons;
        List<Neuron> OutputNeurons;
        List<Neuron> HiddenNeurons;

        public int Id { get => id; set => id = value; }

        public Phenotype(Genome genome, List<Synapse> synapses, List<Neuron> inputNeurons, List<Neuron> outputNeurons, List<Neuron> hiddenNeurons, Counter counter)
        {

            this.Id = counter.GetPhenotypeInnovation();
            this.genome = genome;
            InputNeurons = inputNeurons;
            OutputNeurons = outputNeurons;
            HiddenNeurons = hiddenNeurons;

            inputNeurons.ForEach(AddNeuron);
            outputNeurons.ForEach(AddNeuron);
            hiddenNeurons.ForEach(AddNeuron);

            synapses.ForEach(AddSynapse);
            MakeConnections();
            this.counter = counter;
        }
        private void MakeConnections()
        {

            foreach (Synapse synapse in Synapses)
            {

                if (synapse.Enabled)
                {

                    synapse.AddConnection(Neurons.Find(x => x.GetId.Equals(synapse.GetFrom)));
                    Neurons.Find(x => x.GetId.Equals(synapse.GetTo)).Connections.Add(synapse);
                }

            }
        }
        public void Mutate()
        {

            Random rand = new Random();
            int choise = rand.Next(0, 6);
            switch (choise)
            {

                case 0:
                    break;

                case 1:
                    MutateWeight();
                    break;

                case 2:
                    MutateBias();
                    break;

                case 3:
                    MutateAddConnection();
                    break;

                case 4:
                    MutateTogle();
                    break;

                case 5:
                    MutateAddNode();
                    break;
            }

        }

        private void MutateWeight()
        {

            Synapses[new Random().Next(0, Synapses.Count)].Weight += new Random().NextDouble();
        }

        private void MutateBias()
        {

            Neurons[new Random().Next(0, Neurons.Count)].Bias += new Random().NextDouble();
        }

        private void MutateAddConnection()
        {

        }

        private void MutateTogle()
        {

            int i = new Random().Next(0, Synapses.Count);
            bool s = Synapses[i].Enabled;
            if (s)
            {

                Synapses[i].Enabled = false;
            }
            else
            {

                Synapses[i].Enabled = true;
            }
        }

        private void MutateAddNode()
        {

            Random r = new Random();

            // get random synapse
            Synapse s = Synapses[new Random().Next(0, Synapses.Count)];


            s.Enabled = false;

            // Neurons.Find(n => n.GetId.Equals(s.GetFrom)).Connections.Remove(s);
            Neurons.Find(n => n.GetId.Equals(s.GetTo)).Connections.Remove(s);

            string[] activations = tools.get_possible_activations();

            Neuron newNeuron = new Neuron(tools.string_to_function(activations[r.Next(0, activations.Length)]), counter.GetNeuronInnovation());
            Synapse inSynapse = new Synapse(counter.GetSynapseInnovation(), s.GetFrom, newNeuron.GetId, true);
            Synapse outSynapse = new Synapse(counter.GetSynapseInnovation(), newNeuron.GetId, s.GetTo, true);

            inSynapse.AddConnection(Neurons.Find(x => x.GetId.Equals(inSynapse.GetFrom)));
            newNeuron.Connections.Add(inSynapse);

            outSynapse.AddConnection(newNeuron);
            Neurons.Find(x => x.GetId.Equals(outSynapse.GetTo)).Connections.Add(outSynapse);

            genome.AddNeuron(newNeuron);
            genome.AddSynapse(inSynapse);
            genome.AddSynapse(outSynapse);

            AddNeuron(newNeuron);
            HiddenNeurons.Add(newNeuron);
            AddSynapse(inSynapse);
            AddSynapse(outSynapse);

        }
        public double calculateDistance(Phenotype other)
        {

            double c1 = 1.0;
            double c2 = 1.0;
            double c3 = 0.4;

            int E = 0;
            int D = 0;
            double W = 0;


            int thisGenomeSize = this.Synapses.Count + this.Neurons.Count;
            int otherGenomeSize = other.Synapses.Count + other.Neurons.Count;

            int n = otherGenomeSize;

            if (thisGenomeSize > otherGenomeSize)
            {

                n = thisGenomeSize;
            }

            if (n < 20)
            {

                n = 1;
            }

            double thisSynapticWeight = 0;
            double otherSynapticWeight = 0;

            List<int> thisSynapses = new List<int>();
            List<int> otherSynapses = new List<int>();
            // this.Synapses.ForEach(s => thisSynapses.Add(s.GetId));
            // this.Synapses.ForEach(s => thisSynapticWeight += s.Weight);

            foreach (Synapse thisSynapse in this.Synapses)
            {
                thisSynapses.Add(thisSynapse.GetId);
                thisSynapticWeight += thisSynapse.Weight;
            }

            // other.Synapses.ForEach(s => otherSynapses.Add(s.GetId));
            // other.Synapses.ForEach(s => otherSynapticWeight += s.Weight);

            foreach (Synapse otherSynapse in other.Synapses)
            {
                otherSynapses.Add(otherSynapse.GetId);
                otherSynapticWeight += otherSynapse.Weight;
            }

            W = (thisSynapticWeight / thisSynapses.Count) - (otherSynapticWeight / otherSynapses.Count);

            thisSynapses.Sort();
            otherSynapses.Sort();

            if (thisSynapses.Last() > otherSynapses.Last())
            {
                E += calculateE(thisSynapses, otherSynapses);
            }
            else
            {
                E += calculateE(otherSynapses, thisSynapses);
            }

            thisSynapses.ForEach(s => otherSynapses.Remove(s));
            D += otherSynapses.Count;

            List<int> thisNeurons = new List<int>();
            List<int> otherNeurons = new List<int>();
            this.Neurons.ForEach(neuron => thisNeurons.Add(neuron.GetId));
            other.Neurons.ForEach(neuron => otherNeurons.Add(neuron.GetId));

            thisNeurons.Sort();
            otherNeurons.Sort();

            if (thisNeurons.Last() > otherNeurons.Last())
            {

                E += calculateE(thisNeurons, otherNeurons);
            }
            else
            {

                E += calculateE(otherNeurons, thisNeurons);
            }
            thisNeurons.ForEach(neuron => otherNeurons.Remove(neuron));
            D += otherNeurons.Count;

            double distance = c1 * E / n + c2 * D / n + c3 * W;
            return distance;
        }

        private int calculateE(List<int> A, List<int> B)
        {

            int E = 0;
            while (A.Last() > B.Last())
            {

                E++;
                A.Remove(A.Last());
            }
            return E;
        }


        public Phenotype Mate(Phenotype other)
        {

            List<Synapse> offspringSynapes = new List<Synapse>();
            List<Neuron> offspringOutput = new List<Neuron>();
            List<Neuron> offspringInput = new List<Neuron>();
            List<Neuron> offspringHidden = new List<Neuron>();

            this.InputNeurons.ForEach(n => offspringInput.Add(n.clone));
            this.OutputNeurons.ForEach(n => offspringOutput.Add(n.clone));

            List<Synapse> commonSynapses = new List<Synapse>(other.Synapses.FindAll(this.Synapses.Contains));
            foreach (Synapse commonSynapse in commonSynapses)
            {

                if (new Random().Next(0, 100) > 50)
                {

                    offspringSynapes.Add(this.Synapses.Find(s => s.GetId.Equals(commonSynapse.GetId)).clone);
                }
                else
                {

                    offspringSynapes.Add(other.Synapses.Find(s => s.GetId.Equals(commonSynapse.GetId)).clone);
                }
            }
            // this.Synapses.ForEach(s => offspringSynapes.Add(s.clone));
            foreach (Synapse s in this.Synapses)
            {

                if (!offspringSynapes.Contains(s))
                {

                    offspringSynapes.Add(s);
                }
            }

            List<Neuron> commonNeurons = new List<Neuron>(other.HiddenNeurons.FindAll(this.HiddenNeurons.Contains));
            foreach (Neuron commonNeuron in commonNeurons)
            {

                if (new Random().Next(0, 100) > 50)
                {

                    offspringHidden.Add(this.HiddenNeurons.Find(n => n.GetId.Equals(commonNeuron.GetId)).clone);
                }
                else
                {

                    offspringHidden.Add(other.HiddenNeurons.Find(n => n.GetId.Equals(commonNeuron.GetId)).clone);
                }
            }
            // this.HiddenNeurons.ForEach(n => offspringHidden.Add(n.clone));
            foreach (Neuron n in this.HiddenNeurons)
            {

                if (!offspringHidden.Contains(n))
                {

                    offspringHidden.Add(n);
                }
            }
            Phenotype offspring = new Phenotype(this.genome, offspringSynapes, offspringInput, offspringOutput, offspringHidden, this.counter);
            offspring.Mutate();
            return offspring;
        }

        public Phenotype Clone()
        {
            List<Synapse> CloneSynapes = new List<Synapse>();
            List<Neuron> CloneOutput = new List<Neuron>();
            List<Neuron> CloneInput = new List<Neuron>();
            List<Neuron> CloneHidden = new List<Neuron>();

            foreach (Synapse synapse in this.Synapses)
            {

                CloneSynapes.Add(synapse.clone);
            }

            foreach (Neuron neuron in this.InputNeurons)
            {

                CloneInput.Add(neuron.clone);
            }

            foreach (Neuron neuron in this.OutputNeurons)
            {

                CloneOutput.Add(neuron.clone);
            }

            foreach (Neuron neuron in this.HiddenNeurons)
            {

                CloneHidden.Add(neuron.clone);
            }
            Phenotype Clone = new Phenotype(this.genome, CloneSynapes, CloneInput, CloneOutput, CloneHidden, this.counter);
            return Clone;
        }

        private void ReviveSynapses()
        {

            this.Synapses.ForEach(x => x.Tired = false);
        }

        private void LoadData(double[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {

                this.InputNeurons[i].LoadData(data[i]);
            }
        }

        public double[] Forward(double[] input)
        {
            ReviveSynapses();
            LoadData(input);

            double[] output = new double[this.OutputNeurons.Count];
            for (int i = 0; i < this.OutputNeurons.Count; i++)
            {

                output[i] = this.OutputNeurons[i].Forward();
            }

            return output;

        }
    }
}
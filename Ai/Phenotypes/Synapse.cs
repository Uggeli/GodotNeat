using System;

namespace Neat.Phenotypes
{
    public class Synapse
    {
        double weight;
        double output = 0;
        bool tired = false;
        Neuron input_neuron;
        private int InnovationNum;
        private int FromNeuron;
        private int ToNeuron;
        bool enabled;

        public Synapse(int innovationNum, int fromNeuron, int toNeuron, bool enabled, double weight = 0)
        {
            InnovationNum = innovationNum;
            FromNeuron = fromNeuron;
            ToNeuron = toNeuron;
            this.Enabled = enabled;
            this.weight = weight;
            if (weight == 0)
                this.weight = new Random().NextDouble();
        }

        public Synapse clone => new Synapse(this.InnovationNum, this.FromNeuron, this.ToNeuron, this.Enabled, this.weight);
 
        public void AddConnection(Neuron input_neuron)
        {
            this.input_neuron = input_neuron;
        }

        public int GetId => this.InnovationNum;
        public int GetTo => this.ToNeuron;
        public int GetFrom => this.FromNeuron;

        public bool Tired { get => tired; set => tired = value; }
        public double Weight { get => weight; set => weight = value; }
        public bool Enabled { get => enabled; set => enabled = value; }

        public double Forward()
        {
            if (Tired)
            {
                return output;
            }
            output = weight * input_neuron.Forward();
            tired = true;
            return output;
        }

        public override bool Equals(object obj)
        {
            return obj is Synapse synapse &&
                   FromNeuron == synapse.FromNeuron &&
                   ToNeuron == synapse.ToNeuron;

        }

        public override int GetHashCode()
        {
            int hashCode = 1535288549;
            hashCode = hashCode * -1521134295 + FromNeuron.GetHashCode();
            hashCode = hashCode * -1521134295 + ToNeuron.GetHashCode();
            return hashCode;
        }
    }
}
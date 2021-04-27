using Neat.Activations;
using System.Collections.Generic;
using System;

namespace Neat.Phenotypes
{
    public class Neuron
    {
        private int InnovationNum;
        bool input_neuron = false;
        Activation activation;
        List<Synapse> connections = new List<Synapse>();
        double bias = new Random().NextDouble();
        double data;

        public Neuron(Activation activation, int innovation_num)
        {
            this.InnovationNum = innovation_num;
            this.activation = activation;
        }
        public int GetId => this.InnovationNum;
        public List<Synapse> Connections { get => connections; set => connections = value; }

        public override bool Equals(object obj)
        {
            return obj is Neuron neuron &&
                   InnovationNum == neuron.InnovationNum;
        }

        public Neuron clone
        {
            get
            {
                Neuron Clone = new Neuron(this.activation, this.InnovationNum);
                Clone.Bias = this.Bias;
                return Clone;
            }
        }

        public double Bias { get => bias; set => bias = value; }

        public double Forward()
        {
            if (input_neuron)
            {
                // Godot.GD.Print($"Neuron id: {this.InnovationNum} output: {data}");
                return data;
            }
            double sum = 0;
            foreach (Synapse synapse in Connections)
            {
                sum += synapse.Forward();
            }
            // Godot.GD.Print($"Neuron id: {this.InnovationNum} output: {sum + bias}");
            return activation.Forward(sum + Bias);
        }

        public override int GetHashCode()
        {
            return -277260550 + InnovationNum.GetHashCode();
        }

        public void LoadData(double data)
        {
            input_neuron = true;
            this.data = data;
        }
    }
}
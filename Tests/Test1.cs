using Godot;
using System.Collections.Generic;
using Neat.Phenotypes;
using Neat.Utils;


public class Test1 : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        double[] data = new double[]{3, 3, 3};

        Counter counter = new Counter();
        Genome master = new Genome();
        GenesisPhenotype gp = new GenesisPhenotype(counter, master, 3, 3);
        List<Neuron> inputNeurons = gp.InputNeurons;
        List<Neuron> outputNeurons = gp.OutputNeurons;
        List<Synapse> synapses = gp.Synapses;

        Phenotype new_pheno = new Phenotype(master, synapses, inputNeurons, outputNeurons, new List<Neuron>(), counter);
        Phenotype clone = new_pheno.Clone();

        for (int i = 0; i < 1; i++)
        {
            new_pheno.Mutate();
            clone.Mutate();
        }
        double[] output = new_pheno.Forward(data);
        double[] output2 = clone.Forward(data);

        PrintOutput(output);
        PrintOutput(output2);


        Phenotype mate = new_pheno.Mate(clone);
        double[] output3 = mate.Forward(data);
        PrintOutput(output3);


        

        
    }


    public void PrintOutput(double[] output)
    {
        string prntString = "";
        foreach (double item in output)
        {
            prntString += " " + item;
        }
        GD.Print(prntString);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

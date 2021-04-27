namespace Neat.Utils
{
	public class Counter
	{
		protected private int phenotype_innovation_number = 0;
		protected private int synapse_innovation_number = 0;
		protected private int neuron_innovation_number = 0;

		public int GetNeuronInnovation()
		{
			this.neuron_innovation_number++;
			return this.neuron_innovation_number;
		}

		public int GetSynapseInnovation()
		{
			this.synapse_innovation_number++;
			return this.synapse_innovation_number;
		}

        public int GetPhenotypeInnovation()
        {
            this.phenotype_innovation_number++;
            return this.phenotype_innovation_number;
        }
	}
}
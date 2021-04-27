namespace Neat.Activations
{
    public class ReLu : Activation
    {
        public override double Forward(double input)
        {
            if (input > 0)
            {
                return input;
            }
            else
            {
                return 0;
            }
            
        }

    }
}
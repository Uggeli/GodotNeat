

namespace Neat.Activations
{

    public abstract class Activation
    {
        
        public virtual double Forward(double input)
        {

            return input;
        }
    }
    public class Linear : Activation
    {

        public override double Forward(double input)
        {

            return input;
        }
    }
}
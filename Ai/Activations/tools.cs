

namespace Neat.Activations
{
    public class tools
    {
        public static Activation string_to_function(string item)
        {
            switch(item.ToLower())
            {
                case "linear":
                return new Linear();

                case "relu":
                return new ReLu();


            }
        return new Linear();
        }

        public static string[] get_possible_activations()
        {
            return new string[]{"linear", "relu"};
        }
    }
}
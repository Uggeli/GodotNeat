using Godot;
using Neat.Phenotypes;
using System;


namespace Agents
{
    public class AgentEventArgs : EventArgs
    {
        public int id { get; set; }
        public double score { get; set; }
    }
    public class Simple_Ai : KinematicBody2D
    {
        public delegate void DeathEventHandler(object source, AgentEventArgs args);
        public event DeathEventHandler Death;
        public int speed = 300;
        public int rotationDir = 0;
        public float rotationSpeed = 1.5f;

        public Vector2 velocity = new Vector2();
        Phenotype brain;
        Label label;

        Node2D target;

        double distance;
        double last_distance;

        double score = 0;

        public override void _Ready()
        {
            base._Ready();
            label = GetNode<Label>("Label");
        }

        public void AddBrain(Phenotype brain, Node2D target)
        {
            this.target = target;
            this.brain = brain;
            distance = sense()[0];
        }

        public double[] sense()
        {
            last_distance = distance;
            distance = this.Position.DistanceTo(target.Position);
            return new double[] { last_distance, distance, target.Position.x, target.Position.y, Position.x, Position.y, score };

        }

        public int get_action(double[] input)
        {
            double[] outputs = brain.Forward(input);
            return arg_max(outputs);

        }

        public int arg_max(double[] input)
        {
            int index = 0;
            double highest_value = double.MinValue;

            for (int i = 0; i < input.Length; i++)
            {
                if (highest_value < input[i])
                {
                    highest_value = input[i];
                    index = i;
                }
            }
            return index;
        }

        // public void act(int action)
        // {
        //     // 4 actions up, down, left, right
        //     velocity = new Vector2();

        //     switch (action)
        //     {
        //         case 0: // right
        //             velocity.x += 1;
        //             break;

        //         case 1: // left
        //             velocity.x -= 1;
        //             break;

        //         case 2: // down
        //             velocity.y += 1;
        //             break;

        //         case 3: // up
        //             velocity.y -= 1;
        //             break;
        //     }

        //     velocity = velocity.Normalized() * speed;
        //     velocity = MoveAndSlide(velocity);

        // }


        public void act(int action)
        {
            // 4 actions up, down, left, right
            velocity = new Vector2();
            rotationDir = 0;

            switch (action)
            {
                case 0: // right
                    rotationDir += 1;
                    break;

                case 1: // left
                    rotationDir -= 1;
                    break;

                case 2: // down
                    velocity = new Vector2(-speed, 0).Rotated(Rotation);
                    break;

                case 3: // up
                    velocity = new Vector2(speed, 0).Rotated(Rotation);
                    break;
            }

            velocity = velocity.Normalized() * speed;
            

        }
        public void _on_Timer_timeout()
        {
            OnDeath();
            QueueFree();
        }

        public void CalculateScore()
        {
            // distance = this.Position.DistanceTo(target.Position);
            // if (distance < 50)
            // {
            //     score++;
            // }
            // else
            // {
            //     score--;
            // }

            if (distance < 200)
            {
                score--;
                if (distance < 50)
                {
                    score--;
                }
            }
            else if (distance > last_distance)
            {
                score++;
            }

            // score = 100 - distance;
        }

        public void updateLabel()
        {
            string labelstr = $"Position{this.Position}\nTarget{target.Position}\nScore{score}\nDistance{distance}";
            label.Text = labelstr;
        }

        protected virtual void OnDeath()
        {
            if (Death != null)
                Death(this, new AgentEventArgs() { id = this.brain.Id, score = this.score });

        }




        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            double[] state = sense();
            int action = get_action(state);
            act(action);
            Rotation += rotationDir * rotationSpeed * delta;
            velocity = MoveAndSlide(velocity);
            CalculateScore();
            updateLabel();
            // GD.Print(this.Position);
        }
    }
}

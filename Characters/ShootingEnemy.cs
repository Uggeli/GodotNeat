using Godot;
using Neat.Phenotypes;
using System.Collections.Generic;


namespace Agents
{
    public class ShootingEnemy : KinematicBody2D
    {
        public delegate void DeathEventHandler(object source, AgentEventArgs args);
        public event DeathEventHandler Death;
        public int speed = 300;
        public int rotationDir = 0;
        public float rotationSpeed = 1.5f;
        public int health = 100;
        public Vector2 velocity = new Vector2();
        public Phenotype brain;
        Label label;
        double distance;
        double last_distance;
        bool shoot = true;
        double score = 0;

        double[] output;
        List<RayCast2D> Senses = new List<RayCast2D>();
        RayCast2D shootRay;

        public double Score { get => score; set => score = value; }

        public override void _Ready()
        {
            shootRay = GetNode<RayCast2D>("shootRay");
            label = GetNode<Label>("Label");
            foreach  (RayCast2D Child in GetNode("Senses").GetChildren())
            {
                Senses.Add(Child);
            }
            base._Ready();

        }

        public void AddBrain(Phenotype brain)
        {
            this.brain = brain;
        }

        public Phenotype GetPhenotype()
        {
            return this.brain;
        }

        public double[] sense()
        {
            List<double> state = new List<double>();
            state.Add(health);
            state.Add(score);
            state.Add(GlobalPosition.x);
            state.Add(GlobalPosition.y);
            state.Add(GlobalRotation);
            foreach (RayCast2D sense in Senses)
            {
                Vector2 distance = new Vector2(0,0);
                if (sense.IsColliding())
                    distance = sense.GetCollisionPoint();
                state.Add(distance.x);
                state.Add(distance.y);
            }
            double[] newState = new double[state.Count];
            newState = state.ToArray();
            return newState;
        }

        public int get_action(double[] input)
        {
            output = brain.Forward(input);
            return arg_max(output);

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
                case 4:
                    Shoot();
                    break;
            }

            velocity = velocity.Normalized() * speed;
            

        }
        public void _on_Timer_timeout()
        {
            health = -9001;
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

            string labelstr = $"Position{this.Position}\nScore{score}\nHealth{health}";
            label.Text = labelstr;


       
        }

        public void Shoot()
        {
            if (shoot)
            {
                // score -= 2;
                health -= 5;
                shoot = false;
                GetNode<Timer>("ShootTimer").Start();
                
                if (shootRay.IsColliding())
                {
                    // Particles2D hitFx = ResourceLoader.Load<Particles2D>("res://Assets/FX/HitSmoke.tscn");
                    // var resource = GD.Load("res://Assets/FX/HitSmoke.tscn") as PackedScene;
                    // Particles2D hitFx = resource.Instance() as Particles2D;
                    // GetParent().AddChild(hitFx);
                    // hitFx.Position = shootRay.GetCollisionPoint();

                    if (shootRay.GetCollider().HasMethod("takeDmg"))
                    {
                        ShootingEnemy target = shootRay.GetCollider() as ShootingEnemy;
                        if (target.isAlive())
                        {
                            target.takeDmg(25);
                            if (target.isAlive())
                            {
                                score += 4;
                                health += 10;
                            }
                            else
                            {
                                score += 10;
                                health += 100;
                            }                    
                        }
                    }
                }
            }
        }

        public void takeDmg(int dmg)
        {
            health -= dmg;
            // score -= dmg;
        }

        public bool isAlive()
        {
            if (health > 0)
            {
                return true;
            }
            return false;
        }

        protected virtual void OnDeath()
        {
            
            if (Death != null)
                Death(this, new AgentEventArgs() { id = this.brain.Id, score = this.score });
            QueueFree();
        }

        
        public void canShoot()
        {
            shoot = true;
        }



        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if (isAlive())
            {
                // _Draw();
                double[] state = sense();
                int action = get_action(state);
                act(action);
                Rotation += rotationDir * rotationSpeed * delta;
                velocity = MoveAndSlide(velocity);
                // CalculateScore();
                updateLabel();
            }
            else
            {
                OnDeath();
            }
            Update();
            // GD.Print(this.Position);
        }

        public override void _Draw()
        {
            if (shootRay.IsColliding() && !shoot)
            {
                // DrawCircle(shootRay.GetCollisionPoint() - Position, 5, Color.ColorN("red"));
                DrawLine(new Vector2(), (shootRay.GetCollisionPoint() - Position).Rotated(-Rotation), Color.ColorN("red"));
            }
            else if(!shoot)
            {
                // DrawCircle(new Vector2(500, 0), 5, Color.ColorN("red"));
                // DrawLine(new Vector2().Rotated(-Rotation), (this.Position + new Vector2(0, 500)), Color.ColorN("red"));
                DrawLine(new Vector2(0, 0), new Vector2(500, 0), Color.ColorN("red"));
            }
 
        
        }
    }
}

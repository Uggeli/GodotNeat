using Godot;
using System;

public class Target : KinematicBody2D
{
    [Export] public int speed = 200;

    public Vector2 velocity = new Vector2();

    private Vector2 target_location;

    public void GetInput()
    {
        velocity = new Vector2();

        if (Input.IsActionPressed("ui_right"))
            velocity.x += 1;

        if (Input.IsActionPressed("ui_left"))
            velocity.x -= 1;

        if (Input.IsActionPressed("ui_down"))
            velocity.y += 1;

        if (Input.IsActionPressed("ui_up"))
            velocity.y -= 1;

        velocity = velocity.Normalized() * speed;
        velocity = MoveAndSlide(velocity);

    }

    private void pickRandomLocation()
    {
        Random r = new Random();
        target_location = new Vector2(r.Next(-1400, 1400), r.Next(-1400, 1400));
    }

    private void moveToTarget()
    {
        velocity = Position.DirectionTo(this.target_location) * speed;
        if (Position.DistanceTo(target_location) > 5)
        {
            velocity = MoveAndSlide(velocity);
        }
        else
        {
            pickRandomLocation();
        }
    }




    public override void _Process(float delta)
    {
        moveToTarget();
        // GetInput();
    }
}
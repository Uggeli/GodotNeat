using Godot;
using System;

public class HitSmoke : Particles2D
{
    public void _on_Timer_timeout()
    {
        QueueFree();
    }
}

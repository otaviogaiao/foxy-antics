using Godot;
using System;

public partial class Snail : EnemyBase
{
    [Export] private RayCast2D _floorDetection;
    
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        Vector2 velocity = Velocity;
        if (!IsOnFloor())
        {
            velocity.Y += _gravity * (float) delta;
        }
        else
        {
            velocity.X = _animatedSprite2D.IsFlippedH() ? _speed : -_speed;
        }
        
        
        Velocity = velocity;
        MoveAndSlide();
        FlipIfCantMove();
    }

    private void FlipIfCantMove()
    {
        if (IsOnFloor() && (!_floorDetection.IsColliding()) || IsOnWall())
        {
            _animatedSprite2D.FlipH = !_animatedSprite2D.FlipH;
            Vector2 floorDetectionPosition = _floorDetection.Position;
            floorDetectionPosition.X = -floorDetectionPosition.X;
            
            _floorDetection.Position = floorDetectionPosition;
        }
    }
}

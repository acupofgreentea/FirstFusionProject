using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : CharacterMovement
{
    protected new Player CharacterBase => base.CharacterBase as Player;

    public event UnityAction<Vector2> OnMovementUpdate; 

    [Networked] public Vector3 MovementInput {get; set;}


    public override void FixedUpdateNetwork() 
    {
        Move();
    }
    
    private void Move()
    {
        if(GetInput(out PlayerNetworkInput playerNetworkInput))
        {
            var Direction = playerNetworkInput.MoveInput.normalized;
            OnMovementUpdate?.Invoke(Direction.ToVector2());

            MovementInput = Direction;

            transform.position += MovementInput * moveSpeed * Runner.DeltaTime;

            if (MovementInput == Vector3.zero) 
                return;
            
            Quaternion lookRotation = Quaternion.LookRotation(MovementInput.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotateSpeed * Runner.DeltaTime);
        }
    }
}

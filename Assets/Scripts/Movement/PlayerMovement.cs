using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : CharacterMovement
{
    protected new Player CharacterBase => base.CharacterBase as Player;

    [Networked] public Vector3 MovementInput {get; set;}
 
    public override void FixedUpdateNetwork() 
    {
        Move();
    }
    
    private void Move()
    {
        if(GetInput(out PlayerNetworkInput playerNetworkInput))
        {
            MovementInput = playerNetworkInput.MoveInput.normalized;

            transform.position += MovementInput * moveSpeed * Runner.DeltaTime;

            if (MovementInput != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(MovementInput.normalized, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotateSpeed * Runner.DeltaTime);
            } 
            
        }
    }
}

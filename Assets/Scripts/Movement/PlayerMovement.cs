using Fusion;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    protected new Player CharacterBase => base.CharacterBase as Player;

    [Networked] public Vector3 MovementInput {get; set;}

    public override void Spawned()
    {
        CharacterBase.PlayerHealth.OnPlayerDie += DisableRpc;
        CharacterBase.PlayerHealth.OnPlayerRespawn += EnableRpc;
        SessionManager.OnSessionStart += EnableRpc;
        SessionManager.OnSessionFinish += DisableRpc;
    }
 
    public override void FixedUpdateNetwork() 
    {
        Move();
    }

    [Rpc]
    private void DisableRpc()
    {
        CanMove = false;
    }

    [Rpc]
    private void EnableRpc()
    {
        CanMove = true;
    }
    
    private void Move()
    {
        if(!CanMove)
            return;
        
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

using UnityEngine;

public class PlayerAnimationController : CharacterAnimationControllerBase
{
    public new Player CharacterBase => base.CharacterBase as Player;
    protected override void CreateDictionary()
    {
        animationDic = new()
        {
            {AnimationKeys.Move, AnimationHashKeys.MoveHashKey},
        };
    }
    public override void Spawned() 
    {
        CharacterBase.CharacterMovement.OnMovementUpdate += HandleOnMovementUpdate;
    }

    private float moveLerpSpeed = 15f;
    private float currentMoveParamValue;
    private void HandleOnMovementUpdate(Vector2 moveInput)
    {
        float sqrMagnitude = moveInput.sqrMagnitude;

        currentMoveParamValue = Mathf.Lerp(currentMoveParamValue, sqrMagnitude, moveLerpSpeed * Runner.DeltaTime);

        currentMoveParamValue = Mathf.Clamp01(currentMoveParamValue);
        
        SetFloat(AnimationKeys.Move, currentMoveParamValue);
    }

    private void OnDestroy() 
    {
        CharacterBase.CharacterMovement.OnMovementUpdate -= HandleOnMovementUpdate;
    }
}
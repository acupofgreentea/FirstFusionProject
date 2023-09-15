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
    public override void FixedUpdateNetwork()
    {
        HandleOnMovementUpdate(CharacterBase.CharacterMovement.MovementInput);
    }

    private float moveLerpSpeed = 15f;
    private float currentMoveParamValue;
    private void HandleOnMovementUpdate(Vector3 moveInput)
    {
        float sqrMagnitude = moveInput.ToVector2().sqrMagnitude;

        currentMoveParamValue = Mathf.Lerp(currentMoveParamValue, sqrMagnitude, moveLerpSpeed * Runner.DeltaTime);

        currentMoveParamValue = Mathf.Clamp01(currentMoveParamValue);
        
        SetFloat(AnimationKeys.Move, currentMoveParamValue);
    }
}
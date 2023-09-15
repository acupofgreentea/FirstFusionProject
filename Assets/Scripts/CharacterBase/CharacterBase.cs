using Fusion;

public class CharacterBase : NetworkBehaviour
{
    public CharacterMovement CharacterMovement {get; private set;}
    public CharacterAnimationControllerBase CharacterAnimationControllerBase {get; private set;}

    protected virtual void Awake()
    {
        CharacterMovement = GetComponent<CharacterMovement>().Init(this);
        CharacterAnimationControllerBase = GetComponent<CharacterAnimationControllerBase>().Init(this);
    }
}

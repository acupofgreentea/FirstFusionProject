public class Player : CharacterBase
{
    public new PlayerMovement CharacterMovement => base.CharacterMovement as PlayerMovement;
    public PlayerInputHandler PlayerInputHandler {get; private set;}
    public PlayerShootController PlayerShootController {get; private set;}
    protected override void Awake() 
    {
        base.Awake();
        CacheComponents();
    }

    private void CacheComponents()
    {
        PlayerInputHandler = GetComponent<PlayerInputHandler>().Init(this);
        PlayerShootController = GetComponent<PlayerShootController>().Init(this);
    }
}

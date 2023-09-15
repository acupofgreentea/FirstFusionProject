using UnityEngine;
using Fusion;
public class Player : CharacterBase
{
    public new PlayerMovement CharacterMovement => base.CharacterMovement as PlayerMovement;
    public PlayerInputHandler PlayerInputHandler {get; private set;}
    public PlayerShootController PlayerShootController {get; private set;}
    public PlayerHealth PlayerHealth {get; private set;}

    [SerializeField] private GameObject rendererRoot;
    protected override void Awake() 
    {
        base.Awake();
        CacheComponents();
    }

    public override void Spawned()
    {
        PlayerHealth.OnPlayerDie += DisableRendererRootRpc;
        PlayerHealth.OnPlayerRespawn += EnableRendererRootRpc;
    }

    [Rpc]
    private void EnableRendererRootRpc()
    {
        rendererRoot.SetActive(true);
        transform.position = new Vector3(Random.Range(-90, 90), 1f, Random.Range(-90, 90));
    }

    [Rpc]
    private void DisableRendererRootRpc()
    {
        rendererRoot.SetActive(false);
    }

    private void CacheComponents()
    {
        PlayerInputHandler = GetComponent<PlayerInputHandler>().Init(this);
        PlayerShootController = GetComponent<PlayerShootController>().Init(this);
        PlayerHealth = GetComponent<PlayerHealth>().Init(this);
    }
}

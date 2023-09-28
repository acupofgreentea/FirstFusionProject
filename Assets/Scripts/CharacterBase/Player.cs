using UnityEngine;
using Fusion;
public class Player : CharacterBase
{
    public new PlayerMovement CharacterMovement => base.CharacterMovement as PlayerMovement;
    //public PlayerInputHandler PlayerInputHandler {get; private set;}
    public PlayerShootController PlayerShootController {get; private set;}
    public PlayerHealth PlayerHealth {get; private set;}
    public PlayerNetworkedData PlayerNetworkedData {get; private set;}

    [SerializeField] private GameObject rendererRoot;
    protected override void Awake() 
    {
        base.Awake();
        CacheComponents();
    }

    private Camera cam;
    public override void Spawned()
    {
        PlayerHealth.OnPlayerDie += DisableRendererRootRpc;
        PlayerHealth.OnPlayerRespawn += EnableRendererRootRpc;
        SessionManager.OnSessionStart += SetRandomPosition;
        SessionManager.OnSessionRestart += SetRandomPosition;

        if(!Object.HasInputAuthority)
            return;

        cam = Camera.main;

        cam.GetComponent<CameraController>().SetFollowObject(transform);

        Debug.LogError("spawned");
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        PlayerHealth.OnPlayerDie -= DisableRendererRootRpc;
        PlayerHealth.OnPlayerRespawn -= EnableRendererRootRpc;
        SessionManager.OnSessionStart -= SetRandomPosition;
        SessionManager.OnSessionRestart -= SetRandomPosition;
    }

    private void SetRandomPosition()
    {
        transform.position = new Vector3(Random.Range(-90, 90), 1f, Random.Range(-90, 90));
    }

    [Rpc]
    private void EnableRendererRootRpc()
    {
        rendererRoot.SetActive(true);
        SetRandomPosition();
    }

    [Rpc]
    private void DisableRendererRootRpc()
    {
        rendererRoot.SetActive(false);
    }

    private void CacheComponents()
    {
        //PlayerInputHandler = GetComponent<PlayerInputHandler>().Init(this);
        PlayerShootController = GetComponent<PlayerShootController>().Init(this);
        PlayerHealth = GetComponent<PlayerHealth>().Init(this);
        PlayerNetworkedData = GetComponent<PlayerNetworkedData>().Init(this);
    }
}

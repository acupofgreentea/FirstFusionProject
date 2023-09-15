using Fusion;
using UnityEngine;

public class PlayerShootController : NetworkBehaviour
{
    [SerializeField] public Bullet bulletPrefab;

    [SerializeField] private Transform spawnPos;
    private Player player;

    private bool canShoot = false;
    public PlayerShootController Init(Player player)
    {
        this.player = player;

        return this;
    }

    private void Awake()
    {
        SessionManager.OnSessionStart += HandleSessionStart;
        SessionManager.OnSessionFinish += HandleSessionFinish;
    }

    private void HandleSessionFinish()
    {
        canShoot = false;
    }

    private void HandleSessionStart()
    {
        canShoot = true;
    }

    public override void FixedUpdateNetwork()
    {
        if(!GetInput(out PlayerNetworkInput playerNetworkInput))
            return;

        if(playerNetworkInput.IsShooting)
        {
            HandleShootPerformed();
        }
    }

    private void HandleShootPerformed()
    {
        Runner.Spawn(bulletPrefab, spawnPos.position, Quaternion.LookRotation(transform.forward));
    }   
    
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        SessionManager.OnSessionStart -= HandleSessionStart;
        SessionManager.OnSessionFinish -= HandleSessionFinish;
    }
}

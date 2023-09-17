using Fusion;
using UnityEngine;

public class PlayerShootController : NetworkBehaviour
{
    [SerializeField] public NetworkPrefabRef bulletPrefab = NetworkPrefabRef.Empty;
    private Player player;

    [Networked] private bool CanShoot {get; set;} = true;
    public PlayerShootController Init(Player player)
    {
        this.player = player;
        cam = Camera.main;
        return this;
    }

    private Camera cam;

    public override void Spawned()
    {
        player.PlayerHealth.OnPlayerDie += DisableShooting;
        player.PlayerHealth.OnPlayerRespawn += EnableShooting;
        SessionManager.OnSessionStart += EnableShooting;
        SessionManager.OnSessionFinish += DisableShooting;
    }

    private void EnableShooting()
    {
        CanShoot = true;
    }

    private void DisableShooting()
    {
        CanShoot = false;
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        SessionManager.OnSessionStart -= EnableShooting;
        SessionManager.OnSessionFinish -= DisableShooting;
        player.PlayerHealth.OnPlayerDie -= DisableShooting;
        player.PlayerHealth.OnPlayerRespawn -= EnableShooting;
    }

    [Networked] public NetworkButtons ButtonsPrevious {get; set;}

    public override void FixedUpdateNetwork()
    {
        if(!CanShoot)
            return;

        if(!GetInput(out PlayerNetworkInput playerNetworkInput))
            return;

        if (playerNetworkInput.buttons.WasPressed(ButtonsPrevious, MyButtons.Attack))
        {
            HandleShootPerformed(playerNetworkInput);
        }

        ButtonsPrevious = playerNetworkInput.buttons;
    }
 
    private void HandleShootPerformed(PlayerNetworkInput playerNetworkInput)
    {
        Vector3 targetPosition = playerNetworkInput.MousePosition;
        Vector3 spawnPosition = transform.position;
        Vector3 direction = targetPosition - spawnPosition;
        direction.y = 0f;
        direction = direction.normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        spawnPosition.y += 2f;

        Runner.Spawn(bulletPrefab, spawnPosition, rotation, Object.InputAuthority);
    }   
}

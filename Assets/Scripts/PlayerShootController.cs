using Fusion;
using UnityEngine;

public class PlayerShootController : NetworkBehaviour
{
    [SerializeField] public GameObject bulletPrefab;
    private Player player;
    public PlayerShootController Init(Player player)
    {
        this.player = player;

        return this;
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
        Runner.Spawn(bulletPrefab, transform.position, Quaternion.identity);
    }   

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        player.PlayerInputHandler.OnShootPerformed -= HandleShootPerformed;
    }
}

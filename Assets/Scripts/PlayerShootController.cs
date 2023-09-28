using Fusion;
using UnityEngine;

public class PlayerShootController : NetworkBehaviour
{
    [SerializeField] public NetworkPrefabRef bulletPrefab = NetworkPrefabRef.Empty;
    private Player player;

    [Networked] private bool CanShoot {get; set;} = true;

    [SerializeField] private Transform bulletSpawnPosition;
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

        networkTransform = GetComponent<NetworkTransform>();
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

    private NetworkTransform networkTransform;

    public override void FixedUpdateNetwork()
    {
        if(!CanShoot)
            return;

        if(!GetInput(out PlayerNetworkInput playerNetworkInput))
            return;
        if (playerNetworkInput.buttons.WasPressed(ButtonsPrevious, MyButtons.Attack) == true)
		{
			HandleShootPerformed(playerNetworkInput);
		}

        ButtonsPrevious = playerNetworkInput.buttons;
    }
 
    private void HandleShootPerformed(PlayerNetworkInput playerNetworkInput)
    {
        var key = new NetworkObjectPredictionKey {Byte0 = (byte) Object.InputAuthority.RawEncoded, Byte1 = (byte) Runner.Simulation.Tick};

        Vector3 targetPosition = playerNetworkInput.MousePosition;
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;
        direction = direction.normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        Runner.Spawn(bulletPrefab, networkTransform.InterpolationTarget.position, rotation, Object.InputAuthority,(runner, obj) =>
			{
				obj.GetComponent<Bullet>().Owner = player;
			}, key );
    }   
}

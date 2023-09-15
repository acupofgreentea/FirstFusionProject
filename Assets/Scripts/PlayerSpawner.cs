using Fusion;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private Transform[] _spawnPoints = null;

    [SerializeField] private NetworkPrefabRef playerNetworkPrefabRef;

    public override void Spawned()
    {
        foreach (var player in Runner.ActivePlayers)
        {
            SpawnPlayer(player);
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
        SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        DespawnPlayer(player);
    }

    private void SpawnPlayer(PlayerRef player)
    {
        int index = player % _spawnPoints.Length;
        var spawnPosition = _spawnPoints[index].transform.position;

        var playerObject = Runner.Spawn(playerNetworkPrefabRef, spawnPosition, Quaternion.identity, player);
        Runner.SetPlayerObject(player, playerObject);
    }

    private void DespawnPlayer(PlayerRef player)
    {
        if (Runner.TryGetPlayerObject(player, out var playerNetworkPrefabRef))
        {
            Runner.Despawn(playerNetworkPrefabRef);
        }

        Runner.SetPlayerObject(player, null);
    }
}

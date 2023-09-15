using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputHandler : NetworkBehaviour, INetworkRunnerCallbacks
{
    private PlayerInput inputActions;

    private Player player;
    
    private bool isShooting = false;

    PlayerNetworkInput playerNetworkInput = new();
    
    public PlayerInputHandler Init(Player player)
    {
        inputActions = new PlayerInput();
        this.player = player;
        return this;
    }

    public override void Spawned()
    {
        Runner.AddCallbacks(this);

        inputActions.PlayerActions.Shoot.performed += HandleShoot;
        player.PlayerHealth.OnPlayerDie += DisableInput;
        player.PlayerHealth.OnPlayerRespawn += EnableInput;

        EnableInput();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        inputActions.PlayerActions.Shoot.performed -= HandleShoot;
        player.PlayerHealth.OnPlayerDie -= DisableInput;
        player.PlayerHealth.OnPlayerRespawn -= EnableInput;
    }

    private void HandleShoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isShooting = true;
    }

    public void EnableInput()
    {
        inputActions.Enable();
    }

    public void DisableInput()
    {
        inputActions.Disable();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        playerNetworkInput.MoveInput = inputActions.PlayerActions.Movement.ReadValue<Vector3>();
        playerNetworkInput.IsShooting = isShooting;
        input.Set(playerNetworkInput);

        isShooting = false;
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
    
}

public struct PlayerNetworkInput : INetworkInput
{
    public Vector3 MoveInput {get; set;}
    
    public bool IsShooting{get; set;}
}


using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class PlayerInputHandler : NetworkBehaviour, INetworkRunnerCallbacks
{
    private PlayerInput inputActions;

    public Vector3 MoveInput => inputActions.PlayerActions.Movement.ReadValue<Vector3>();

    private Player player;
    
    public PlayerInputHandler Init(Player player)
    {
        inputActions = new PlayerInput();
        this.player = player;
        return this;
    }

    public override void Spawned()
    {
        Runner.AddCallbacks( this );
        EnableInput();
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
        PlayerNetworkInput playerNetworkInput = new();
        playerNetworkInput.MoveInput = inputActions.PlayerActions.Movement.ReadValue<Vector3>();
        input.Set(playerNetworkInput);
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
}

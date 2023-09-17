using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInputHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    private Player player;
    
    private bool isShooting = false;

    PlayerNetworkInput playerNetworkInput = new();
    
    public PlayerInputHandler Init(Player player)
    {
        this.player = player;
        return this;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }
    private const string AXIS_HORIZONTAL = "Horizontal";
    private const string AXIS_VERTICAL = "Vertical";
    private const string BUTTON_FIRE1 = "Fire1";

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        PlayerNetworkInput localInput = new PlayerNetworkInput();
        localInput.MoveInput = new Vector3(Input.GetAxisRaw(AXIS_HORIZONTAL), 0f, Input.GetAxisRaw(AXIS_VERTICAL));
        localInput.buttons.Set(MyButtons.Attack, Input.GetButton(BUTTON_FIRE1));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit target))
        {
            localInput.MousePosition = target.point;
        }

        input.Set(localInput);
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

    public Vector3 MousePosition {get; set;}
    
    public bool IsShooting{get; set;}

    public NetworkButtons buttons;
}

public enum MyButtons
{
    Attack = 0
}


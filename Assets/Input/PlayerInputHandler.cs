using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class PlayerInputHandler : SimulationBehaviour, ISpawned, IDespawned, IBeforeUpdate
{
    PlayerNetworkInput playerNetworkInput = new();
	private bool _resetCachedInput;
    
    void ISpawned.Spawned()
		{
			playerNetworkInput = default;

			if (Runner.LocalPlayer == Object.InputAuthority)
			{
				var events = Runner.GetComponent<NetworkEvents>();
                Debug.LogError("subs");
				events.OnInput.RemoveListener(OnInput);
				events.OnInput.AddListener(OnInput);
			}
		}
 
		void IDespawned.Despawned(NetworkRunner runner, bool hasState)
		{
			var events = Runner.GetComponent<NetworkEvents>();
			events.OnInput.RemoveListener(OnInput);
		}

		void IBeforeUpdate.BeforeUpdate()
		{
			if (Object == null || Object.HasInputAuthority == false)
				return;

			if (_resetCachedInput == true)
			{
				_resetCachedInput = false;
				playerNetworkInput = default;
			}

			if (Runner.ProvideInput == false )
				return;

            playerNetworkInput.MoveInput = new Vector3(Input.GetAxisRaw(AXIS_HORIZONTAL), 0f, Input.GetAxisRaw(AXIS_VERTICAL));
            if(Input.GetButtonDown(BUTTON_FIRE1))
            {
                playerNetworkInput.Fire = true;  
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit target))
            {
                playerNetworkInput.MousePosition = target.point;
            }
		}

    private const string AXIS_HORIZONTAL = "Horizontal";
    private const string AXIS_VERTICAL = "Vertical";
    private const string BUTTON_FIRE1 = "Fire1";

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        _resetCachedInput = true;
        input.Set(playerNetworkInput);
    }
}

public struct PlayerNetworkInput : INetworkInput
{
    public Vector3 MoveInput {get; set;}

    public Vector3 MousePosition {get; set;}
    
    public bool IsShooting{get; set;}

    public NetworkButtons buttons;
    public bool Fire { get { return buttons.IsSet(MyButtons.Attack); } set { buttons.Set((int)MyButtons.Attack, value); } }
}

public enum MyButtons
{
    Attack = 0
}


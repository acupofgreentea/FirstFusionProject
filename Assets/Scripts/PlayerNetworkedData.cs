using Fusion;
using UnityEngine;

public class PlayerNetworkedData : NetworkBehaviour
{
    [Networked]
    public NetworkString<_16> NickName { get; private set; }

    [Networked]
    public int KillCount { get; private set; }

    private Player player;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            var nickName = FindObjectOfType<PlayerData>().GetNickName();
            RpcSetNickName(nickName);
        }
    }

    public void AddToKillCount()
    {
        if(!HasStateAuthority)
            return;

        KillCount++;
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void RpcSetNickName(string nickName)
    {
        if (string.IsNullOrEmpty(nickName)) return;
        NickName = nickName;
    }

    public PlayerNetworkedData Init(Player player)
    {
        this.player = player;
        return this;
    }
}

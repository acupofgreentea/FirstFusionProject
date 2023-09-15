using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : NetworkBehaviour, IDamagable
{
    [SerializeField] private int maxHealth = 15;

    [Networked] public int CurrentHealth {get; set;}
    [Networked] public TickTimer RespawnTimer {get; set;}
    [Networked] public bool IsAlive {get; set;}

    public event UnityAction OnPlayerDie;
    public event UnityAction OnPlayerRespawn;

    private Player player;

    public PlayerHealth Init(Player player)
    {
        this.player = player;
        return this;
    }

    public override void Spawned()
    {
        HandleInit();
    }

    public override void FixedUpdateNetwork()
    {
        if(IsAlive)
            return;

        if(RespawnTimer.Expired(Runner))
        {
            Debug.LogError("expired");
            HandleInit();
            OnPlayerRespawn?.Invoke();
        }
    }

    public void TakeDamage(int damage)
    {
        if(!Object.HasStateAuthority)
            return;
        
        CurrentHealth -= damage;

        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void HandleInit()
    {
        IsAlive = true;
        CurrentHealth = maxHealth;
    }

    private void Die()
    {
        IsAlive = false;
        OnPlayerDie?.Invoke();
        RespawnTimer = TickTimer.CreateFromSeconds(Runner, 3f);
    }
}

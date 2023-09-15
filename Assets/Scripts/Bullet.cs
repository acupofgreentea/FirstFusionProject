using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float maxLifetime = 3.0f;
    [Networked] private TickTimer currentLifetime {get; set;}
    [SerializeField] private LayerMask layerMasks; 

    [SerializeField] private int damage = 3;

    [Networked] public Player Owner {get; set;}

    public override void Spawned()
    {
        if(Runner.TryGetPlayerObject(Object.InputAuthority, out var player))
        {
            Owner = player.GetComponent<Player>();
        }
        if (Object.HasStateAuthority == false) 
            return;

        currentLifetime = TickTimer.CreateFromSeconds(Runner, maxLifetime);
    }

    public void SetOwner(Player player)
    {
        Owner = player;
    }

    public override void FixedUpdateNetwork()
    {
        if (HasHitSomething() == false)
        {
            transform.position += transform.forward * moveSpeed * Runner.DeltaTime; 
        }
        else
        {
            Runner.Despawn(Object);
            return;
        }

        CheckLifetime();       
    }   

    private void CheckLifetime()
    {
        if (!currentLifetime.Expired(Runner)) 
            return;

        Runner.Despawn(Object);
    }

    private bool HasHitSomething()
    {
        var hitAnything = Runner.LagCompensation.Raycast(transform.position, transform.forward, moveSpeed * Runner.DeltaTime,
            Object.InputAuthority, out var hit, layerMasks);

        if (hitAnything == false) 
            return false;

        if(hit.GameObject.TryGetComponent(out IDamagable damagable))
        {
            if(CheckIfEnemyWillDie(damagable))
                Owner.PlayerNetworkedData.AddToKillCount();

            damagable.TakeDamage(damage);
        }

        return true;
    }

    private bool CheckIfEnemyWillDie(IDamagable damagable)
    {
        int health = damagable.CurrentHealth;
        return health - damage <= 0;
    }
}

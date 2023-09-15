using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float maxLifetime = 3.0f;
    [Networked] private TickTimer currentLifetime {get; set;}
    [SerializeField] private LayerMask layerMasks; 

    [SerializeField] private int damage = 3;

    public override void Spawned()
    {
        if (Object.HasStateAuthority == false) 
            return;

        currentLifetime = TickTimer.CreateFromSeconds(Runner, maxLifetime);
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
            damagable.TakeDamage(damage);
        }

        return true;
    }
}

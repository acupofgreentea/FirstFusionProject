using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour, IPredictedSpawnBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float maxLifetime = 3.0f;
    [SerializeField] private LayerMask layerMasks; 

    [SerializeField] private int damage = 3;

    public Player Owner {get; set;}

    private Vector3 _interpolateFrom;
	private Vector3 _interpolateTo;
    private NetworkTransform _nt;

    [Networked]
	public TickTimer networkedLifeTimer { get; set; }		
    private TickTimer _predictedLifeTimer;
	private TickTimer lifeTimer
	{
		get => Object.IsPredictedSpawn ? _predictedLifeTimer : networkedLifeTimer;
		set { if (Object.IsPredictedSpawn) _predictedLifeTimer = value;else networkedLifeTimer = value; }
	}

    public override void Spawned()
    {
        if(Runner.TryGetPlayerObject(Object.InputAuthority, out var networkObject))
        {
            Owner = networkObject.GetComponent<Player>();
        }
        if (Object.HasStateAuthority == false) 
            return;

        lifeTimer = TickTimer.CreateFromSeconds(Runner, maxLifetime);
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
        if (!lifeTimer.Expired(Runner)) 
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
            if(hit.GameObject.GetInstanceID() == Owner.gameObject.GetInstanceID())
                return false;

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

    public void PredictedSpawnSpawned()
    {
        _nt = GetComponent<NetworkTransform>();
		_interpolateTo = transform.position;
		_interpolateFrom = _interpolateTo;
	    _nt.InterpolationTarget.position = _interpolateTo;
		Spawned();
    }

    public void PredictedSpawnUpdate()
    {
        _interpolateFrom = _interpolateTo;
		_interpolateTo = transform.position;
		FixedUpdateNetwork();
    }

    public void PredictedSpawnRender()
    {
        var a = Runner.Simulation.StateAlpha;
		_nt.InterpolationTarget.position = Vector3.Lerp(_interpolateFrom, _interpolateTo, a);
    }

    public void PredictedSpawnFailed()
    {
        Debug.LogWarning($"Predicted Spawn Failed Object={Object.Id}, instance={gameObject.GetInstanceID()}, resim={Runner.IsResimulation}");
		Runner.Despawn(Object, true);
    }

    public void PredictedSpawnSuccess()
    {
    }
}

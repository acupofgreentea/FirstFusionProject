using Fusion;
using UnityEngine;

public class CharacterMovement : NetworkBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float rotateSpeed = 6f;
    protected CharacterBase CharacterBase {get; private set;}
    public float MoveSpeed => moveSpeed;

    public CharacterMovement Init(CharacterBase characterBase)
    {
        CharacterBase = characterBase;
    
        return this;
    }
}

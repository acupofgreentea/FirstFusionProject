using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class CharacterAnimationControllerBase : NetworkBehaviour
{
    protected Dictionary<AnimationKeys, int> animationDic;

    private Animator animator;

    public CharacterBase CharacterBase {get; private set;}
    public CharacterAnimationControllerBase Init(CharacterBase characterBase)
    {
        animator = GetComponentInChildren<Animator>();
        CharacterBase = characterBase;
        CreateDictionary();
        return this;
    }

    protected abstract void CreateDictionary();

    

    #region Get-Set Methods
    public void SetFloat(AnimationKeys key, float target)
    {
        if(animationDic.ContainsKey(key))
        {
            animator.SetFloat(animationDic[key], target);
        }
        else
            Debug.LogError($"There is no element for key: {key}");
    }

    public void SetInt(AnimationKeys key, int target)
    {
        if(animationDic.ContainsKey(key))
        {
            animator.SetInteger(animationDic[key], target);
        }
        else
            Debug.LogError($"There is no element for key: {key}");
    }

    public void SetBool(AnimationKeys key, bool target)
    {
        if(animationDic.ContainsKey(key))
        {
            animator.SetBool(animationDic[key], target);
        }
        else
            Debug.LogError($"There is no element for key: {key}");
    }

    public void SetTrigger(AnimationKeys key)
    {
        if(animationDic.ContainsKey(key))
        {
            animator.SetTrigger(animationDic[key]);
        }
        else
            Debug.LogError($"There is no element for key: {key}");
    }

    public bool GetBool(AnimationKeys key)
    {
        if(animationDic.ContainsKey(key))
        {
            return animator.GetBool(animationDic[key]);
        }
        else
            Debug.LogError($"There is no element for key: {key}");

        return false;
    }

    public float GetFloat(AnimationKeys key)
    {
        if(animationDic.ContainsKey(key))
        {
            return animator.GetFloat(animationDic[key]);
        }
        else
            Debug.LogError($"There is no element for key: {key}");

        return 0f;
    }

    public int GetInt(AnimationKeys key)
    {
        if(animationDic.ContainsKey(key))
        {
            return animator.GetInteger(animationDic[key]);
        }
        else
            Debug.LogError($"There is no element for key: {key}");

        return 0;
    }

    #endregion
}

public struct AnimationHashKeys
{
    public static readonly int MoveHashKey = Animator.StringToHash("Move");
}

public enum AnimationKeys
{
    Move,
}

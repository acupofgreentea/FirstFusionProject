using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class SessionManager : NetworkBehaviour
{
    [SerializeField] private float sessionTime = 60f;
    [Networked] public TickTimer SessionTimer {get; set;}
    
    public static event UnityAction OnSessionStart;
    public static event UnityAction OnSessionFinish;

    private bool isSessionActive = false;

    public override void Spawned()
    {
        if(!Object.HasStateAuthority)
            return;

        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return null;
            StartSession();
        }
    }

    private void StartSession()
    {
        SessionTimer = TickTimer.CreateFromSeconds(Runner, sessionTime);
        OnSessionStart?.Invoke();
        isSessionActive = true;
    }

    public override void FixedUpdateNetwork()
    {
        if(SessionTimer.Expired(Runner) && isSessionActive)
        {
            Debug.LogError("Session has ended");
            OnSessionFinish?.Invoke();
            isSessionActive = false;
        }
    }

    void OnGUI()
    {
        if(!Runner)
            return;
        if(!SessionTimer.IsRunning)
            return;
        
        int remainingTime = (int)SessionTimer.RemainingTime(Runner);
        GUI.Label(new Rect(0, 0, 100, 100), remainingTime.ToString());
    }
}

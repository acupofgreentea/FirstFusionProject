using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class SessionManager : NetworkBehaviour
{
    [Networked] public GameState GameState { get; set; }
    [Networked] private NetworkBehaviourId winner { get; set; }
    private List<int> _playerDataNetworkedIds = new List<int>();
    [SerializeField] private float sessionTime = 60f;
    [SerializeField] private float warmupTime = 60f;
    [Networked] public TickTimer SessionTimer {get; set;}
    
    public static event UnityAction OnSessionStart;
    public static event UnityAction OnSessionFinish;
    private bool isSessionActive = false;

    public static SessionManager Instance;

    private List<NetworkObject> players = new();
    public List<NetworkObject> GetPlayers()
    {
        foreach (var player in Runner.ActivePlayers)
        {
            if (Runner.TryGetPlayerObject(player, out var playerObject) == false) 
                continue;

            if(players.Contains(playerObject))
                continue;

            players.Add(playerObject);
        }

        return players;
    }

    public override void Spawned()
    {
        if (GameState != GameState.WarmUp)
        {
            foreach (var player in Runner.ActivePlayers)
            {
                if (Runner.TryGetPlayerObject(player, out var playerObject) == false) 
                    continue;
                TrackNewPlayer(playerObject.GetInstanceID());
            }
        }
        
        Instance = this;

        if (Object.HasStateAuthority == false) 
            return;
            
        GameState = GameState.WarmUp;
        SessionTimer = TickTimer.CreateFromSeconds(Runner, warmupTime);
        TrackNewPlayer(Object.GetInstanceID());
    }
    public void TrackNewPlayer(int playerDataNetworkedId)
    {
        _playerDataNetworkedIds.Add(playerDataNetworkedId);
    }

    private void StartSession()
    {
        Debug.LogError("Session has started");
        SessionTimer = TickTimer.CreateFromSeconds(Runner, sessionTime);
        OnSessionStart?.Invoke();
        isSessionActive = true;
        GameState = GameState.Main;
    }

    private void FinishSession()
    {
        Debug.LogError("Session has ended");
        OnSessionFinish?.Invoke();
        isSessionActive = false;
        GameState = GameState.End;
    }

    public override void FixedUpdateNetwork()
    {
        switch (GameState)
        {
            case GameState.WarmUp:
                if(SessionTimer.Expired(Runner))
                {
                    StartSession();
                }
                break;
            case GameState.Main:
                if(SessionTimer.Expired(Runner) && isSessionActive)
                {
                    FinishSession();
                }
                break;
            case GameState.End:
                break;
        }
    }

    void OnGUI()
    {
        if(!Runner)
            return;
        int remainingTime = 0;
        
        switch (GameState)
        {
            case GameState.WarmUp:
                remainingTime = (int)SessionTimer.RemainingTime(Runner);
                break;
            case GameState.Main:
                remainingTime = (int)SessionTimer.RemainingTime(Runner);
                break;
            case GameState.End:
                break;
        }
        
        GUI.Label(new Rect(0, 0, 100, 100), remainingTime.ToString());
        GUI.Label(new Rect(50, 0, 100, 100), GameState.ToString());
    }
}

public enum GameState { WarmUp, Main, End,}

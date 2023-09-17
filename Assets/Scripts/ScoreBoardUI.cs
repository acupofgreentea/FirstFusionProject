using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class ScoreBoardUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private List<ScoreboardElementUI> scoreboardElements;

    private void Awake()
    {
        panel.SetActive(false);
    }

    void Start()
    {
        SessionManager.OnSessionFinish += HandleSessionFinish;
        SessionManager.OnSessionRestart += HandleSessionRestart;
    }

    private void HandleSessionRestart()
    {
        TogglePanel(false);
    }

    private void HandleSessionFinish()
    {
        TogglePanel(true);
    }

    public void TogglePanel(bool enable)
    {
        panel.SetActive(enable);

        if(enable)
            UpdateElements();
    }
    
    private void UpdateElements()
    {
        var players = SessionManager.Instance.GetPlayers();

        foreach (var item in scoreboardElements)
        {
            item.gameObject.SetActive(false);
        }

        players = players.OrderByDescending(x => x.GetComponent<PlayerNetworkedData>().KillCount).ToList();

        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            var element = scoreboardElements[i];
            element.gameObject.SetActive(true);
            var playerData = player.GetComponent<PlayerNetworkedData>();
            element.SetTexts(playerData.NickName.ToString(), playerData.KillCount.ToString());
        }
    }

    void Update()
    {
        if(SessionManager.Instance == null)
            return;
        
        if(SessionManager.Instance.GameState == GameState.End)
            return;

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePanel(true);
        }

        if(Input.GetKeyUp(KeyCode.Tab))
        {
            TogglePanel(false);
        }
    }

    void OnDestroy()
    {
        SessionManager.OnSessionFinish -= HandleSessionFinish;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardElementUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nickNameText;
    [SerializeField] private TMP_Text killCountText;

    public void SetTexts(string nickName, string killCount)
    {
        nickNameText.text = nickName;
        killCountText.text = killCount;
    } 
}

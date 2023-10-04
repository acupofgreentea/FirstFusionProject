using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private bool isSubscribed = false;

    void Update()
    {
        if(Player.Local == null)
            return;
        
        if(!isSubscribed)
        {
            isSubscribed = true;
            Player.Local.PlayerHealth.OnDamageReceived += HandleDamageReceived;
            healthText.text = $"{Player.Local.PlayerHealth.CurrentHealth}";
        }
    }

    private void HandleDamageReceived(int currentHealth)
    {
        healthText.text = $"{currentHealth}";
    }
}

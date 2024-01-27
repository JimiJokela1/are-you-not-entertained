using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI PlayerHealthNumberText;
    public TextMeshProUGUI PlayerScoreNumberText;
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        PlayerHealthNumberText.text = player.Health.ToString();
        PlayerScoreNumberText.text = player.Score.ToString();
    }
}

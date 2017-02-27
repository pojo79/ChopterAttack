using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour {
    public Slider healthGauge;
    public GameObject player;
    public Text gameMessage;

	// Use this for initialization
	void Start () {
        healthGauge.maxValue = player.GetComponent<HeliControlScript>().MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateHealthGauge();
	}

    private void UpdateHealthGauge()
    {
        float health = player.GetComponent<HeliControlScript>().GetHealth();
        healthGauge.value = healthGauge.maxValue - health;
        if(health <= 0)
        {
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        gameMessage.enabled = true;
        gameMessage.text = "Game Over!";
        gameMessage.fontSize = 60;
    }

}

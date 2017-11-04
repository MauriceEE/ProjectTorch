using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public PlayerCombat player;
    public RectTransform health;
    public RectTransform bar;
    public int maxHP = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        health.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, player.hp * (bar.rect.width/maxHP));
            
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler{
    public void OnPointerClick(PointerEventData eventData) {
        AkSoundEngine.PostEvent("ButtonPress", gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        AkSoundEngine.PostEvent("Pause", gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

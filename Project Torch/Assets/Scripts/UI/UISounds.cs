using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISounds : MonoBehaviour, IPointerEnterHandler {
    public void OnPointerEnter(PointerEventData eventData) {
        AkSoundEngine.PostEvent("Pause", gameObject);
        Debug.Log("Sound");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

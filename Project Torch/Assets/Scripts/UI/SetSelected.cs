using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetSelected : MonoBehaviour {
    public EventSystem es;
    bool selected = false;
    public GameObject splash;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (splash.activeSelf == false && !selected) {
            es.SetSelectedGameObject(gameObject);
            selected = true;
        }
	}
}

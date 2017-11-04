using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditsScroll : MonoBehaviour {
    public float maxTime;
    float scrollTimer;
    public float scrollSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(scrollTimer < maxTime) {
            gameObject.transform.position += new Vector3(0f, scrollSpeed * Time.deltaTime, 0f);
            scrollTimer += Time.deltaTime;
        }else {
            SceneManager.LoadScene(0);
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(0);
        }
	}
}

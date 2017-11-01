using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SplashScreen : MonoBehaviour {
    public float maxTime;
    float timer;
    public GameObject splashScreen;
    public Text text;
    bool fadeText = true;
    bool startFadeOut = true;
    // Use this for initialization
    void Start() {
        timer = maxTime / 3;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
            Destroy(gameObject);
        } else if (fadeText) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                fadeText = false;
            }
            text.color = new Vector4(text.color.r, text.color.g, text.color.b, ((maxTime / 3f)-timer) / (maxTime / 3f));
        } else {
            timer += Time.deltaTime;
            if (timer > maxTime * (4f / 5f)) {
                if (startFadeOut) {
                    splashScreen.GetComponent<Image>().CrossFadeAlpha(0f, maxTime / 5, false);
                    startFadeOut = false;
                }
                text.color = new Vector4(text.color.r, text.color.g, text.color.b, (maxTime - timer)/(maxTime/5));
            }
            if (timer >= maxTime) {
                Destroy(gameObject);
            }
        }
    }
}

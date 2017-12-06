using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour {
    string[] gameOverDialogue = { "As the King of the Dark lay slain, man rejoiced. No longer would they merely make do with the False Flame. From this day on, they would bear a true flame, a privilege long since coveted by the serpents, a desire inherited by man.",
     "Yet more royal blood would be spilt. A King and his Princess, meet their ends on this day. He, battered by war. She, sacrificed in the name of a perceived right.",
     "The Adjudicator was found dead by the side of the murdered King. A burnt out torch of a long dead age, clutched in a bastard hand.",
    };
    public float maxTime;
    float timer;
    public Text text;
    bool fadeText = true;
    bool startFadeOut = true;
    int index = 0;
    // Use this for initialization
    void Start() {
        timer = maxTime;
    }

    // Update is called once per frame
    void Update() {
        if (index < gameOverDialogue.Length) {
            text.text = gameOverDialogue[index];
            if (Input.GetKeyDown(KeyCode.E)) {
                index++;
                timer = maxTime;
            }
            Debug.Log(index);
            timer -= Time.deltaTime;
            if (timer > maxTime * 3 / 4f) {
                text.color = new Vector4(text.color.r, text.color.g, text.color.b, (maxTime - timer) - (maxTime / 4f) / maxTime / 4);
            } else if (timer < maxTime / 4f) {
                text.color = new Vector4(text.color.r, text.color.g, text.color.b, timer / (maxTime / 4));
            }
            if (timer <= 0f) {
                index++;
                timer = maxTime;
            }
        } else {
            index = gameOverDialogue.Length;
            SceneManager.LoadScene("Credits");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;

public class DialogueManager : MonoBehaviour {
    //reference to the dialogue box UI element
    public Text dialogueBox;
   // public GameObject AudioManager;

   /* public GameObject ChoiceContainer;
    public bool choiceState = false;
    public bool choiceBuffer = false;*/

    //List<string> choiceTexts;
    //int selectedChoice = -1;
   

    //A buffered queue that stores the chars to be displayed
    Queue<char> bufferText = new Queue<char>();
    public float PercentageMargin = .02f;

    //The current active text that *should* be displayed on the screen
    //(not accounting for animation buffer)
    public string currentText;

    //Timing & Timer for letter pause, should be adjustable by the user
    public float letterPause = 0.2f;
    float letterTimer;
    bool isClicked = false;
    public float textScrollPause = .07f;
    float textScrollTimer;

    float scrollSpeed = .1f;

    //test dialogue array
    string[] dialogue = { "Dude:\nHello.", "Dude:\nThis is a test", "Dude:\ntesting, testing, 1. 2. 3.", "Dude:\neverything looks good" };
    int dialogueIndex = 0;
    // Use this for initialization
    void Start() {
       
       
        Debug.Log("----------Execute----------");
        //SetText(currentText);
        letterTimer = letterPause * scrollSpeed;
        textScrollTimer = textScrollPause * scrollSpeed;
        dialogueBox.color = new Color(dialogueBox.color.r, dialogueBox.color.g, dialogueBox.color.b, 1f);
        /*RectTransform rectTransform = dialogueBox.GetComponent<RectTransform>();

        float width = GameObject.Find("DialogueBox").GetComponent<RectTransform>().rect.width;
        // float height = GameObject.Find("DialogueBox").GetComponent<RectTransform>().rect.height;
        //rectTransform.sizeDelta = new Vector2(width * .96f, height * .30f);
        rectTransform.offsetMax = new Vector2(width * -PercentageMargin, width * -PercentageMargin);
        rectTransform.offsetMin = new Vector2(width * PercentageMargin, width * PercentageMargin);*/
    }

    // Update is called once per frame
    void Update() {
        //Subtract the passed time from the timer
        letterTimer -= Time.deltaTime;
        Color c = dialogueBox.transform.parent.Find("arrow").GetComponent<Image>().color;
        if(bufferText.Count <= 0) {
            c = new Color(150, 150, 255, 1.0f);
            dialogueBox.transform.parent.Find("arrow").GetComponent<Image>().color = c;
        }else {
            c = new Color(c.r, c.b, c.g, .5f);
            dialogueBox.transform.parent.Find("arrow").GetComponent<Image>().color = c;
        }
        if (bufferText.Count <= 0 && (Input.GetKeyDown(KeyCode.Mouse0)|| Input.touchCount > 0) && isClicked) {
            if(Input.touchCount > 0) {
                for (int i = 0; i < Input.touchCount; i++) {
                    if (Input.GetTouch(i).phase == TouchPhase.Began) {
                        ClearText();
                        SetText(dialogue[dialogueIndex]);
                        dialogueIndex++;
                        letterTimer = letterPause * scrollSpeed;
                        isClicked = false;
                    }
                }
            }else {
                ClearText();
                SetText(dialogue[dialogueIndex]);
                dialogueIndex++;
                letterTimer = letterPause * scrollSpeed;
                isClicked = false;
            }
            if(dialogueIndex > 3) {
                dialogueIndex = 0;
            }

        }
         //Display all text on left click
         else if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0) && isClicked) {//&& dialogueBox.transform.parent.GetComponent<DialogueBox>().isClicked()) {
            StringBuilder stringBuilder = new StringBuilder("", bufferText.Count);
            while (bufferText.Count > 0) {
                stringBuilder.Append(bufferText.Dequeue());
            }
            DisplayText(stringBuilder.ToString());
            isClicked = false;
        }
        //Check if enqueued text is null, if it has any characters, 
        //and if the timer on the character delay is up
        if (bufferText != null && bufferText.Count > 0 && letterTimer <= 0.0f) {
            for (int i = 0; i < (int)(letterTimer * -10000f); i += (int)(letterPause * scrollSpeed * 10000f)) {
                if (bufferText.Count > 0) {
                    DisplayText(bufferText.Dequeue() + "");
                } else {
                    break;
                }
            }
            letterTimer = letterPause * scrollSpeed;
        }

        textScrollTimer -= Time.deltaTime;
        if (bufferText.Count > 0 && textScrollTimer <= 0 && bufferText.Peek() != ' ') {
            //play sound for text scrolling
            //AudioManager.GetComponent<AudioManager>().PlayTextScroll();
            textScrollTimer = textScrollPause * scrollSpeed;
        }

        dialogueBox.text.Replace("<br>", "\n");
    }
    public void OnClick() {
        isClicked = true;
        //SetText(dialogue[dialogueIndex]);
        //MORE TEST CODE
    }
    public void ClearText() {
        dialogueBox.text = "";
        bufferText.Clear();
    }
    //displays text to the screen
    void DisplayText(string s) {
        dialogueBox.text += s;
    }
    //Turn string s into a queue of chars and add to the bufferText queue
    public void SetText(string s) {
        char[] temp = s.ToCharArray();
        string name = "";
        int index = 0;
        for (int i = 0; temp[i] != '\n'; i++) {
            name += temp[i];
            index = i;
        }
        string text = "";
        for (int i = index + 1; i < temp.Length; i++) {
            text += temp[i];
        }
        bufferText = new Queue<char>(text.ToCharArray());
        DisplayText(name);
    }
    //Add text to the end of the current active text
    void AppendText(string s) {
        char[] newChars = bufferText.ToArray().Concat(s.ToCharArray()).ToArray();
        bufferText = new Queue<char>(newChars);
    }
    //dialogue choice making
    /*public void ChoiceInit(string[] texts) {
        ResetChoice();
        choiceState = true;
        for(int i = 0; i < texts.Length; i++) {
            GameObject temp = ChoiceContainer.transform.GetChild(i).gameObject;
            temp.SetActive(true);
            temp.transform.FindChild("Text").GetComponent<Text>().text = texts[i];
        }
    }
    public void ResetChoice() {
        choiceState = false;
        choiceBuffer = false;
        selectedChoice = -1;
        for (int i = 0; i < 4; i++) {
            GameObject temp = ChoiceContainer.transform.GetChild(i).gameObject;
            if(temp != null) {
                temp.transform.FindChild("Text").GetComponent<Text>().text = "";
                temp.SetActive(false);
            }
        }
    }
    public int GetSelectedChoice() {
        return selectedChoice;
    }
    public void SetSelectedChoice(int choice) {
        if (choiceState) {
            selectedChoice = choice;
            choiceState = false;
            choiceBuffer = true;
        }
    }*/
}

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

    //Getting all the lines of dialogue in the game
    public TextManager tManager;

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
    bool finishedDialogueSequence;
    //test dialogue array
    Queue<string> dialogue;

    //NOTE: I put this here to manage the container being active/inactive
    //  -Connor @ 11/1 18:29
    public GameObject dialogueContainer;
    //  Also put this here to help with flags and triggers and stuff
    string currentSequenceName;
    FlagManager flagMan;

    //USE THIS METHOD TO ADD A DIALOGUE SEQUENCE TO THE TEXT BOX. 
    public void AddDialogueSequence(string[] dialogue, string sequenceName) {
        if (!dialogueContainer.activeInHierarchy)
            dialogueContainer.SetActive(true);
        this.dialogue = new Queue<string>(dialogue);
        this.currentSequenceName = sequenceName;
    }
    
    // Use this for initialization
    void Start() {
        //string[] temp = { "Joey:\n Yo", "Dude:\n Ayy lmao", "Joey:\n SHIT IT'S DA FUZZ" };
        //AddDialogueSequence(temp);
        finishedDialogueSequence = false;
        //Debug.Log("----------Execute----------");
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

        //tManager = new TextManager();
        flagMan = GameObject.Find("FlagManagerGO").GetComponent<FlagManager>();
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
        if (bufferText.Count <= 0 && (Input.GetKeyDown(KeyCode.Mouse0)|| Input.touchCount > 0 || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1))) {
            ClearText();
            if (dialogue != null && dialogue.Count > 0) {
                SetText(dialogue.Dequeue());
            }
            letterTimer = letterPause * scrollSpeed;
            isClicked = false;
            
        }
         //Display all text on left click
         else if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0 || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1))) {//&& dialogueBox.transform.parent.GetComponent<DialogueBox>().isClicked()) {
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
        if (bufferText.Count > 0 && textScrollTimer <= 0 && bufferText.Peek() != ' ')
        {
            //play sound for text scrolling
            //AudioManager.GetComponent<AudioManager>().PlayTextScroll();
            textScrollTimer = textScrollPause * scrollSpeed;
        }
        if (finishedDialogueSequence && dialogue.Count == 0 && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1)))
        {
            finishedDialogueSequence = false;
            dialogueContainer.SetActive(false);
            flagMan.DialogueEnded(currentSequenceName);//Tell the flagmanager
        }
        if(dialogue != null && dialogue.Count == 0)
        {
            finishedDialogueSequence = true;
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
    public void DisplayText(string s) {
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

    public void SetTextAndShowImmediately()
    {
        SetText(dialogue.Dequeue());
    }

    //Add text to the end of the current active text
    void AppendText(string s) {
        char[] newChars = bufferText.ToArray().Concat(s.ToCharArray()).ToArray();
        bufferText = new Queue<char>(newChars);
    }
}

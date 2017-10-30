using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour {
    GameObject menuCanvas;
    public List<Button> buttons;
    // Use this for initialization
    void Start () {
        menuCanvas = gameObject.transform.GetChild(0).gameObject;
        buttons = new List<Button>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
        if (SceneManager.sceneCount > 1 && SceneManager.GetSceneAt(1).isLoaded) {
            SceneManager.UnloadSceneAsync("Loading");
        }
       
       /* foreach(Button b in buttons) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                AkSoundEngine.PostEvent("Scroll",gameObject);
                Debug.Log("Sound");
            }
        }*/
    }
    public void Resume() {
        menuCanvas.SetActive(true);
    }
    public void ExitGame() {
        Application.Quit();
        //temp code for editor
        UnityEditor.EditorApplication.isPlaying = false;
    }
    public void LoadMainMenu() {
        SceneManager.LoadScene("Main Menu");

    }
    public void LoadGame() {
        SceneManager.UnloadSceneAsync("Main Menu");
        SceneManager.LoadSceneAsync("Loading");
        SceneManager.LoadSceneAsync("Game");
        
    }
}

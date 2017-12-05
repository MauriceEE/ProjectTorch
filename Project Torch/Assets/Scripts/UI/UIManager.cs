using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour {
    public EventSystem es;
    public GameObject menuCanvas;
    public List<Button> buttons;
    public Button resume;
    bool isMenu = false;
    public GameObject optionsCanvas;
    // Use this for initialization
    void Start () {
        
        buttons = new List<Button>();
        if (SceneManager.GetActiveScene().name == "Main Menu") {
            isMenu = true;
        } else {
           // menuCanvas = gameObject.transform.GetChild(0).gameObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !isMenu && !optionsCanvas.activeSelf) {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
			AkSoundEngine.SetState ("GameState", "Pause");
            if (Time.timeScale != 0f) { Time.timeScale = 0f; } 
            else { Time.timeScale = 1f; 
				AkSoundEngine.SetState ("GameState", "InGame");
			}
            es.SetSelectedGameObject(resume.gameObject);
        }else if(Input.GetKeyDown(KeyCode.Escape) && optionsCanvas.activeSelf) {
            HideOptions();
            ShowMenu();
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
    public void HideMenu() {
        menuCanvas.SetActive(false);
    }
    public void ShowMenu() {
        menuCanvas.SetActive(true);
    }
    public void ShowOptions() {
        optionsCanvas.SetActive(true);
    }
    public void HideOptions() {
        optionsCanvas.SetActive(false);
    }
    public void Resume() {
        menuCanvas.SetActive(false);
        es.SetSelectedGameObject(null);
        Time.timeScale = 1f;
		AkSoundEngine.SetState ("GameState", "InGame");
    }
    public void ExitGame() {
        Application.Quit();
        //temp code for editor
        //UnityEditor.EditorApplication.isPlaying = false;
    }
    public void LoadMainMenu() {
        SceneManager.LoadScene("Main Menu");
		AkSoundEngine.SetState ("Music", "Menu");

    }
    public void LoadGame() {
 
        SceneManager.LoadSceneAsync("Loading");
        SceneManager.LoadSceneAsync("Game");
		AkSoundEngine.SetState ("Music", "Ambient");
        
    }
    public void LoadCredits() {
        SceneManager.LoadSceneAsync("Credits");
    }
}

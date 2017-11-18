using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionsManager : MonoBehaviour {
    public Slider masterVol;
    public Slider musicVol;
    public Slider effectVol;
    public Toggle fullscreen;
    public Dropdown resolution;
	// Use this for initialization
	void Start () {
        AkSoundEngine.SetRTPCValue("Master_Volume", (int)masterVol.value);
        AkSoundEngine.SetRTPCValue("Music_Volume", (int)musicVol.value);
        AkSoundEngine.SetRTPCValue("SFX_Volume", (int)effectVol.value);
        
    }
	
	// Update is called once per frame
	void Update () {
        
	}
    public void SetMasterVolume() {
        AkSoundEngine.SetRTPCValue("Master_Volume", (int)masterVol.value);
    }
    public void SetMusicVolume() {
        AkSoundEngine.SetRTPCValue("Music_Volume", (int)musicVol.value);
    }
    public void SetEffectsVolume() {
        AkSoundEngine.SetRTPCValue("SFX_Volume", (int)effectVol.value);
    }
    public void SetResolution() {
        int width = 0;
        int height = 0;
        switch (resolution.value) {
            case 0:
                width = Screen.currentResolution.width;
                height = Screen.currentResolution.height;
                break;
            case 1:
                width = 1920;
                height = 1080;
                break;
            case 2:
                width = 1680;
                height = 1050;
                break;
            case 3:
                width = 1600;
                height = 900;
                break;
            case 4:
                width = 1280;
                height = 800;
                break;
            case 5:
                width = 1366;
                height = 768;
                break;
        }
        Screen.SetResolution(width, height, fullscreen.isOn);
    }
    public void SetFullScreen() {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullscreen.isOn);
    }

}

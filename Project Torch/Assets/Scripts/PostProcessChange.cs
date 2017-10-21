using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;


public class PostProcessChange : MonoBehaviour {

    #region Public Fields
    // the post processing profile attached to the camera
    public PostProcessingProfile profile;
    // variable that controls the daaahhkness
    public bool darkness;
    #endregion

    #region Private Fields
    // Local copy necessary processing settings
    private VignetteModel.Settings vignetteSettings;
    private ChromaticAberrationModel.Settings chromaticAberSettings;
    private GrainModel.Settings grainSettings;
    // copies of starting intensities for reference
    private float vignetteStartingIntensity;
    private float chromaticAbStartingIntensity;
    private float grainStartingIntensity;
    #endregion

    // Use this for initialization
    void Start ()
    {
        darkness = false;
        vignetteSettings = profile.vignette.settings;
        chromaticAberSettings = profile.chromaticAberration.settings;
        grainSettings = profile.grain.settings;

        // set starting intensities for easier code management
        vignetteStartingIntensity = vignetteSettings.intensity;
        chromaticAbStartingIntensity = chromaticAberSettings.intensity;
        grainStartingIntensity = grainSettings.intensity;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(darkness == true)
        {
            profile.grain.enabled = true;
            profile.chromaticAberration.enabled = true;
            DarknessCreep(darkness); // adjust darkness
        }
        else
        {
            profile.grain.enabled = false;
            profile.chromaticAberration.enabled = false;
            DarknessCreep(darkness); // adjust darkness
        }
	}

    /// <summary>
    /// Method controlling the visual representation of the darkness
    /// </summary>
    /// <param name="grow">Whether to grow or shrink the darkness. True for grow. False for shrink.</param>
    private void DarknessCreep(bool grow)
    {
        // copy current settings
        vignetteSettings = profile.vignette.settings;


        // change temporary settings
        switch(grow)
        {
            case true: // grow
                // increase values in small increments
                if (vignetteSettings.intensity < vignetteStartingIntensity) vignetteSettings.intensity += .005f;
                if (chromaticAberSettings.intensity < chromaticAbStartingIntensity) chromaticAberSettings.intensity += .005f;
                if (grainSettings.intensity < grainStartingIntensity) grainSettings.intensity += .005f;
                break;

            case false: // shrink
                // decrease values in small increments, making sure not to go below zero to avoid errors

                // vignette
                if (vignetteSettings.intensity > 0f)
                {
                    vignetteSettings.intensity -= .005f;
                    if (vignetteSettings.intensity < 0) vignetteSettings.intensity = 0;
                }

                // chromatic aberration
                if (chromaticAberSettings.intensity > 0f)
                {
                    chromaticAberSettings.intensity -= .005f;
                    if (chromaticAberSettings.intensity < 0) chromaticAberSettings.intensity = 0;
                }

                // grain
                if (grainSettings.intensity > 0f)
                {
                    grainSettings.intensity -= .005f;
                    if (grainSettings.intensity < 0) grainSettings.intensity = 0;
                }
                break;
        }

        // update actual settings
        profile.vignette.settings = vignetteSettings;
        profile.chromaticAberration.settings = chromaticAberSettings;
        profile.grain.settings = grainSettings;
    }
}

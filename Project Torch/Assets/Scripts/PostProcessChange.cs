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
    public bool enhancedRadius;
    #endregion

    #region Private Fields
    // Local copy necessary processing settings
    private VignetteModel.Settings vignetteSettings;
    private ChromaticAberrationModel.Settings chromaticAberSettings;
    private GrainModel.Settings grainSettings;
    // copies of starting intensities for reference
    private float vignetteStartingIntensity; // .6
    private float chromaticAbStartingIntensity; // .375
    private float grainStartingIntensity; // .11
    // desired current values
    private float desiredVignetteIntensity;
    private PlayerMovement playerMove;
    private float enhancedRadiusTime;
    // The camera
    private Camera gameCam;
    #endregion

    // Use this for initialization
    void Start ()
    {
        playerMove = GameObject.Find("Player").GetComponent<PlayerMovement>();
        darkness = false;
        enhancedRadius = false;
        enhancedRadiusTime = 0;
        vignetteSettings = profile.vignette.settings;
        chromaticAberSettings = profile.chromaticAberration.settings;
        grainSettings = profile.grain.settings;
        gameCam = GameObject.Find("Main Camera").GetComponent<Camera>();

        // set starting intensities for easier code management
        vignetteStartingIntensity = .6f;
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
        }
        else
        {
            profile.grain.enabled = false;
            profile.chromaticAberration.enabled = false;
        }
        DarknessCreep(); // adjust darkness
    }

    /// <summary>
    /// Method controlling the visual representation of the darkness
    /// </summary>
    private void DarknessCreep()
    {
        // copy current settings
        vignetteSettings = profile.vignette.settings;

        // adjust vignette center's y position
        vignetteSettings.center.y = gameCam.WorldToViewportPoint(playerMove.transform.position).y;

        // set desired vignette intensity
        if (enhancedRadius) affectRadius();
        else
        {
            if (darkness) desiredVignetteIntensity = vignetteStartingIntensity;
            else desiredVignetteIntensity = 0;
        }

        // determine grow or shrink
        bool grow = (desiredVignetteIntensity > vignetteSettings.intensity);

        // change temporary settings
        switch(grow)
        {
            case true: // grow
                // increase values in small increments
                if (vignetteSettings.intensity < desiredVignetteIntensity) vignetteSettings.intensity += .005f;
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

    private void affectRadius()
    {
        enhancedRadiusTime += Time.deltaTime;
        if(enhancedRadiusTime > 4f)
        {
            desiredVignetteIntensity = 1;
            if(enhancedRadiusTime > 10f)
            {
                enhancedRadiusTime = 0;
                enhancedRadius = false;
            }
        }
        else desiredVignetteIntensity = .33f;
    }
}

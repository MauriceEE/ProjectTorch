using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script exists for the sole purpose of holding the game's dialogue lines
/// </summary>
public class TextManager : MonoBehaviour {

    /// <summary>
    /// A huge batch of every set of dialogue lines in the game
    /// </summary>
    private Dictionary<string, string[]> lines;
    public Dictionary<string, string[]> Lines { get { return lines; } }

    void Start () {
        lines = new Dictionary<string, string[]>();

        //Here we will add all possible lines of dialogue in the game to the dictionary
        //FORMAT:
        lines.Add("ID Name for Lines",  //This is what you'll type in other scripts to access this set of lines
                                        //Consider a format like "King of Man - Default" or something easy to understand like that
            new string[] {              //This "new string[] {" is necessary to instantiate a string array like this
            "line1",                    //Just keep adding strings which represent lines of dialogue in this format
            "line2",                    //NOTE that each string here represents a chunk that will appear at once in a dialogue box
            "line4" });                 //At the end, add a "});" to close off the stuff opened earlier. That's all there is to it
        //Example:
        lines.Add("King of Man - Default",
            new string[]
            {
                "\nWe meet at last, mysterious warrior.",
                "\nI have heard your feats, helping my men defeat those dreaded Shadows.",
                "\nI know not who you are, but would ask that you help contribute to our cause.",
                "\nWe humans were once a proud race, but have been reduced to this sad state by those creatures.",
                "\nTheir stronghold is far to the west. I implore you, save what remains of my kingdom."
            });
        //^^Feel free to delete this or copy+paste it as a template or something
        //Just make one of these for every set of dialogue lines in the game and you're good
	}
}

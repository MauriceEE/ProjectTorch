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
        //The basic lines the King will say to you with no other triggers
        lines.Add("King of Man - Default",
            new string[]
            {
                "King of Man:\nGood traveler, I know not of your origin, be it the Serpent’s Earth or Dragon’s sky, but I beg of you to aid us in our time of need.",
                "King of Man:\nAid us as we repel the invaders from the shadows so that we may retake our place in this world!",
                "King of Man:\nTheir King’s fortress lies to the west.",
                "King of Man:\nLead the charge as I fulfill the last wish of the Serpents."
            });
        //^^Feel free to delete this or copy+paste it as a template or something
        //Just make one of these for every set of dialogue lines in the game and you're good

        //The lines the King of the Dark says when he's at his last 15% of health and can't move
        lines.Add("King of the Dark - Downed",
            new string[]
            {
                "King of the Dark:\nGive her back… You monsters.",
                "King of the Dark:\nMy daughter is more than a means to your ends.",
                "King of the Dark:\nShe is more than your kindling.",
                "King of the Dark:\nPlease... Do not let your King murder my daughter..."
            });

        //The Captain of the Guard's text when he meets the player in the village after they've protected it
        lines.Add("Captain of the Gaurd - Village",
            new string[]
            {
                "Captain of the Gaurd:\nWell met stranger.",
                "Captain of the Gaurd:\nWe’ve heard word of a foreign warrior defending this village against the oncoming hoards.",
                "Captain of the Gaurd:\nGather your strength friend, for today we retake our birthright.",
                "Captain of the Gaurd:\nWe will seize the opportunity you’ve opened here and charge into the heart of the enemy!"
            });

        //The Captain of the Guard's text when he reaches the Sentinels
        lines.Add("Captain of the Gaurd - Sentinels",
            new string[]
            {
                "Captain of the Gaurd:\nThe strength of man shall not falter here.",
                "Captain of the Gaurd:\nWhile we can force the way open, their reinforcements shall not sit idly.",
                "Captain of the Gaurd:\nWhen the gate opens, you, the strongest of us all, should bring a close to this war.",
                "Captain of the Gaurd:\nWe will hold the line here as you battle their King.",
                "Captain of the Gaurd:\nHerald us all to the Serpents’ Promised Flame!"
            });
    }
}

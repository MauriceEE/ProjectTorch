using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EndingVariables {

    public static int endNumber = 1;

    public static bool princessSaved = false;

    public static string[] getEnding()
    {
        string[] end1 = { "As the King of the Dark lay slain, man rejoiced. No longer would they merely make do with the False Flame.From this day on, they would bear a true flame, a privilege long since coveted by the serpents, a desire inherited by man.",
        "Yet more royal blood would be spilt. A King and his Princess, meet their ends on this day. He, battered by war. She, sacrificed in the name of a perceived Right.",
        "The Adjudicator was found dead by the side of the murdered King. A burnt out torch of a long dead age, clutched in a bastard hand." };

        string[] end2 = { "It seemed the people of The Shadow were too late. A slain Princess lay at their feet, dead for nothing but a lust for power. A waste of a precious life; a waste of a once proud Kingdom. In a blind rage, the rest of man was hunted, slaughtered and made extinct. Never again would such a needless war be thrust upon this world again.",
        "The Adjudicator’s torch was all that was found of the wanderer. Judgement had been passed and an ancient conflict had been decided. The Shadow, born from Dragon ashes, had at last prevailed over the usurpers.",
        "With the sentence of an eternal exile carried out, the Adjudicator finally rested in the afterlife, liberated of an unloving duty." };

        string[] end3 = { "Tearing through the crumbling world of man, the princess is returned to her people. The world must always move forward, changing hands as one empire falls. The will of the strong and vengeful prevail over the old ways. The Shadow claims this world, settling into a quiet peace.",
        "The Adjudicator, too settles for a time, an eternal sentence of wandering carried out at long last. Now is the time for rest.",
        "It is rather disappointing. While I may have told man’s King of the Shadow’s Princess, it seems that nothing came of it. In time though, we will all return to the ocean. It is the water’s will that the flame be drowned. Do not forget this, descendants of the Divine Beasts." };

        switch (endNumber)
        {
            case 1:
                return end1;
            case 2:
                if(princessSaved == false)
                {
                    return end2;
                }
                else
                {
                    return end3;
                }
            default:
                return null;
        }
    }
}

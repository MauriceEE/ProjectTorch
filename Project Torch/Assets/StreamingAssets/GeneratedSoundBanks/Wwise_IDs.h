/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID BUTTONPRESS = 317641954U;
        static const AkUniqueID DASH = 1942692385U;
        static const AkUniqueID HUMAN_BASIC_ATTACK = 3244706930U;
        static const AkUniqueID HUMAN_BRUTE_ATTACK = 3823776748U;
        static const AkUniqueID HUMAN_SPEARMAN_ATTACK = 892714673U;
        static const AkUniqueID MUSIC_AMBIENT = 3611180283U;
        static const AkUniqueID MUSIC_COMBAT = 3944980085U;
        static const AkUniqueID MUSIC_DEATH = 3678931145U;
        static const AkUniqueID MUSIC_START = 3725903807U;
        static const AkUniqueID PAUSE = 3092587493U;
        static const AkUniqueID RESUME = 953277036U;
        static const AkUniqueID SCROLL = 454121546U;
        static const AkUniqueID SHADOW_BASIC_ATTACK = 3817024617U;
        static const AkUniqueID SHADOW_BRUTE_ATTACK = 3498381063U;
        static const AkUniqueID SHINE = 3055381918U;
        static const AkUniqueID SLASH = 4107276880U;
        static const AkUniqueID THRUST = 2197345151U;
        static const AkUniqueID TORCH_PLANT = 2547765723U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace GAMESTATE
        {
            static const AkUniqueID GROUP = 4091656514U;

            namespace STATE
            {
                static const AkUniqueID INGAME = 984691642U;
                static const AkUniqueID PAUSE = 3092587493U;
            } // namespace STATE
        } // namespace GAMESTATE

        namespace MUSIC
        {
            static const AkUniqueID GROUP = 3991942870U;

            namespace STATE
            {
                static const AkUniqueID AMBIENT = 77978275U;
                static const AkUniqueID COMBAT = 2764240573U;
                static const AkUniqueID DEATH = 779278001U;
            } // namespace STATE
        } // namespace MUSIC

        namespace PLAYERLIFE
        {
            static const AkUniqueID GROUP = 444815956U;

            namespace STATE
            {
                static const AkUniqueID ALIVE = 655265632U;
                static const AkUniqueID DEAD = 2044049779U;
            } // namespace STATE
        } // namespace PLAYERLIFE

    } // namespace STATES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID MASTER_VOLUME = 4179668880U;
        static const AkUniqueID MUSIC_VOLUME = 1006694123U;
        static const AkUniqueID PLAYERHEALTH = 151362964U;
        static const AkUniqueID PLAYERMOVEMENTSPEED = 2557281390U;
        static const AkUniqueID SFX_VOLUME = 1564184899U;
        static const AkUniqueID TIMEREMAINING = 1691631674U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID SFX = 393239870U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MASTER_SECONDARY_BUS = 805203703U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID SFX = 393239870U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID OUTSIDE_VERB = 3695620394U;
    } // namespace AUX_BUSSES

}// namespace AK

#endif // __WWISE_IDS_H__

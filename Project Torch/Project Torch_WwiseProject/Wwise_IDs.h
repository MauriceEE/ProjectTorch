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
        static const AkUniqueID AMBIENT = 77978275U;
        static const AkUniqueID BUTTONPRESS = 317641954U;
        static const AkUniqueID COMBAT = 2764240573U;
        static const AkUniqueID ENEMYHUMANATTACK = 2442141738U;
        static const AkUniqueID ENEMYHUMANBRUTEATTACK = 2271782150U;
        static const AkUniqueID ENEMYHUMANBRUTEDASH = 3590139274U;
        static const AkUniqueID ENEMYHUMANBRUTEDEATH = 3408497084U;
        static const AkUniqueID ENEMYHUMANBRUTEHIT = 2421117389U;
        static const AkUniqueID ENEMYHUMANBRUTEWALK = 735920725U;
        static const AkUniqueID ENEMYHUMANDASH = 3056021366U;
        static const AkUniqueID ENEMYHUMANDEATH = 3658215168U;
        static const AkUniqueID ENEMYHUMANHIT = 2928579217U;
        static const AkUniqueID ENEMYHUMANSPEARMANATTACK = 3466660987U;
        static const AkUniqueID ENEMYHUMANSPEARMANDASH = 3304273307U;
        static const AkUniqueID ENEMYHUMANSPEARMANDEATH = 3685561835U;
        static const AkUniqueID ENEMYHUMANSPEARMANHIT = 143401290U;
        static const AkUniqueID ENEMYHUMANSPEARMANWALK = 2131966636U;
        static const AkUniqueID ENEMYHUMANWALK = 2268161225U;
        static const AkUniqueID ENEMYSHADOWATTACK = 3335836155U;
        static const AkUniqueID ENEMYSHADOWBRUTEATTACK = 2295227437U;
        static const AkUniqueID ENEMYSHADOWBRUTEDASH = 2691316833U;
        static const AkUniqueID ENEMYSHADOWBRUTEDEATH = 2859281905U;
        static const AkUniqueID ENEMYSHADOWBRUTEHIT = 4206366272U;
        static const AkUniqueID ENEMYSHADOWBRUTEWALK = 2857301246U;
        static const AkUniqueID ENEMYSHADOWDASH = 1517887259U;
        static const AkUniqueID ENEMYSHADOWDEATH = 3179006571U;
        static const AkUniqueID ENEMYSHADOWGLOWERATTACK = 1138021477U;
        static const AkUniqueID ENEMYSHADOWGLOWERDASH = 3498057465U;
        static const AkUniqueID ENEMYSHADOWGLOWERDEATH = 3876290745U;
        static const AkUniqueID ENEMYSHADOWGLOWEREXPLOSION = 2420314586U;
        static const AkUniqueID ENEMYSHADOWGLOWERHIT = 3945367480U;
        static const AkUniqueID ENEMYSHADOWGLOWERWALK = 3385257286U;
        static const AkUniqueID ENEMYSHADOWHIT = 3970335178U;
        static const AkUniqueID ENEMYSHADOWWALK = 345580588U;
        static const AkUniqueID GAMEEND = 2197986718U;
        static const AkUniqueID MUSIC_START = 3725903807U;
        static const AkUniqueID PAUSE = 3092587493U;
        static const AkUniqueID PLAYERDASH = 2525052962U;
        static const AkUniqueID PLAYERDEATH = 1656947812U;
        static const AkUniqueID PLAYERHIT = 3831688773U;
        static const AkUniqueID PLAYERSHINE = 2522435555U;
        static const AkUniqueID PLAYERSLASH = 3910121377U;
        static const AkUniqueID PLAYERSLASHDEFLECTED = 3747613209U;
        static const AkUniqueID PLAYERTHRUST = 41749840U;
        static const AkUniqueID PLAYERWALK = 1592629277U;
        static const AkUniqueID RESUME = 953277036U;
        static const AkUniqueID SCROLL = 454121546U;
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
                static const AkUniqueID MENU = 2607556080U;
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

    namespace SWITCHES
    {
        namespace AMBIENTMUSICSTATUS
        {
            static const AkUniqueID GROUP = 1546524390U;

            namespace SWITCH
            {
                static const AkUniqueID GREATTHREAT = 70773388U;
                static const AkUniqueID MEDIUMTHREAT = 3192291216U;
                static const AkUniqueID MILDTHREAT = 347704107U;
                static const AkUniqueID SADNESS = 478431442U;
                static const AkUniqueID WONDER = 2976041628U;
            } // namespace SWITCH
        } // namespace AMBIENTMUSICSTATUS

        namespace COMBATMUSICINTENSITY
        {
            static const AkUniqueID GROUP = 224141993U;

            namespace SWITCH
            {
                static const AkUniqueID HIGH = 3550808449U;
                static const AkUniqueID LOW = 545371365U;
                static const AkUniqueID MEDIUM = 2849147824U;
            } // namespace SWITCH
        } // namespace COMBATMUSICINTENSITY

    } // namespace SWITCHES

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
        static const AkUniqueID REVERB = 348963605U;
    } // namespace AUX_BUSSES

}// namespace AK

#endif // __WWISE_IDS_H__

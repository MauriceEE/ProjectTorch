# ProjectTorch
2.5D Dark Fantasy Game
<h1>Project Torch ChangeLog</h1>
<h2>Joey Tong - 9/10/17 - 14:30 - "First Log"</h2>
<ul><li>This is the format for change logs</li></ul>
<h2>Joey Tong - 9/17/17 - 2:00 - "Dialogue"</h2>
<ul><li>Dialogue System implemented</li><li>Placeholder textures & test code to be removed in future</li></ul>
<h2>Connor Menard - 9/19/17 - 20:00 - "Combat"</h2>
<ul><li>Controller input and some simple combat input/system</li><li>Hitboxes and frame data tools in editor</li></ul>
<ul><li>Reduced player speed in X from .1 to .075</li><li>Reduced player speed in Y from .1 to .055</li><li>Reduced size of player sprite placeholder and hitboxes</li><li>Increased Thrust start-up from 12 to 14 frames and recovery from 10 to 12 frames</li><li>Changed Slash input on controller to actually be Square on a PS4 controller</li></ul>
<h2>Joey Tong - 9/20/17 - 2:00 - "Dialogue API"</h2>
<ul><li>Dialogue API implemented.</li><li>Call AddDialogueSequence(string[]) to display a sequence of dialogue lines to the screen.</li>
<li>Format dialogues as arrays of dialogue. Lines should be formatted as such: Charactername:\nDialogue goes here.</li>
<li>Placeholder textures & test code to be removed in future</li></ul>
<h2>Connor Menard - 9/23/17 - 18:00</h2>
<ul><li>Finished combat basics (no shine yet), added dummy enemies</li><li>Torch timer in the top left counts down visually</li></ul><h2>Joey Tong - 9/30/17 - 18:41 - "Backgrounds"</h2>
<ul>
  <li>Battleground assets imported and configured</li>
  <li>Camera Following implemented</li>
  <li>Background scrolling & parallax layers implemented</li>
</ul>
<h2>Connor Menard - 10/1/17 - 2:44</h2>
<ul><li>Lots and lots of code cleanup and hitbox shenanigans</li><li>Enemies now recieve knockback on attacks</li></ul>
<h2>Maurice Edwards - 10/3/17 19:37 </h2>
<ul>
<li>Decreased base movement speed</li>
<li>Tweaked dash speed and increased dash frames</li>
<li>The player can now move during slash</li>
<li>Enabled the player to chain Slash into itself up to 2 additional times or into another attack once</li>
<li>Increased Slash recovery and decreased Slash damage</li>
<li>Added Cancel method to cancel any attacks and dash. Press Spacebar to test Cancel. DashTime is public now, by the way</li>
<li>Added HitStop method for combat feel, but nothing calls it yet</li>
</ul>
<h2>Connor Menard - 10/4/17 - 17:00</h2>
<ul><li>Fixed moving while attacking bug</li><li>Started Shine, still bugged though</li></ul>
<h2>Connor Menard - 10/4/17 - 17:30</h2>
<ul><li>Ok I fixed shine</li>
<h2>Steven Ma - 10/4/17 - 1:41</h2>
<ul><li>Finished up the level blocking</li>
<h2>Connor Menard - 10/7/17 - 21:30</h2>
<ul><li>Started doing stuff for the encounter manager and overall added more enemy stuff</li><li>Put in Judi and a shadow monster sprite</li></ul>
<h2>Maurice Edwards - 10/10/17 - 1:11 </h2>
<ul>
  <li>Coded reaction selection for enemies, but not the reactions themselves fully</li>
  <li>Added a function in the EM that checks if any enemies are attacking. I have it call that before it chooses an enemy to attack and before any enemies want to counterattack</li>
  <li>Added isAttacking variable to enemies for use in offensive regulations for the EM</li>
</ul>
<h2>Connor Menard - 10/15/17 - 5:00</h2>
<ul><li>Fleshed out encounter manager/encounter objects</li><li>Mostly finished reaction states, still needs number tweaking and a bug or two squashed</li></ul>
<h2>Maurice Edwards - 10/18/17 and 10/20/17 - 4:43</h2>
<ul>
  <li>Adjusted enemy attack initiations. Enemies now dynamically adjust speed and attack range to catch fleeing players while approaching</li>
  <li>Enemy attack range,  awareness radii, arrival radii, and start-up changed. See build notes on the document for exact numbers</li>
  <li>Enemies now move at half speed while surrounding the player</li>
  <li>Changed Shine keyboard input to the 'I' key</li>
  <li>Coded cancel and hitstun method for enemies, who now take 20 frames of hitstun per attack</li>
  <li>Added post processing effects. The darkness effect occurs whenever the player eneters an initially dark location</li>
  <li>Post processing effects are controllable via code via PostProcessChange script and directly via the profile</li>
</ul>
<h2>Connor Menard - 10/20/17 - 23:00</h2>
<ul><li>Added outline for the TextManager script</li></ul>
<h2>Steven Ma - 10/21/17 - 18:18</h2>
<ul><li>Added the dialogue lines to the TextManager script</li>
<h2>Connor Menard - 10/21/17 - 20:30</h2>
<ul><li>Added interactivity functions, can talk with King of Man (Uses default dialogue only)</li></ul>
<h2>Maurice Edwards 10/24/17 - 20:01</h2>
<ul>
<li>Fixed prefab issues. Kept old enemy prefab in just in case the code references it or something, but nothing in the scene uses it</li>
</ul>
<h2>Connor Menard - 10/25/17 - 18:30</h2>
<ul><li>Finished shine and improved knockback functionality</li></ul>
<h2>Connor Menard - 10/28/17 - 18:12</h2>
<ul><li>Reworked zones and zone transitions</li><li>Rough outline for level blocking in place</li></ul>
<h2>Maurice Edwards - 10/29 - 15:16</h2>
<ul>
  <li>Fixed darkness. It is now completely automatic. Just tell the profile changer if you want darkness and it will do the rest</li>
  <li>Consecutive attacks now have less start-up frames</li>
  <li>Created Shadow Brute, Spearman, and Normal Human prefabs but did not implement any in the levels yet</li>
</ul>
<h2>Steven Ma - 10/30/17 - 2:31</h2>
<ul><li>Made all the backgrounds close to what they should be</li></ul>
<h2>Connor Menard - 11/1/17 - 5:00</h2>
<ul><li>More Aggression stuff (not done yet though)</li><li>Shine hitbox visible in-game</li></ul>
<h2>Maurice Edwards - 11/2/17 - 19:24</h2>
<ul>
  <li>Increased player hitbox size</li>
  <li>Increased Shine hitbox and increased Shine recovery frames from 10 to 17</li>
  <li>Successful counter hits with Shine reduce its recovery frames to 1 frame</li>
  <li>Player should now be granted 30 HP upon killing all enemies in an encounter. This is inconsistent however</li>
  <li>Added instructions UI element and functionality. Check the InstructionManager script to see how to add and use them</li>
  <li>Added null reference checks for the dialogue queue in DialogueManager</li>
  <li>Adjusted encounter spacing and enemy contents in every level</li>
  <li>Deleted depricated OldReact method in Enemy class</li>
  <li>Added brute human prefab</li>
</ul>
<h2>Steven Ma - 11/3/17 - 4:26</h2>
<ul><li>Added in the WarZoneStage2 and TrueHumanStage1 levels</li></ul>
<h2>Connor Menard - 11/4/17 - 18:55</h2>
<ul><li>Implemented respawning</li></ul>
<h2>Connor Menard - 11/4/17 - 20:07</h2>
<ul><li>Implemented aggression</li><li>NOTE: There's currently a game breaking bug when an enemy goes to attack the player... sometimes. Haven't squashed it yet</li></ul>
<h2>Garret Reynolds - 11/9/17 - 12:15PM</h2>
<ul>
  <li>Added music hooks to enemy manager and temp sound</li>
  <li>added SFX hooks for Shine, Thrust, and Slash in PlayerCombat</li>
  <li>added SoundBank loader and Music_Start event to WwiseGlobal object in Unity</li>
  <li>moved GeneratedSoundBanks to StreamingAssets for ease of adding to build </li>
</ul>
<h2>Connor Menard - 11/11/17 - 4:19PM</h2>
<ul>
  <li>Added glower and status effects</li>
  <li>Encounter manager related bug fixes</li>
</ul>
<h2>Maurice Edwards - 11/12/17 - 0:55 </h2>
<ul>
  <li>I did a lot of combat changes. Read the build notes on the google doc in the Feedback folder.</li>
</ul>
<h2>Steven Ma - 11/12/17 - 8:14 </h2>
<ul>
  <li>Replaced enemy prefabs with correct prefabs.</li>
  <li>Added in dialogue for braziers.</li>
  <li>Removed all level end points from view.</li>
</ul>
<h2>Garret Reynolds - 11/9/17 - 12:15PM</h2>
<ul>
  <li>Added events for spearman attacks and non-integrated death music</li>
</ul>
<h2>Steven Ma - 11/15/17 - 17:22</h2>
<ul>
  <li>Defined active areas.</li>
  <li>Fixed level assignment issues with human soldiers.</li>
</ul>
<h2>Maurice Edwards - 11/16/17 - 2:00</h2>
<ul>
  <li>Finished coding categorical spawns. Even numbers spawn shadows, odds spawn humans, with classes being in order from basic, to brute, to special.</li>
  <li>Increased enemy start-up by 10 for every enemy</li>
  <li>Enemies now have windows of time after being hit multiple times where they have increased chances of doing reactions</li>
  <li>A lot of miscellaneous combat changes. Read the build notes on the google doc in the Feedback folder.</li>
</ul>
<h2>Maurice Edwards - 11/16/17 - 20:52</h2>
<ul>
  <li>Increased shine recovery to 20 from 17</li>
  <li>Shine now has a spotlight turn on during active frames to denote effective range</li>
  <li>Shine now slows enemies to a third of their movement speed if it doesn't land the counter</li>
  <li>Enemies now spawn lights when hit and stay lit for either 2 or 5 seconds, depending on if the shine hit was counter or not</li>
  <li>Increased animation speeds for Shine and Thrust</li>
  <li>Increased base darkness vignette size to .8 from .6</li>
</ul>
<h2>Steven Ma - 11/18/17 - 15:58</h2>
<ul>
  <li>Made all graves and grave dialogue.</li>
  <li>Finished Princess Rescue dialogue.</li>
  <li>Changed level layouts for many levels.</li>
</ul>
<h2>Garret Reynolds - 11/18/17 - 1:00AM</h2>
<ul>
  <li>Added new combat and ambient music</li>
  <li>compressed audio to .vorbis</li>
</ul>
<h2>Garret Reynolds - 11/18/17 - 4:00PM</h2>
<ul>  
  <li>added menu music</li>
</ul>
<h2>Connor Menard - 11/20/17 - 1:42PM</h2>
<ul>
  <li>Added SoundManager</li>
  <li>Reworked current sound system (still requires renaming of sound events)</li>
  <li>Added required sound cues (except for game end screen)</li>
</ul>
<h2>Garret Reynolds - 11/21/17 - 10:10PM</h2>
<ul>
  <li>added sound manager events to wwiseproject</li>
  <li>renamed sound events to match sound manager</li>
  <li>pausing, resuming, and navigating to the menu now affects the music</li>
</ul>
<h2>Garret Reynolds - 11/22/17 - 2:53PM</h2>
<ul>
  <li>began replacing player and basic enemy SFX</li>
</ul>
<h2>Steven Ma - 11/22/17 - 6:43PM</h2>
<ul>
  <li>Added in more difficult boss encounters with different prefabs</li>
</ul>
<h2>Garret Reynolds - 11/25/17 - 5:11PM</h2>
<ul>
  <li>All enemies now have attack, hit, and death sounds.</li>
  <li>All player events have sounds (PlayerWalk is still broken).</li>
</ul>
<h2>Maurice Edwards - 11/26/17 - 0:34</h2>
<ul>
  <li>Reworked Brute. See Build Notes doc for details</li>
  <li>Basic shadow enemies now turn partially opaque when going to attack</li>
  <li>Adjusted Spearman start-up frames, attack range, and attack hitbox heights. See build notes for exact numbers</li>
  <li>Adjust Glower across the board. See Build Notes doc for details.</li>
  <li>Added and improved tutorials. Had to rewrite InstructionManager and InstructionTrigger</li>
  <li>Adjusted some later encounters to better introduce enemies</li>
  <li>Normalized damage and start-up frames of boss enemies</li>
</ul>
<h2>Connor Menard - 11/26/17 - 10:42PM</h2>
<ul>
  <li>Added waves/wave manager</li>
  <li>Can no longer interact with NPCs when in combat, allowing for interactable enemies</li>
</ul>
<h2>Maurice Edwards - 11/29/17 - 2:37</h2>
<ul>
  <li>Braziers now properly determine the next level’s darkness status</li>
  <li>Adjsuted UI scaling</li>
  <li>Changed initial torch time to 900 (15 minutes)</li>
  <li>Timer now dislays time in minutes and seconds, gradually changes color, and displays a phrase at the end instead of the time</li>
  <li>Player can no longer light braziers when they have no time on the torch. Added instruction explaining this.</li>
  <li>EnemyManager now checks what the selected enemy is doing before it tells it to attack and removes it from the grid</li>
  <li>Re-enabled stuns</li>
</ul>
<h2>Steven Ma - 11/29/17 - 6:40</h2>
<ul>
  <li>Added in all Unhostile Enemy Dialogue</li>
  <li>Adjsuted some levels slightly</li>
</ul>
<h2>Connor Menard - 11/29/17 - 8:03PM</h2>
<ul>
  <li>Removed enemy temp hitboxes </li>
  <li>Made it possible for enemies to auto-engage encounters</li>
  <li>Started FlagManager final boss waves stetup for Steven</li>
  <li>Glower projectile now is reflect-able</li>
  <li>There's still some debugging junk in here... Sorry if it bothers you, I'm working on it</li>
</ul>
<h2>Garret Reynolds - 12/1/17 - 10:03PM</h2>
<ul>
  <li>Defined low, medium, and high intensity combat music loops and transitions.</li>
  <li>Wwise Game Parameter CombatIntensity from values 0-3 determines which set of combat music will play. 1 = low, 2 = medium, 3 = high</li>
  <li>audio mix adjustments</li>
</ul>
<h2>Maurice Edwards - 12/3/17 - 2:11</h2>
<ul>
  <li>Updated all enemy prefabs with exclamation points except boss enemies</li>
  <li>Removed instances of boss enemies</li>
  <li>Allied enemies now display exclamation points to convey aggression levels and betrayal</li>
  <li>Put in King of the Dark sprite</li>
  <li>Adjusted level bounds to make levels able to be completed</li>
  <li>Implemented waves but left them disabled since we likely won't have time to balance and debug them</li>
</ul>
<h2>Connor Menard - 12/4/17 - 9:55PM</h2>
<ul>
  <li>May have fixed the freezing bug (more testing required) </li>
  <li>Removed the encounter with no encounter enemies in it</li>
  <li>Added a failsafe case if there are more than 6 enemies in the grid - game no longer crashes</li>
  <li>Fixed some enemies in incorrect zones</li>
</ul>
<h2>Garret Reynolds - 12/5/17 - 10:37AM</h2>
<ul>
  <li>implemented zone specific music</li>
  <li>fixed menu music</li>
  <li>increased lower engine pool size </li>
</ul>
<h2>Garret Reynolds - 12/6/17 - 12:22PM</h2>
<ul>
  <li>replaced UI sounds with dark sounds</li>
  <li>menu music is now less repetitive when left idling</li>
</ul>

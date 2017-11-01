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
<h2>Steven Ma - 11/1/17 - 2:41</h2>
<ul><li>Added the War Zone level and added a trigger for them in the FlagManager</li></ul>
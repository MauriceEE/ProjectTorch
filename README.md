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
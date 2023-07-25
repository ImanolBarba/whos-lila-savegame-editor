# whos-lila-savegame-editor
A savegame editor for the game "Who's Lila?"

This is a save editor for the persistent game data (endings obtained, palettes unlocked, etc), not the individual save points at certain game locations.

Features:
- Can automatically load the current user save data
- Can load save data from path
- Backs up the original save in the same location before modifying
- Can create and edit blank save data using the "New" menu option

Game related features:
- Can unlock all endings and mark as discussed with Yu
- Can change persistent game flags
- Is able to unlock all street addresses, including the Sunshine cafe and Ryibkin's apartment (secret location)
- Can unlock all 17 palettes included in the game, as well as add new custom ones

How to use
--
This project requires the game Assembly and Unity Assembly as dependencies. The quickest way to use this is:
- Get the files from the Releases section (or build from source using Visual Studio)
- Put the files on a folder
- Copy all files from `C:\Program Files (x86)\Steam\steamapps\common\Who's Lila\WhosLila_Data\Managed` (or wherever it is you installed the game) to the folder you created with save editor files
- Then, run the editor executable

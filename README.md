# FreeCamMod (first version)
Mod for the Alpha Outer Wilds version 1.2 that adds a free cam to the game

### How do I install it?

1. Download the repo. and open the directory `Mod Instaling Kit`

2. Place all the files from that directory in `OuterWilds_Alpha_1_2_Data\Managed`

3. Run the executable `FreeCamModInstaller`, it will check if the mod hasn't been already installed, and if not, will ask you if you want to do so. If no error occures it means that the mod has been succesfully installed and there will be a new file called `Assembly-CSharp-ModLoaded.dll`

4. Rename this new file to `Assembly-CSharp.dll` (The reason to why it doesn't change the original file is so that a backup can be made even after using the executable)

5. Run the game! 

### How do I uninstall it?

Run the executable `FreeCamModInstaller`, it will check if the mod has been already installed, and if so, will ask you if you want to uninstall it. If no error occures it means that the mod has been succesfully uninstalled and the file `Assembly-CSharp.dll` will have been modified to get rid of the mod. After that, excluding any file from the directory `Mod Instaling Kit` won't cause any harm.

### What it does?

It allows the player to use a freecam while in the game ( the cam still doen't work )

### How do I use it?

1. Press **CapsLock**, it will activate the camera

2. Use **WASD** to move and **TAB** to go faster

3. Press **CapsLock** again if you want to go back to the player's camera

### Things to keep in mind

- There could be some bugs, so please send a *issue* about what you find in this repo.

- The player will *slide* in the ground if the player was in the air when activating the camera. 

- Pressing **Esc** would open the settings menu normaly, but if you are in the free cam it won't apear, so press **CapsLock** again to go back to it. The game will freese in that state, so it's a good way to take pictures while everything is static.

- The camera might appear to be going up or down sometimes, that's is caused by the rotation of the planets.
  

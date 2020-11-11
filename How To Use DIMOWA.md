# How To Use DIMOWA
   So you want to create a mod (for OW Alpha I assume) with DIMOWA hun? Well, this guide will try to help you to get everything ready for that! This won't explain **how to mod** (I will try to cover that another time). 
   
   The editor that I'll be explaining with will be Visual Studio 2017, but it should work others too.
  
## Starting the Solution
   
   After you have created a solution, make a `.net Framework` project in the **.net** version **2.0**, the reason to why this version is because the game's assembly was made using it, so it **needs** to be exactly it. Open the references and reference `IMOWAAnotations.dll`, `UnityEngine.dll` and `Assembly-CSharp.dll` (this last two from the OuterWilds_Alpha_1_2_Data\Managed, a good tip would be to make a copy of the last one, because it may be changed by DIMOWA when you install mods), and if you want to work with [Harmony2](https://github.com/pardeike/Harmony) reference `0Harmony.dll`, the .net 2.0 version from [0x0ade](https://github.com/0x0ade) which comes with the other files, and `HarmonyDnet2Fixes.dll`.
  
   Now that all the files are correctly referenced, you will need to create a `ModInnit` method to the class of the mod, it doesn't need to be called exactly that, but it is usefull so others can identify it easily. That method **needs** to **be public**, **be static**, **take an string as a parameter** (it will be receiving the name of the mod back) and **return nothing (void)**. You will need to add annotations to it too. Annotations are the things written inside the -> [ ], for example:
   
    [IMOWAModInnit("Some game class", "Some method from that game class", modName = "Really Cool Mod, pls play with it")]
    static public void ModInnit(string modName)
    {
        Debug.Log(modName + " started, have fun!"); // This is a really usefull place to check if the mod is running at all
    }
 
  This annotations, that come from `IMOWAAnotations.dll`, are the way DIMOWA detects a dll as a valid mod, and how it knows where it should make the patches on the game. A good idea is to use classes that make sense with the objective of the mod, and methods that occur early in the game (*Start* and *Awake* for example). This method that you just made is your door to add and change stuff in the game, so add all the scripts to game objects and do all Harmony patches that you want there.
  
  ## Testing the mod
  
  To test you mod you will need to compile it, and place the dll that comes from it in the `mods` folder. Using DIMOWA, you will need to install it and you will be ready to check if everyting is working fine. If you make any changes to your mod don't even worry about touching the Installer, you can just replace the old file with the new one and everything will work fine (unless you change the name too, in that case, unistall the mod and do all that process again). If you will be using multiples files, place only the file with the `ModInnit` method in that folder, the other should be kept with DIMOWA. DIMOWA, for now, doesn't have a way to debug in real time the library, so use `Debug.Log()` (the one that comes from Unity) for that, everything that is logged is kept in a file called `output_log` that exists in `\OuterWilds_Alpha_1_2_Data`.
  
  
  Well, that is all folks! If you have any question or saw that something in this guide is incomplete or blantly wrong, please make an [issue](https://github.com/ShoosGun/DIMOWA/issues/new) and write all that you want.
  
  
  
  

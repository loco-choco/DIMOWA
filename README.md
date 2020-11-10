# DIMOWA 1.0.2 - Debugger e Instalador de Mods de Outer Wilds Alpha (1.2)

An universal mod installer (and future mod debugger) for Outer Wilds Alpha (version 1.2)

<img src="https://github.com/ShoosGun/DIMOWA/blob/main/Icon%20and%20other%20images/DIMOWA_icon.png"  width="224" height = "289" >


### How do I install and use it?

1. Download the latest Release version

2. Place all the files in `OuterWilds_Alpha_1_2_Data\Managed`.

3. Create a folder called **mods** (it needs to be named exactly like that), that is where all the mods dlls that you want to install or just store should be kept

4. Run the executable, it will search the `mods` folder for valid mods dlls, and then will make a list of them telling you if they are already installed or not (**Attention:** the installer only knows if a mod is installed if its dll is on the `mods` folder, so before deleting anything check if the program detects it as installed, otherwise the game will give out errors) 

5. When a mod is already installed you just need to run the game, have fun! 

### How do I uninstall it?

Just delete the files that you took from the repository folder, but again, before doing so, check if any mod is installed.

### PSAQ (Probabily Some Asked Questions)

#### Why some files keep poping in and out of existance when I'm installing/unistalling a mod?

That happens because when the mod manager uninstall a mod it deletes the ddl from `Managed` (it doesn't do the same to the ones on the `mods` folder), and when it installs one, it copies it from the `mods` folder to `Managed`.


#### What mods are compatible with it?

For now only the [OWBA](https://github.com/ShoosGun/OWBA) bundle is compatible, but soon all the other mods that I made will be too.

## Credits
  - Thanks [ioncodes](https://github.com/ioncodes) for creating [dnpatch](https://github.com/ioncodes/dnpatch)
  - Thanks [Raicuparta](https://github.com/Raicuparta), [Mister_Nebula](https://github.com/misternebula), [AmazingAlek](https://github.com/amazingalek) and [TAImatem](https://github.com/TAImatem) for creating [OWML](https://github.com/amazingalek/owml) which inspired me to create my own for the Alpha
  - And thanks [Andreas Pardeike](https://github.com/pardeike/) for creating [Harmony](https://github.com/pardeike/Harmony) and [0x0ade](https://github.com/0x0ade) for making a .net 2.0 compatible version of it
  

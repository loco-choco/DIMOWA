# DIMOWA 1.0.3 - Debugger e Instalador de Mods de Outer Wilds Alpha (1.2)

An universal mod installer (and future mod debugger) for Outer Wilds Alpha (version 1.2)

<img src="https://github.com/ShoosGun/DIMOWA/blob/main/Icon%20and%20other%20images/DIMOWA_icon.png"  width="224" height = "289" >


### How do I install it?

1. Donwload the .zip called `DIMOWAFiles`

2. Extract the files from it and move them to the `Managed` folder inside the game files (`\OuterWilds_Alpha_1_2_Data\Managed`)

3. Run `dimowa Installer & Unistaller.exe` and type *s* or *y* to confirm the installation of DIMOWA

4. Run `DIMOWA.exe`, it will create a folder called **mods** inside `Managed`, that will be the place where you should add the mods dlls

### How do I use it?

#### Installing mods
1. Have a **valid** mod dll in the `mods` folder (mods that were made for DIMOWA 1.0.2 aren't compatible with 1.0.3 and having them on that folder will crash the program)

2. Select the number from the list that is prompted for you, or write *ia* or *it* to intall all the mods

3. Write *yes* or *sim* to install the mod, you can then type *r* to refresh the main page and check to see if the mod is installed

4. Some mods use assets like 3d meshes, textures and sounds, and for them to work you need to create a folder called `Assets` and place it inside `OuterWilds_Alpha_1_2_Data`, then place the assets inside that folder (the path of those files should look like this: `\OuterWilds_Alpha_1_2_Data\Assets\[name  of the file].wav`)

#### Uninstalling mods
1. //

2. Select the number from the list that is prompet for you, or write *ua* or *dt* to unintall all the mods

3. Write *yes* or *sim* to uninstall the mod, you can then type *r* to refresh the main page and check to see if the mod is uninstalled


### How do I uninstall it?

1. Run `dimowa Installer & Unistaller.exe` and type *s* or *y* to unistall DIMOWA

2. Delete all the files from DIMOWA and from other mods that you might have downloaded

* You don't need to unistall the mods to safely unistall DIMOWA, and if you had some mods installed when you unistalled DIMOWA, they will be preserved when you install it again 

### PSAQ (Probabily Some Asked Questions)

#### Why some files keep poping in and out of existance when I'm installing/unistalling a mod?

That happens because when the mod manager uninstall a mod it deletes the ddl from `Managed` (it doesn't do the same to the ones on the `mods` folder), and when it installs one, it copies it from the `mods` folder to `Managed`.


#### What mods are compatible with it?

* [OWBA](https://github.com/ShoosGun/OWBA) (DIMOWA 1.0.2 only)
* [Free Cam Mod](https://github.com/ShoosGun/FreeCamMod)
* [Probe Graple Mod](https://github.com/ShoosGun/ProbeGrapleMod) (DIMOWA 1.0.2 only)

## Credits
  - Thanks [ioncodes](https://github.com/ioncodes) for creating [dnpatch](https://github.com/ioncodes/dnpatch)
  - Thanks [Raicuparta](https://github.com/Raicuparta), [Mister_Nebula](https://github.com/misternebula), [AmazingAlek](https://github.com/amazingalek) and [TAImatem](https://github.com/TAImatem) for creating [OWML](https://github.com/amazingalek/owml) which inspired me to create my own for the Alpha
  - And thanks [Andreas Pardeike](https://github.com/pardeike/) for creating [Harmony](https://github.com/pardeike/Harmony) and [0x0ade](https://github.com/0x0ade) for making a .net 2.0 compatible version of it
  

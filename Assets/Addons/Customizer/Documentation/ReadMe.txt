Thanks for purchase Customizer.

Version 1.5.3

For full documentation please unzip the "documentation" file out of unity project for avoid errors.
or see the online documentation:
http://lovattostudio.com/documentations/customizer/

MFPS:---------------------------------------------------------------------------------------------

For use with MFPS:

After import the package go to (Toolbar)MFPS -> Addons -> Customizer -> Enable
Then add the 'Customizer' scene to the Build Settings, the scene is located in: Assets -> Addons -> Customizer -> Content -> Scene -> *
Use the example player prefab which have the rifle integrated with customizer for test the system, the prefab is located in the 'Resources' folder of the addon.

Add a new weapon:

- First you need create the weapon info in 'CustomizerData' which is located in the 'Resources' folder of Customizer addon, add a new field in the list "Weapons" 
and add all the attachments and camos that you want for that weapon.

- Then open the player prefab, and first, select the FPWeapon -> add the bl_CustomizerWeapon.cs, select the weapon name that you previously setup in the CustomizerData and
 click in the Button 'Refresh', after this the attachments list will automatically generated with the required attachments, so here you need assign the attachments models,
 (the models need be positioned inside of the weapon not from prefabs), in 'CamoRender' drag the Mesh of the weapon and assign the ID of the material (from the materials list of the mesh) that will change in case of change of camo.
  check 'isFirstPersonWeapon'.

- Now select the TPWeapon (the bl_NetworkGun in the player model) and add again the script bl_CustomizerWeapon.cs where bl_NetworkGun is, and again select the weapon name and click Refresh button,
   add the attachments models and the camo mesh, now make sure to UNCHECK 'isFirstPersonWeapon' and 'ApplyOnStart' toggles.

- if you have troubles, use the example player prefab that comes in the addon Resources folder, check the First Person and Third Person Rifle weapon for check how it's setup.

Make attachment affect the gun:
 - For example silencers, when a silencer is attached the weapon must have a different fire sound than the normal, for change the sound when a silencer is attached simply
   add the script bl_AttachmentGunModifier.cs to the Silencer object of the FPWeapon, in the script mark "Override Fire Sound" and add your Audio Clip,
   do the same for 'Sights' in order to change the Aim Position of the weapon and for Magazines in order to add more bullets.

   1.5.3
   - Add attachment gun modifier, allow change properties of the weapon like: Aim Position, Fire Sound and Bullets when the attachment is active.

Any problem or question please contact us.
http://www.lovattostudio.com/en/select-support/
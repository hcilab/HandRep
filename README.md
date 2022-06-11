# HandRep
 (VR) Game and Tasks themed around making a potion for testing different Hand Representations in VR

Note: The current setup requires the use of Valve Index Controllers. However, if you have 3d models of other VR controllers (which are easy to get. Valve VR packages have them I think) you can then assign the appropriate materials to different buttons and the controller itself (look at the index controller prefabs I made as reference) then map the appropriate openXR control profiles, then they should just work. There is code to swap between the controllers for this reason (Index, Vive, Oculus etc) but it is easy to expand. SubNote: Some bug patching for controller models may currently be targeted for just Index in how it behaves with interacting.

Any VR headset should work with this.

While there are defaults setup for use in the editor, there are also commandline variables for running the build.

-seated true/false : Whether or not the player is seated.
-pid PXXX : The participant ID number.
-order 0/1/2/3/4/5 the order of the current three hand representations (Sphere/Index/Hand)

This uses a slightly out of date version of OpenXR/XR Interaction Toolkit (just before dual-hand interactions on an object were introduced).
XR Interaction Toolkit: Version 1.0.0-pre.8 - October 27, 2021

Due to this, upgrading may break a few things as a few beta methods were used and have slightly changed since. It's possible but just takes effort to redo some code.


### Scenes

#### Intro Scene 

The introduction scene allows you to get used to each hand type before starting the tasks. Mostly to get used to VR, pick up items on a table, and also move using the joysticks on the controllers if the user wants to (instructions are on the black carpet on the floor, which is also the centre of the VR room and the VR origin/starting point)

In this introduction scene, press Z to go to the next hand type (Initializes on one, goes to next, then next. Stops). Then press N to go to the first Task Scene.

Alternatively, you can press K to skip to the Game scene.

#### Tasks

There are 12 tasks and each have their own scene and data managers.

Currently it's setup to perform a task 5 times per hand type, then a brief pause for the next hand type, then the third. Then you can go to the next task.

Note: The task scenes are 0 indexed in the project, so Task 1 is scene T0.

To reset a data point: Press R on keyboard.
To reset the whole scene: Press P on keyboard.

- Task1: Grab & Drop
- Task2: Grab & Place
- Task3: Grab & Pull Down
- Task4: Grab & Rotate (Fixed Position)
- Task5: Grab & Drop | Grab & Open
- Task6: Grab & Look Around Hand & Press A Button (Colour Cube) (Not Timed, Random)
- Task7: Grab & Drop | Grab & Push/Pull Forward/Back
- Task8: Grab & Place | Grab & Rotate (Around Posiition/Point)
- Task9: Bimanual. Grab & Attach Two Objects
- Task10: Bimanual. Grab & Separate Two Objects
- Task11: Grab & Pour (Precision)
- Task12: Grab & Scoop Bottle

#### Game

The game scene will require the user to make one potion with each hand type. The recipes and tiers of potions are configurable in the Resources folder in a json file. `potion_recipes.json`

To reset a recipe/data point: Press R on keyboard.
Bug - do not press P to reset the scene. For some reason the Colour Cube Spawner does not infinitely spawn if the scene is reloaded. Instead, close out of the game, reload and press K in the introduction scene.

Each recipe will require:
1x Temperature Change
1x Enchanted Item (May ask for by weight)
1x Ground Item (May ask for by weight)
1x Colour Cube
3.00-3.60ml of a Liquid
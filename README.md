# HandRep
 (VR) Game and Tasks themed around making a potion for testing different Hand Representations in VR

Note: The current setup requires the use of Valve Index Controllers. However, if you have 3d models of other VR controllers (which are easy to get. Valve's VR packages have them I think) you can then assign the appropriate materials to different buttons and the controller itself (look at the index controller prefabs I made as reference) then map the appropriate OpenXR control profiles, then they should just work. There is code to swap between the controllers for this reason (Index, Vive, Oculus etc) but it is easy to expand. SubNote: Some bug patching for controller models may currently be targeted for just Index in how it behaves with interacting.

For other controller models, you may need to edit them in a 3d modelling program like Blender in order to separate the buttons from the body of the controller.

Any VR headset should work with this.

While there are defaults setup for use in the editor, there are also commandline variables for running the build.


-seated true/false : Whether or not the player is seated.

-pid PXXX : The participant ID number.

-order 0/1/2/3/4/5 the order of the current three hand representations (Sphere/Index/Hand)


Example run once built: /HandRep.exe -seated false -pid P001 -order 0

This uses a slightly out of date version of OpenXR/XR Interaction Toolkit (just before dual-hand interactions on an object were introduced).
XR Interaction Toolkit: Version 1.0.0-pre.8 - October 27, 2021

Due to this, upgrading may break a few things as a few beta methods were used and have slightly changed since. It's possible but just takes effort to redo some code.


### Scenes

#### Intro Scene 

The introduction scene allows you to get used to each hand type before starting the tasks. Mostly to get used to VR, pick up items on a table, and also move using the joysticks on the controllers if the user wants to (instructions are on the black carpet on the floor, which is also the centre of the VR room and the VR origin/starting point)

In this introduction scene, press Z to go to the next hand type (Initializes on one, goes to next, then next. Stops). Then press N to go to the first Task Scene.

Alternatively, you can press K to skip to the Game scene.

#### Tasks

There are 12 tasks and each have their own scene and data managers. Each task's trials can be started by hitting a bell. For some (T4 and T11) a bell also has to be hit to submit it.

Currently it's setup to perform a task 5 times per hand type, then a brief pause for the next hand type, then the third. Then you can go to the next task by hitting the bell.

For most tasks, the placement of objects can be altered to be more comfortable for left or right handed participants by hitting Q and W.

Note: The task scenes are 0 indexed in the project, so Task 1 is scene T0.

To reset a data point: Press R on keyboard.
To reset the whole scene: Press P on keyboard.
To start each task: Hit the bell

- Task1: Grab & Drop - Grab mushroom, drop in green box.
- Task2: Grab & Place - Grab mushroom, place in weighing scale, timer stops, take it out of scale.
- Task3: Grab & Pull Down - Grab Lever and pull all the way down.
- Task4: Grab & Rotate (Fixed Position) - Rotate handle within the numerical range (inclusive), let go of handle, hit bell to submit.
- Task5: Grab & Drop | Grab & Open - Grab mushroom, Grab chest lid and open it (may be buggy depending how you do it), mushroom goes into chest, have chest lid close either manually or by gravity
- Task6: Grab & Look Around Hand & Press A Button (Colour Cube) (Not Timed, Random) - Get colour prompt, grab cube, hit A button on same hand holding cube, look for colour changing side. Once match, drop in green box.
- Task7: Grab & Drop | Grab & Push/Pull Forward/Back - Grab mushroom, pull dish handle in the fire out like a drawer, place mushroom in middle of dish, push all the way forward.
- Task8: Grab & Place | Grab & Rotate (Around Posiition/Point) - Place mushroom in stone bowl, grab handle, rotate *clockwise* 10 times.
- Task9: Bimanual. Grab & Attach Two Objects - Grab bottle, Grab cork with other hand, place cork in top of bottle.
- Task10: Bimanual. Grab & Separate Two Objects - Grab bottle, with other hand pull out cork.
- Task11: Grab & Pour (Precision) - Grab bottle, pour into cauldron (it's very fast, so many small pours is a strategy), cauldron will display contents once some is in. Goal is exactly 5.00, but it pours in increments of 0.10. Do not be below 5.00, but it's okay to be above until it cuts you off at 6.00. Once satisfied, stop pouring and hit bell on shelf to submit. Note: If you let go of bottle early, it disappears and needs trial reset.
- Task12: Grab & Scoop Bottle - Grab bottle, scoop it into cauldron, it fills up instantly.

#### Game

The game scene will require the user to make one potion with each hand type. The recipes and tiers of potions are configurable in the Resources folder in a json file. `potion_recipes.json`

To reset a recipe/data point: Press R on keyboard.
Bug - do not press P to reset the game scene. For some reason the Colour Cube Spawner does not infinitely spawn if the scene is reloaded. Instead, close out of the game, reload and press K in the introduction scene.

Each recipe will require:
1x Temperature Change
1x Enchanted Item (May ask for by weight)
1x Ground Item (May ask for by weight)
1x Colour Cube
3.00-3.60ml of a Liquid


### Data Collected

This experiment had multiple sources of data collection: A pre-survey for background information and a player traits survey from QuanticLabs, 3 In-between Hand types per task verbal questions, a post-all tasks questionnaire, and an in-between hand type in game questionnaires. However, this system did collect some raw data.

For each task and the game, there are raw files collected for every time any interactor or interactable had an event with appropriate info for details, as well as events pertaining to the task or game.

Additionally, these are summarized into summary csv's which provide only be essential data which is usually the time and sometimes supplemental data regarding the task. This is different for each task, and for each task and the game there are scripts that act as logging managers for this data which all share a parent script for base details.

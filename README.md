# OdysseyEditor
This is an experimental level editor for super mario odyssey.
At the time of writing there is no public way to load patched files, thus it's unknown if the levels made with this editor actually work in-game.

## What's done
  - Load levels, converting models to obj
  - Main editor features already implemented (Link nodes, Clipboard, undo, find)
  - Byml viewing and converting to and from Json (beta)

## What's still missing
  - A native/GPU-accelerated 3D renderer, the current one uses WPF and is kinda laggy
  - Area models are not in the game data, most of them are just replaced by a box *that doesn't reflect the actual area volume*, with some guess work they could be drawn manually to make areas correctly show
  - Undo is not fully implemented
  - Editing multiple objects at the same time
  - Probably other stuff

## Controls
There are two camera modes, choose the one you like from the settings.

Hotkey | action
|---|---|
Space | Move camera to selected object
Ctrl + drag object | Drag an object in the 3d view
Alt while dragging | Snap the object every 100 units
\+ | Add a new object (untested in-game)
D | Duplicate selection
Del | Delete selection
H | Hide selection from view
C | Edit the links of the selected object
B (while editing a links list) | Go back to the previous list

## Credits
This editor contains code or libraries from:
- [KillzXGaming's BFRES C# code ](https://github.com/KillzXGaming/Smash-Forge)
- [gdkchan's BnTxx ](https://github.com/gdkchan/BnTxx)
- [Gericom's EveryFileExplorer](https://github.com/Gericom/EveryFileExplorer)
- [masterf0x's RedCarpet](https://github.com/masterf0x/RedCarpet)
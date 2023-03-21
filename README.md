# Tiler

## Overview
`Tiler` is a set of tools to place and deform meshes along a spline or line.
By adding a spline with `SplineTilerComponent` (`GameObject/Tiler/Spline`) and selecting `Layouter`,
you can bend selected meshes along the spline.

# Implementation
It's done by 3 main interfaces:
`ITileLayouter` selects 'tiles' to place, `ITileInstantiator` creates GameObjects on scene, `ITileDeformer` applies transforms & deformation to meshes.

As an example, `SplineTiler` is using that to place and deform meshes along spline.
`SplineTilerComponent` and `SplineTilerComponentEditor` are using that to bake meshes in scene or prefab.


## ITileLayouter

Basically, it roles is to result a list of information about tile placement along some line.
So, you are feeding layouter with info like "I have a line/spline starting from position 0 to 10", 
and depending on the actual layouter used, you can get in return layouts like
- 1 tile, placed from position 0 to 10
- 10 tiles, next to each other, each with 1 length

Implementations of `ITileLayouter` can use composite pattern, and have multiple sublayouters.

Additionaly, we have `ScriptableTileLayouter` which serve as proxy between configuration in ScriptableObject and "proper" `ITileLayouter`.

### Example
`ScriptableFillLayouter` is a SO, which has "Start, Tiles, End" parameters.
It will place random element from Start at the beginning of the spline, random from End at the end.
And will fill space between them using `Fill` elements, building layout like this:
`Start-Fill-Fill-Fill-End`
You can assign `ScriptableFillLayouter` as an layouter for `SplineTilerComponent`,
and use it to draw a spline mesh with specific beginning and end.
Underneath, it will use `CompositeFillLayouter` with prefix, postfix and fill sublayouters to achieve that.

## ITileInstantiator

This interface is used to create components in scene/prefab, and bake meshes.
It's role is to:
- create gameobjects and components for tiles
- delete unneeded gameobjects (for example, when your spline had 10 children, but after changes it should have 9)
- bake meshes to disk, and remove unneeded meshes

Unfortunately, it's probably most error-prone part, as integration with Unity isn't always clear.
See `Baking Issues` part.

## ITileDeformer

This interface is getting Tile to deform, Spline data, and GameObject in which we should keep results.
Tile data (privided by `ITileLayouter`) tells which source mesh we should use, and where on the spline it should be placed.


## Baking Issues

How I would imagine ideal baking process:
- when needed (when edition of source data is complete), mesh is baked and results are kept on disk, correlated with GameObject which requested bake.
- when GO which requested bake is deleted, baked meshes also should be removed (TODO!!!)
- small changes to source data should bake mesh to memory, to show results quickly. After edition is complete, results should be saved to disk.

Issues:
- correlation gameobject with data on disk (current solution is to use `GlobalObjectId` to get unique ID for gameobject requesting bake)
- detection that gameobject will be removed (current solution - implement OnDestroy in ExecuteAlways script, inform editor which schedules deletion of baked data)
- detection when changes to the source data are completed (maybe additional event in source Spline would suffice, for now sometimes user needs to manually press "Bake")
- user can potentially create gameobject in scene, bake mash to disk, then close unity - original gameobject would vanish, but baked meshes would stay - only simple solution for that that I see is `Rebake All` button for scene that would delete & rebake all meshes on scene/prefab)
- editing prefabs - if prefab scene is open, we add just instantiate objects, otherwise we need to use `EditPrefabContentsScope` (reliability of that is bit spotty)

## Potential improvements

- better spline/spline editor (https://github.com/vvrvvd/Unity-Spline-Editor)
# Projectors

## Details
This architecture is responsible for rendering multiple views to a coherent stereoscopic output. The `ProjectorBrain` starts the initialization of the `ProjectorCamera` and `ProjectorEyes` components with the help of the `ProjectorMount` which holds all `ProjectorCameras` for one `ProjectorBrain` hierarchy. All options for the `ProjectorBrain` are located in one `ProjectorSettings` asset.
Every `ProjectorCamera` has a `ProjectorConfiguration` which defines custom and unique properties for the camera and rendering process.
Also a `ProjectorCamera` has a reference to a `ProjectorPlane` that represents the render surface in real space.

## Rendering
First of all the `ProjectorBrain` assigns a display output to each `ProjectorCamera`.
Before a new image gets rendered by a `ProjectorCamera` a new projection matrix will be computed from the position of the `ProjectorEyes` and the vertices of the `ProjectorPlane`.
The resulting so called [Holographic Off-Axis Projection](https://en.wikibooks.org/wiki/Cg_Programming/Unity/Projection_for_Virtual_Reality) should give a seamless transition between the different `ProjectorCamera` images. The projection matrix supports mono and stereoscopic rendering. Notice that stereoscopic rendering is disabled in the editor.

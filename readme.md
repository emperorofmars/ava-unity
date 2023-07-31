# AVA Format
## A Set of Extensions for the STF Format to Represent VR & V-Tubing Avatars

## **This is a proof of concept!**

A set of hot-loadable extensions for the STF format in Unity to represent the various components that make up Avatars for Social-VR and V-Tubing applications.
This includes 3 second stage loaders for VRChat, VRM and ChilloutVR.

If you have a STF file with AVA components correctly set up, you can just import an avatar and spawn a ready to upload VRChat avatar.

It will give you the option to spawn it only if you have the appropriate SDK included.

## Setup
- It will not work standalone, please import the STF Unity implementation first: [https://github.com/emperorofmars/stf-unity](https://github.com/emperorofmars/stf-unity)
- Download or clone the repository and copy the entire folder into your Unity projects 'Assets' folder.
- Ensure that you have either a current VRChat, ChilloutVR SDK or the UniVRM SDK in the version 0.89.0 (The same version that is required for the VSeeFace SDK).

---

Most components are only partly implemented without too much thought.

This exists only to show that this is possible and realistic.


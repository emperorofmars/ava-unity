# AVA Format
**A Set of Extensions for the [STF Format](https://github.com/emperorofmars/stf-unity) to Represent VR & V-Tubing Avatars**

**This is a proof of concept!**

## [Watch the video presentation here!]()

## Table of Content
- [General Description](#general-description)
- [How to Use](#how-to-use)
- [AVA Format](#ava-format)

## General Description

The AVA project is a set of hot-loadable extensions for the [STF format](https://github.com/emperorofmars/stf-unity) in Unity to represent the various components that make up Avatars for Social-VR and V-Tubing applications.
It contains a set of converters for specific targets like VRChat, VRM and ChilloutVR.

I used this project to test and develop the capabilities of the [STF](https://github.com/emperorofmars/stf-unity) format.

This is very experimental, i tried various things for each component. As such using this will be a very inconsistent and half broken experience.

Most components are only partly implemented without too much thought.

For now, this exists only to show that this is possible and realistic.

## How to Use
- Ensure you have the Newtonsoft JSON package imported in Unity. If you set up your Unity project with the VRC Creator Companion, it will be already imported. If not, install the official package in UPM.
- Either:
	- Download the latest release, which contains this set of extensions and the base STF implementation, and import the .unitypackage into Unity.
	- Or set up the STF Unity implementation first: [https://github.com/emperorofmars/stf-unity](https://github.com/emperorofmars/stf-unity)
	Then download or clone this repository and copy the entire folder into your Unity projects 'Assets' folder.
- Ensure that you have either a current VRChat SDK, ChilloutVR SDK or the UniVRM SDK in the version 0.89.0 (The same version that is required for the VSeeFace SDK).
- If you have an STF file with AVA components correctly set up, you can just import it. Its UI will give you the option to spawn a ready to upload VRChat, ChilloutVR or VRM avatar. It will give you the option to spawn it only if you have the appropriate SDK included.

![Screenshot of an STF file's inspector in Unity with this project's extension included.](./doc/img/import_settings.png)

## AVA Format
Most functionality of avatars in applications like VRChat, ChilloutVR or the VRM format is either nearly identical or very similar. Some of these applications may have features that another doesn't, but generally they work nearly the same.

---

Cheers!


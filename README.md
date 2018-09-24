Couchbase Lite for Unity
==================

Couchbase Lite is a lightweight embedded NoSQL database that has built-in sync to larger backend structures, such as Couchbase Server.

This is an unsupported and undocumented port of the .Net libraries at [https://github.com/couchbase/couchbase-lite-net](https://github.com/couchbase/couchbase-lite-net)

## Unity Package
The most recent Unity Package can be downloaded from the repository [here](https://github.com/virtualplaynl/couchbase-lite-unity/raw/master/Couchbase-Lite-Unity.unitypackage)

## Documentation Overview

For usage in Unity, look at the test scene.
This is the documentation for the regular Couchbase libraries:

* [Official Documentation](https://developer.couchbase.com/documentation/mobile/2.0/guides/couchbase-lite/index.html)
* [Getting Started - C#](https://docs.couchbase.com/couchbase-lite/2.1/csharp.html)

## Status

Couchbase Lite for Unity is currently using version 2.1.0 of the official Couchbase Lite native libraries (from NuGet).
The support libraries are built from the solution in the `Libraries` folder. They are also included in the UnityPackage.
Tested in Unity 2018.2.5 and 2018.2.8, but earlier versions should also work.

* All platforms: shutting down correctly is not included in the example! Cleanup of Replicator needs (your own) implementation

* Android: working, tested only on ARMv7 device
* iOS: working on devices, simulator untested
* Windows: working for x68 and x64, UWP not added yet
* macOS: working
* Linux: added but untested

**Warning: No guarantees are given on the workings of these libraries. Virtual Play doesn't accept any form of liability when using these libraries.**

## Caveat

* Because of the static way sockets are initialized in the Couchbase libraries, replication in the Unity Editor will only work once, after which another attempt will crash the Editor. Therefore, don't test replication in the Editor (unless you want to start your Editor again after the test).

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg?style=plastic)](https://opensource.org/licenses/Apache-2.0)

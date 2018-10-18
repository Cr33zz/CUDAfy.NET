# CUDAfy.NET
CUDAfy .NET allows easy development of high performance GPGPU applications completely from the Microsoft .NET framework. It's developed in C#.

This is original CUDAfy.NET version 1.29 modified to work with CUDA 10.0 and Visual Studio 2017 compiler.

Orignal project website https://archive.codeplex.com/?p=cudafy states that last update was in April 2015. Last supported CUDA version was 7.0. The biggest issue with that is that CUDA 7 nvcc (NVidia CUDA compiler) supports only vs2010, vs2012, and vs2013. Since I wanted to make it work for anyone using vs2017 without installing old VS toolchains, I had to make CUDAfy work with newer CUDA.

I'm **not** the author of CUDAfy.NET, just a fan :)

Originally CUDAfy was reading bunch of registry values to figure out CUDA install directory and cl.exe for Visual Studio 2010 and 2012; I decided to create a separate registry values for these directories, since things has changed over the years (ie. CUDA no longer puts its install dir into registry value) and I wanted to make it work with the little effort. I hope one day I will revisit that and make it more user friendly. Until then you have to set following registry values to make CUDAfy.NET work:

```
[HKEY_CURRENT_USER\Software\CUDAfy.NET]
"CUDAInstallDir"="Directory containing CUDA 10.0 installation"
"CompilerDir"="Directory containing cl.exe"
```

**Modify** and execute SetupInstallPaths.reg to set everything up.

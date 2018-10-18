using Microsoft.Win32;
using System;
using System.IO;

namespace Cudafy
{
    /// <summary>This utility class resolves path to nVidia's nvcc.exe and Microsoft's cl.exe.</summary>
    internal static class NvccExe
    {
        /// <summary>Get GPU Computing Toolkit 10.0 installation path.</summary>
        /// <remarks>Throws an exception if it's not installed.</remarks>
        static string getToolkitBaseDir()
        {
            // http://stackoverflow.com/a/13232372/126995
            RegistryKey localKey;
            if( Environment.Is64BitProcess || !Environment.Is64BitOperatingSystem )
                localKey = Registry.LocalMachine;
            else
                localKey = RegistryKey.OpenBaseKey( RegistryHive.LocalMachine, RegistryView.Registry64 );

            RegistryKey registryKey = localKey.OpenSubKey( @"SOFTWARE\NVIDIA Corporation\GPU Computing Toolkit\CUDA\v10.0", false );
            if( null == registryKey )
                throw new CudafyCompileException( "nVidia GPU Toolkit error: version 10.0 is not installed." );

            RegistryKey userKey;
            if (Environment.Is64BitProcess || !Environment.Is64BitOperatingSystem)
                userKey = Registry.CurrentUser;
            else
                userKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);

            registryKey = userKey.OpenSubKey(csCUDAfyRegKey, false);
            string res = registryKey.GetValue("CUDAInstallDir") as string;
            if( null == res )
                throw new CudafyCompileException( "nVidia GPU Toolkit error: unspecified installation directory in CUDAInstallDir in registry key [HKEY_CURRENT_USER\\Software\\CUDAfy.NET]" );
            if( !Directory.Exists( res ) )
                throw new CudafyCompileException( "nVidia GPU Toolkit error: the installation directory \"" + res + "\" doesn't exist" );
            return res;
        }

        static readonly string toolkitBaseDir = getToolkitBaseDir();

        const string csNVCC = "nvcc.exe";
        const string csCUDAfyRegKey = "Software\\CUDAfy.NET";

        /// <summary>Path to the nVidia's toolkit bin folder where nvcc.exe is located.</summary>
        public static string getCompilerPath()
        {
            return Path.Combine( toolkitBaseDir, "bin", csNVCC );
        }

        /// <summary>Path to the nVidia's toolkit's include folder.</summary>
        public static string getIncludePath()
        {
            return Path.Combine( toolkitBaseDir, @"include" );
        }

        /// <summary>Path to the Microsoft's visual studio folder where cl.exe is localed.</summary>
        public static string getClExeDirectory()
        {
            RegistryKey userKey;
            if (Environment.Is64BitProcess || !Environment.Is64BitOperatingSystem)
                userKey = Registry.CurrentUser;
            else
                userKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);

            RegistryKey registryKey = userKey.OpenSubKey(csCUDAfyRegKey, false);
            string clDir = registryKey.GetValue("CompilerDir") as string;

            if( !Directory.Exists(clDir) )
                throw new CudafyCompileException("nVidia GPU Toolkit error: Visual Studio compiler directory \"" + clDir + "\" doesn't exist" );

            string clPath = Path.Combine( clDir, "cl.exe" );
            if( File.Exists( clPath ) )
                return clDir;
            
            throw new CudafyCompileException( "nVidia GPU Toolkit error: cl.exe was not found" );
        }
    }
}
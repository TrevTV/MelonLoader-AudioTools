using MelonLoader;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace AudioImportLib
{
    public static class API
    {
        internal static DecoderImporter importer;
        internal static IntPtr bassLibrary;
        internal static bool hasLoadedLib;
        internal static string bassLibName;
        internal static bool isRunningAndroid;

        internal static void SetupImporter()
        {
            isRunningAndroid = MelonUtils.CurrentPlatform == (MelonPlatformAttribute.CompatiblePlatforms)3;

            if (!hasLoadedLib && !isRunningAndroid)
            {
                string appDataPath = Path.Combine(MelonUtils.UserDataDirectory, "AudioImportLib");
                string bassDllPath = Path.Combine(appDataPath, "bass.dll");

                if (!Directory.Exists(appDataPath))
                    Directory.CreateDirectory(appDataPath);

                if (!File.Exists(bassDllPath))
                {
                    using (Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("AudioImportLib.bass.dll"))
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        str.CopyTo(memoryStream);
                        File.WriteAllBytes(bassDllPath, memoryStream.ToArray());
                    }
                }

                MelonLogger.Msg("Loading BASS from " + bassDllPath);
                bassLibrary = DllTools.LoadLibrary(bassDllPath);
                hasLoadedLib = true;
            }

            if (importer == null)
                importer = isRunningAndroid ? new CSCoreImporter() : new BassImporter();
        }

        public static AudioClip LoadAudioClip(string absolutePathToFile, bool dontUnloadUnusedAsset = true)
        {
            if (!File.Exists(absolutePathToFile))
            {
                MelonLogger.Error($"A mod is asking to load \"{absolutePathToFile}\" which does not exist!");
                return null;
            }

            SetupImporter();
            importer.Import(absolutePathToFile);
            if (importer.audioClip == null)
            {
                MelonLogger.Error($"Failed to load audio clip from \"{absolutePathToFile}\"!");
                return null;
            }

            if (dontUnloadUnusedAsset)
                importer.audioClip.hideFlags = HideFlags.DontUnloadUnusedAsset;

            return importer.audioClip;
        }
    }
}

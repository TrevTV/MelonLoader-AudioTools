using MelonLoader;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AudioImportLib
{
    public static class API
    {
        internal static BassImporter bassImporter;
        internal static IntPtr bassLibrary;
        internal static bool hasLoadedLib;

        internal static void SetupBASSImporter()
        {
            if (!hasLoadedLib)
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

                MelonLogger.Msg(bassDllPath);
                bassLibrary = DllTools.LoadLibrary(bassDllPath);
                hasLoadedLib = true;
            }

            if (bassImporter == null)
                bassImporter = new BassImporter();
        }

        public static AudioClip LoadAudioClip(string absolutePathToFile, bool dontUnloadUnusedAsset = true)
        {
            if (!File.Exists(absolutePathToFile))
            {
                MelonLogger.Error($"A mod is asking to load \"{absolutePathToFile}\" which does not exist!");
                return null;
            }

            SetupBASSImporter();
            bassImporter.Import(absolutePathToFile);
            if (bassImporter.audioClip == null)
            {
                MelonLogger.Error($"Failed to load audio clip from \"{absolutePathToFile}\"!");
                return null;
            }

            if (dontUnloadUnusedAsset)
                bassImporter.audioClip.hideFlags = HideFlags.DontUnloadUnusedAsset;

            return bassImporter.audioClip;
        }
    }
}

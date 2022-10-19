using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace AudioReplacer
{
    public static class BuildInfo
    {
        public const string Name = "AudioReplacer";
        public const string Author = "trev";
        public const string Company = null;
        public const string Version = "1.3.0";
        public const string DownloadLink = null;
    }

    public class Core : MelonMod
    {
        public static Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();
        public static bool LogSounds;

        private static bool overrideEnabled;
        private readonly string customAudioPath = Path.Combine(MelonUtils.UserDataDirectory, "CustomAudio");
        private readonly string[] allowedExts = new string[] { ".wav", ".mp3" };

        public override void OnInitializeMelon()
        {
            if (!Directory.Exists(customAudioPath))
                Directory.CreateDirectory(customAudioPath);

            var category = MelonPreferences.CreateCategory("AudioReplacer", "");
            category.CreateEntry("LogSounds", false);
            LogSounds = category.GetEntry<bool>("LogSounds").Value;
            MelonPreferences.Save();

            string[] audioFiles = Directory.GetFiles(customAudioPath, "*", SearchOption.AllDirectories);
            var ovrClip = audioFiles.SingleOrDefault(f => Path.GetFileNameWithoutExtension(f) == "REPLACE_ALL");
            if (ovrClip == null)
            {
                foreach (string file in audioFiles)
                {
                    if (!allowedExts.Contains(Path.GetExtension(file)))
                        continue;

                    AudioClip clip = AudioImportLib.API.LoadAudioClip(file);
                    AudioClips.Add(clip.name, clip);
                    LoggerInstance.Msg("Added: " + clip.name);
                }
            }
            else
            {
                AudioClip clip = AudioImportLib.API.LoadAudioClip(ovrClip);
                AudioClips.Add(clip.name, clip);
                overrideEnabled = true;
                LoggerInstance.Msg("Added override: " + clip.name);
            }

            HarmonyInstance.Patch(AccessTools.Method(typeof(AudioSource), "Play", new Type[0]), new HarmonyMethod(typeof(Core).GetMethod("AudioPlayPatch")));
            HarmonyInstance.Patch(AccessTools.Method(typeof(AudioSource), "Play", new Type[1] { typeof(ulong) }), new HarmonyMethod(typeof(Core).GetMethod("AudioPlayPatch")));
        }

        public static void AudioPlayPatch(AudioSource __instance)
        {
            if (__instance.clip == null)
                return;

            if (LogSounds)
                MelonLogger.Msg($"Playing \"{__instance.clip.name}\" from object \"{__instance.gameObject.name}\"");

            string audioClipName = overrideEnabled ? "REPLACE_ALL" : __instance.clip.name;
            if (AudioClips.TryGetValue(audioClipName, out AudioClip replaceClip))
            {
                __instance.pitch = 1;
                __instance.clip = replaceClip;
            }
        }
    }
}

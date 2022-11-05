using MelonLoader;

namespace AudioImportLib
{
    public static class BuildInfo
    {
        public const string Name = "AudioImportLib";
        public const string Author = "trev & zCubed";
        public const string Company = null;
        public const string Version = "1.0.0";
        public const string DownloadLink = null;
    }

    public class Core : MelonMod
    {
        public override void OnApplicationQuit()
        {
            if (API.hasLoadedLib && !API.isRunningAndroid)
                DllTools.FreeLibrary(API.bassLibrary);
        }
    }
}

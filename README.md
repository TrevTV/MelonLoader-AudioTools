# MelonLoader AudioTools (AudioReplacer and AudioImportLib)

## Audio Import Lib
A library to allow code modders to import common audio file formats into AudioClips that are playable in-game.

### API
There is only one method in the API.
`AudioImportLib.API.LoadAudioClip(string absolutePathToFile, bool dontUnloadUnusedAsset = true);`
The first parameter is the local path to the audio file.
The second parameter determines if the lib will automatically set the clip's hideflags to DontUnloadUnusedAsset to prevent Unity from collecting it.

### FAQ
- Why not use the built in Unity methods
  - This was made for BONEWORKS, which didn't have those methods
- Why not use a basic WAV or MP3 reader class instead of a native DLL
  - I did originally, which led to constant issues with files that weren't exactly how it liked

## Audio Replacer

### How To Use
Put your audio files of the same name of the clip you wanna replace in UserData/CustomAudio

If the folder doesn't exist, boot the game once or just make it yourself

### How To Find Audio Clip Names
In MelonPreferences.cfg change LogSounds to true

When in game, whenever an audioclip is played it will log it's name to the console

### Complete Override
I added some stupid thing where if you have an audio file named REPLACE_ALL, it will override every audioclip in the game with it.

## Credits/Licensing
- [Hello-Meow/AudioImporter](https://github.com/Hello-Meow/AudioImporter), modified, under the [MIT License](https://github.com/Hello-Meow/AudioImporter/blob/master/LICENSE)
- [Un4Seen/BASS/BASS.NET](https://www.un4seen.com/), BASS.NET in decompiled form, BASS in compiled form, under a free for commercial use license. It doesn't have a direct link, but it's on the linked website.

# Convert Midi File to [LML](https://sightreading.training/guide/lml)

This converts midi files for use with [sightreading.training](https://sightreading.training).

Midi file is assumed to have 2 tracks with note data, one for right hand, and then one for left hand.


### How to use

* If you have dotnet script installed, just drop any midi file on `DropFileToConvert.bat` (you only need the files inside folder `MidiToLML`)
* For compile or use with msvc, run `msvc create project.bat` to create project files, then drop midi file on `msvc run on dropped file.bat`
* For use in Unity editor, use MidiToLMLUnity.cs


### Midi Parser
[Midi parser by David Gouveia](https://github.com/davidluzgouveia/midi-parser)

### **NOTE: MORE TESTING NEEDED, ALPHA STAGE!**

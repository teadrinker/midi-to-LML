
// Martin Eklund, MIT License
// https://github.com/teadrinker/midi-to-lml

using System.Collections.Generic;


// Midi -> LML (https://sightreading.training/guide/lml)

public static class MidiToLML
{

    private static string MidiNoteToText(int n /*, int keysign*/)
    {
        // this probably need to change depending on key signature
        var key = n % 12;
        var oct = n / 12;
        if (key == 0) return "c" + oct;
        if (key == 1) return "d-" + oct;
        if (key == 2) return "d" + oct;
        if (key == 3) return "e-" + oct;
        if (key == 4) return "e" + oct;
        if (key == 5) return "f" + oct;
        if (key == 6) return "g-" + oct;
        if (key == 7) return "g" + oct;
        if (key == 8) return "a-" + oct;
        if (key == 9) return "a" + oct;
        if (key == 10) return "b-" + oct;
        if (key == 11) return "b" + oct;
        throw new System.Exception("MidiNoteToText() should never reach");
    }

    // quant_shift (Quantization):
    //    0 == 1/4   (each beat)
    //    1 == 1/8   (8th)
    //    2 == 1/16  (16th)
    //    3 == 1/32  (32th)
    //    etc 

    public static string Convert(string path, int transpose, int rightHandTranspose, int quant_shift, System.Action<string> logger)
    {
        var lml = new List<string>();
        for (int i = 0; i < quant_shift; i++)
            lml.Add("dt");

        var midiFile = new MidiFile(path);
        var ticksPerQuarterNote = midiFile.TicksPerQuarterNote;

        var song = new MidiSong(midiFile, logger);


        int n = song.trackIds.Count;

        for(int i = 0; i < n; i++)
		{
            lml.Add("t" + i);
            foreach (var e in song.events)
			{
                if(e.track == i && e.midi.MidiEventType == MidiEventType.NoteOn)
				{
                    var time = convertTime(e.midi.Time, ticksPerQuarterNote, quant_shift);
                    if (time > 0)
                        lml.Add(time == 1 ? "r" : "r" + time);

                    var duration = convertTime(e.eventDuration, ticksPerQuarterNote, quant_shift);
                    var transp = transpose + (i == 2 ? rightHandTranspose : 0);
                    lml.Add(MidiNoteToText(e.midi.Note + transp) + (duration <= 1 ? "" : "." + duration));
                        
                    lml.Add("|");
                } 
                else if(/*e.track == i&&  // can tracks have different time sign? prob not... */
                        e.midi.MidiEventType == MidiEventType.MetaEvent && e.midi.MetaEventType == MetaEventType.TimeSignature)
				{
                    var time = convertTime(e.midi.Time, ticksPerQuarterNote, quant_shift);
                    if (time > 0)
                        lml.Add(time == 1 ? "r" : "r" + time);

                    lml.Add("ts"+ (int)e.midi.Arg2 +"/"+ (int)e.midi.Arg3);

                    lml.Add("|");
				}
            }
		}


        return string.Join(' ', lml.ToArray());
    }
    private static int convertTime(int t, int ticksPerQuarterNote, int quant_shift)
	{
        var timeInQuarterNotes = ((double)t) / ticksPerQuarterNote;
        var shiftedTime = timeInQuarterNotes * (1 << quant_shift);
        return (int)(shiftedTime + 0.5); // round to avoid floating point values slightly lower than integers etc 
    }


} 


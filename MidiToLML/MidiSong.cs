
// Martin Eklund, MIT License
// https://github.com/teadrinker/midi-to-lml

using System.Collections.Generic;
using System.Linq;


public class MidiSong
{
    public MidiSong(MidiFile midifile, System.Action<string> logger_) { logger = logger_; ConvertToSong(midifile); }

    public class SongEvent
    {
        public int track;
        public MidiEvent midi;
        public int eventDuration = -1;


        public bool isSameNote(SongEvent e)
        {
            return track == e.track
                && midi.Channel == e.midi.Channel
                && midi.Note == e.midi.Note;

        }
    }
    public List<SongEvent> events = new List<SongEvent>();
    public List<string> log = new List<string>();
    public HashSet<int> trackIds = new HashSet<int>();
    public System.Action<string> logger;
    public void Log(string s)
    {
        if (logger != null)
            logger(s);
    }
    private void ConvertToSong(MidiFile midiFile)
    {
        Log("g");
        for (var trackId = 0; trackId < midiFile.Tracks.Length; trackId++)
        {
            var track = midiFile.Tracks[trackId];
            foreach (var midiEvent in track.MidiEvents)
            {
                trackIds.Add(trackId);
                var e = new SongEvent() { track = trackId, midi = midiEvent };
                if (midiEvent.MidiEventType == MidiEventType.NoteOn && midiEvent.Velocity == 0) // fix annoying case with vel == 0 
                    e.midi.Type = (byte)MidiEventType.NoteOff;
                events.Add(e);
            }
            /*
            foreach (var textEvent in track.TextEvents)
            {
                if (textEvent.TextEventType == TextEventType.Lyric)
                {
                    var time = textEvent.Time;
                    var text = textEvent.Value;
                }
            }
            */
        }

        events = events.OrderBy(e => e.midi.Time).ToList(); // we have to use stable sort not to mess up order of midi events with identical time stamp

        // calculate durations for NoteOns 
        var heldKeys = new List<SongEvent>();
        for (int i = 0; i < events.Count; i++)
        {
            var e = events[i];
            if (e.midi.MidiEventType == MidiEventType.NoteOn)
            {
                var held = FindSameNote(e, heldKeys);
                if (held == -1)
                    heldKeys.Add(e);
                else
                    Log("double note-on");
            }
            else if (e.midi.MidiEventType == MidiEventType.NoteOff)
            {
                var held = FindSameNote(e, heldKeys);
                if (held != -1)
                {
                    var heldKeyMatch = heldKeys[held];
                    heldKeys.RemoveAt(held);
                    heldKeyMatch.eventDuration = e.midi.Time - heldKeyMatch.midi.Time;
                    if (e.eventDuration <= 0)
                        Log("eventLength <= 0 : " + e.eventDuration);
                }
                else
                    Log("note-off without note-on");
            }
        }
    }
    int FindSameNote(SongEvent e, List<SongEvent> l)
    {
        for (int i = 0; i < l.Count; i++)
        {
            if (e.isSameNote(l[i]))
                return i;
        }
        return -1;
    }

}

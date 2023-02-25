#load "MidiFile.cs"
#load "MidiSong.cs"
#load "MidiToLML.cs"

if(Args.Count == 0)
{
    Console.WriteLine("Missing midi file path (pass as argument)");
    return;
}
Console.WriteLine("Begin "+Args[0]);

var LML = MidiToLML.Convert(Args[0], 0, 0, 2, s=> Console.WriteLine(s) );

Console.WriteLine(" ------------- ");
Console.WriteLine(LML);
Console.WriteLine(" ------------- ");

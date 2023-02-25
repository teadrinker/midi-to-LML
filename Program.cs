
// Use this if you want to build command line tool using visual studio

using System;


class Program
{
    static void Main(string[] args)
    {
        if(args.Length == 0)
        {
            Console.WriteLine("Missing midi file path (pass as argument)");
            return;
        }
        Console.WriteLine("Begin "+args[0]);
        
        var LML = MidiToLML.Convert(args[0], 0, 0, 2, s=> Console.WriteLine(s) );
        
        Console.WriteLine(" ------------- ");
        Console.WriteLine(LML);
        Console.WriteLine(" ------------- ");
    }
}


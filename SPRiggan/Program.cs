using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SPRiggan
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename;

            Console.WriteLine("SPRiggan");
            Console.WriteLine("A Ragnarok Online SPR file to PNG converter");
            Console.WriteLine("2016 - Delfioh - https://github.com/delfioh \n");

            if (args.Length > 0)
            {
                filename = args[0];
            }
            else
            {
                Console.WriteLine("No filename specified.");
                Console.WriteLine("Usage: SPRiggan filename");
                return;
            }

            if (File.Exists(filename))
            {
                ROSprite spr = new ROSprite(filename);
                spr.SavePNGFiles();
            }
            else
            {
                Console.WriteLine("Specified file does not exist");
                return;
            }
            
        }
    }
}

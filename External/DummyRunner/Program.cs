using System;
using System.Threading;

namespace RLToolkit.External
{
    class MainClass
    {
        public static int Main (string[] args)
        {
            int retCode = 1;

            Console.WriteLine ("Start");
            foreach (string s in args) {
                if (s.ToLower().StartsWith ("-delay:")) {
                    string[] split = s.Split (':');
                    int delay;
                    Int32.TryParse(split [1], out delay);

                    Console.WriteLine ("Delay: " + delay);
                    Thread.Sleep (delay*1000);
                    Console.WriteLine ("Delay done");
                }
                else if (s.ToLower() == "-error")
                {
                    retCode = 2;
                }
            }
            Console.WriteLine ("Finished");
            return retCode;
        }
    }
}

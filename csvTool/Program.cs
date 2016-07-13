using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSVTool
{
    class Program
    {
        static void Main(string[] args)
        {
            //string input = File.ReadAllText("export.csv");
            if(args.Length < 1)
            {
                Console.WriteLine("Usage: csvtool.exe [PATH TO DEFINITIONS] [PATH TO WORKING FILE] [PATH TO EXPORT FILE]");
                Environment.Exit(1);
            }
            try {
                File.ReadAllText(args[1]);
                }
            catch(FileNotFoundException)
            {
                Console.WriteLine("Error: Unable to open working file");
                Environment.Exit(1);
            }

            try
            {
                File.ReadAllText(args[0]);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: Unable to open definitions file");
                Environment.Exit(1);
            }
            string input = File.ReadAllText(args[1]);
            Console.WriteLine("Read {0} bytes.", input.Length);

            var definitions = File.ReadAllText(args[0]).Split(';').ToArray();
            
            Console.WriteLine("Read {0} definitions.", definitions.Length);
            for(int i = 0; i < definitions.Length; i++)
            {
                definitions[i] = definitions[i].Replace("?", "\\?");
                definitions[i] = definitions[i].Replace("/", "\\/");
                //Console.WriteLine(definitions[i]);
                if (definitions[i].Contains('*'))
                {
                    definitions[i] = definitions[i].Replace("*", ".*?");
                    definitions[i] = definitions[i].Replace("/", "\\/");
                    
                    //definitions[i] = "/" + definitions[i] + "/g";
                    definitions[i] = definitions[i].Replace(Environment.NewLine, null);
                    var regexmatches = Regex.Matches(input, definitions[i]);
                    Console.WriteLine("Definition {1}: Removed {0} Occurrences", regexmatches.Count, i+1);

                    input = Regex.Replace(input, definitions[i].Trim(), "");
                    Console.WriteLine(definitions[i].Trim());
                }
                else
                {

                    var matches = Regex.Matches(input, definitions[i].Trim());
                    Console.WriteLine("Definition {1}: Removed {0} Occurrences", matches.Count, i+1);
                    input = input.Replace(definitions[i].Trim(), null);
                    Console.WriteLine(definitions[i].Trim());
                }
                
            }
            File.WriteAllText(args[2], input);
            
            
         
            
        }
    }
}

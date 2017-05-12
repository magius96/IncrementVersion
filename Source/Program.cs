using System;
using System.Collections.Generic;
using System.IO;

namespace IncrementVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if(args.Length < 1)
                {
                    Console.WriteLine("Usage: IncrementVersion <filename> <type>");
                    Console.WriteLine("    <filename> should be the full path to the Assembly Information file.");
                    Console.WriteLine("    <type> should be either \"Debug\" or \"Release\" without the quotes.");
                    Console.WriteLine("    <type> is optional and defaults to \"Debug\".");
                    return;
                }
                var filename = args[0];
                var incrementType = "Debug";
                if (args.Length > 1)
                    incrementType = args[1];

                if (!File.Exists(filename))
                {
                    Console.WriteLine("Could not locate the Assembly Information file.");
                    return;
                }

                var reader = new StreamReader(filename);
                var linesOut = new List<string>();
                var versionType = "";
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("AssemblyVersion")) versionType = "AssemblyVersion";
                        if (line.Contains("AssemblyFileVersion")) versionType = "AssemblyFileVersion";
                        if ((line.Contains("AssemblyVersion") || line.Contains("AssemblyFileVersion")) &&
                            !line.Contains("//"))
                        {
                            var loc = line.IndexOf('"');
                            var sub1 = line.Substring(loc + 1);
                            loc = sub1.IndexOf('"');
                            var sub2 = sub1.Substring(0, loc);
                            var ver = sub2.Split('.');

                            var major = 0;
                            var minor = 0;
                            var build = 0;
                            var revision = 0;

                            try
                            {
                                major = Convert.ToInt32(ver[0]);
                                minor = Convert.ToInt32(ver[1]);
                                build = Convert.ToInt32(ver[2]);
                                revision = Convert.ToInt32(ver[3]);
                            }
                            catch
                            {
                                Console.WriteLine("Could not parse the version number. Reverting to 1.0.0.0");
                                major = 1;
                                minor = 0;
                                build = 0;
                                revision = 0;
                            }

                            if (incrementType == "Debug")
                                revision++;
                            else
                                minor++;

                            if (revision > 9)
                            {
                                build++;
                                revision -= 9;
                            }
                            if (build > 9)
                            {
                                minor++;
                                build -= 9;
                            }
                            if (minor > 9)
                            {
                                major++;
                                minor -= 9;
                            }

                            var lineout = line.Replace(sub2,
                                                       string.Format("{0}.{1}.{2}.{3}", major, minor, build, revision));
                            Console.WriteLine("Updating {0} from {1} to {2}", versionType, sub2,
                                              string.Format("{0}.{1}.{2}.{3}", major, minor, build, revision));
                            linesOut.Add(lineout);
                        }
                        else
                        {
                            linesOut.Add(line);
                        }
                    }
                    else
                    {
                    }
                }
                reader.Close();
                var writer = new StreamWriter(filename);
                foreach (var s in linesOut)
                {
                    writer.WriteLine(s);
                }
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error: {0}\r\n\r\n{1}", ex.Message, ex.StackTrace));
            }
        }
    }
}

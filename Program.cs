using System;
using System.Text;
using System.IO;

namespace Architecture_Project_2014_15
{
    class Program
    {
        public const int BASE_PROGRAM_ADDR = 0x00400000;
        public const int MEMORY_ENTRY_SIZE_BYTES = 0x04;

        public const int LOOP_MIN = 0;
        public const int LOOP_MAX = 3;
        public const int INNERLOOP_MIN = 0;
        public const int INNERLOOP_MAX = 3;

        static void Main(string[] args)
        {
            Int64 PROGRAM_SIZE = 0x000F0000;

            bool debug_mode_on = false;

            Console.WriteLine("");
            Console.WriteLine("");

            if (args.Length == 0)
            {
                Console.WriteLine("No address space size entered. Using default {0}", PROGRAM_SIZE);
                debug_mode_on = true;
            }
            else
            {
                try 
                {
                    PROGRAM_SIZE = Int64.Parse(args[0]);
                    Console.WriteLine("Address space size: {0}", PROGRAM_SIZE);
                }

                catch (OverflowException e)
                {
                    Console.WriteLine("{0}", e.Source);
                    Console.WriteLine("Nice try...");
                    Console.WriteLine("Using default {0:X}", PROGRAM_SIZE);
                    debug_mode_on = true;
                }

                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught...", e);
                    Console.WriteLine("Using default {0:X}", PROGRAM_SIZE);
                    debug_mode_on = true;
                }
            }

            try
            {

                string folderName = @"C:\Users";
                string pathString = System.IO.Path.Combine(folderName, Environment.UserName);
                string pathString2 = System.IO.Path.Combine(pathString, "Desktop");
                string dtformat = "ddd_MMM_yyy_HH_mm_ss";
                string fileName = DateTime.Now.ToString(dtformat);
                string pathString3 = System.IO.Path.Combine(pathString2, fileName);

                TextWriter tw = File.CreateText(pathString3);

                Console.WriteLine("Created file: {0}", pathString3);

                Random isloopaddr = new Random(Guid.NewGuid().GetHashCode());
                Random loopstowrite = new Random(Guid.NewGuid().GetHashCode());
                Random innerloopstowrite = new Random(Guid.NewGuid().GetHashCode());
                Random innerloopsaddrlength = new Random(Guid.NewGuid().GetHashCode());
                int loopstotal = 1;
                bool loop_true = false;
                bool innerloop_true = false;
                int loops = 0;
                int innerloops = 0;
                int loop_id = 0;
                int innerloop_id = 0;
                int loopaddr = 0;
                int innerloopaddr = 0;
                int innerloopaddrticks = 0;

                Console.WriteLine("Populating file, please wait...");

                for (int i = BASE_PROGRAM_ADDR; i <= BASE_PROGRAM_ADDR + PROGRAM_SIZE; i += MEMORY_ENTRY_SIZE_BYTES)
                {
                    if (isloopaddr.Next(0, 2) == 1)
                    {
                        loop_true = true;
                        loops = isloopaddr.Next(LOOP_MIN, LOOP_MAX);
                        loop_id = loopstotal;
                        loopstotal++;
                        loopaddr = i;

                        if (isloopaddr.Next(0, 4) == 1)
                        {
                            innerloop_true = true;
                            innerloops = innerloopstowrite.Next(INNERLOOP_MIN, INNERLOOP_MAX);
                            innerloopaddrticks = innerloopsaddrlength.Next(0, innerloops);
                            innerloop_id = loopstotal;
                            loopstotal++;
                            if (innerloopaddrticks < 1)
                            {
                                innerloopaddrticks = 1;
                            }
                            innerloopaddr = loopaddr + (innerloopaddrticks * MEMORY_ENTRY_SIZE_BYTES);
                        }
                        else
                        {
                            innerloop_true = false;
                        }
                    }
                    else
                    {
                        loop_true = false;
                    }

                    if(loop_true)
                    {
                        int j = 0;

                        for (j = 0; j < loops; j++)
                        {
                            if (debug_mode_on)
                            {
                                tw.WriteLine("{0:X8} # This is iteration {1}/{2} of loop {3} - T", loopaddr, j, loops, loop_id);
                            }
                            else
                            {
                                tw.WriteLine("{0:X8}", loopaddr);
                            }

                            if (innerloop_true)
                            {
                                int k = 0;

                                for (k = 0; k < innerloops; k++)
                                {
                                    if (debug_mode_on)
                                    {
                                        tw.WriteLine("{0:X8} # This is iteration {1}/{2} of inner loop {3} - T", innerloopaddr, k, innerloops, innerloop_id);
                                    }
                                    else
                                    {
                                        tw.WriteLine("{0:X8}", innerloopaddr);
                                    }
                                }
                                
                                if (debug_mode_on)
                                {
                                    tw.WriteLine("{0:X8} # This is iteration {1}/{2} of inner loop {3} - NT", innerloopaddr, k, innerloops, innerloop_id);
                                }
                                else
                                {
                                    tw.WriteLine("{0:X8}", innerloopaddr);
                                }
                                
                                i = i + (innerloops * MEMORY_ENTRY_SIZE_BYTES);
                            }
                        }
                        
                        if (debug_mode_on)
                        {
                            tw.WriteLine("{0:X8} # This is iteration {1}/{2} of loop {3} - NT", loopaddr, j, loops, loop_id);
                        }
                        else
                        {
                            tw.WriteLine("{0:X8}", loopaddr);
                        }
                        
                        i = i + (loops * MEMORY_ENTRY_SIZE_BYTES);
                    }
                    else
                    {
                        continue;
                    }
                }

                if (debug_mode_on)
                {
                    tw.WriteLine("Total loops: {0}", loopstotal - 1);
                }
                Console.WriteLine("Total loops: {0}", loopstotal - 1);
                tw.Close();
                Console.WriteLine("Population complete.");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();

                }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught...", e);
                Console.WriteLine("Using default {0:X}", PROGRAM_SIZE);
            }
        }
    }
}
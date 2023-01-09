using System.Text;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace Pong
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize variables
            String switchRegex = "(-|\\/)[b|D|B|\\?]"; // detect if arg is a valid switch
            String hostNotKnownRegex = "(No such host is known.|Name or service not known)";
            List<String> targets = new List<String> (); // command line ip / hostname
            List<String> switches = new List<String> (); // command line switches
            List<long> roundTripTimes = new List<long> (); // list of rtt in ms for succesful pings
            int count = 0; // number of ping attempts
            int success = 0; // number of succesful pings
            bool loop = true; // break the while loop
            Ping pong = new Ping();

            // validates args
            foreach (String arg in args)
            {
                if (Regex.IsMatch(arg, switchRegex))
                {
                    switches.Add(arg);
                }
                else 
                {
                    targets.Add(arg);
                }
            }

            // help
            if (switches.Contains("/?") || switches.Contains("-?") || args.Length == 0)
            {
                Console.WriteLine("Usage: [-b][-B][-D] target\n");
                Console.WriteLine("Options:");
                Console.WriteLine("    -b        Play a console beep when a ping is succesful.");
                Console.WriteLine("    -B        Reverse of -b. Play a console beep when a ping is not succesful.");
                Console.WriteLine("    -D        Print timestamp at the start of each line.");
                Environment.Exit(0);
            }

            // only one target specified
            if (targets.Count != 1)
            {
                Console.WriteLine("Invalid parameters.");
                Environment.Exit(1);
            }

            // print ping stats to console on exit
            Console.CancelKeyPress += delegate
            {
                if (success == 0) // no succesful pings
                {
                    Console.WriteLine($"--- {targets[0]} ping statistics ---");
                    Console.WriteLine($"{count} packets sent, 0 recieved, 100% packet loss");

                }
                if (success > 0)
                {
                    Double loss = 100 - (((double)success/(double)count) * 100);
                    roundTripTimes.Sort();
                    long min = roundTripTimes[0];
                    long max = roundTripTimes.Last();
                    long avg = roundTripTimes.Sum()/roundTripTimes.Count;

                    Console.WriteLine($"--- {targets[0]} ping statistics ---");
                    Console.WriteLine($"{count} packets sent, {success} recieved, {(int)loss}% packet loss");
                    Console.WriteLine($"round trip time: min {(int)min}ms, max {(int)max}ms, avg {(int)avg}ms");
                }
            };

            // ping
            while (loop)
            {
                try
                {
                    var reply = pong.Send(targets[0]);
                    // display message on first run
                    if (count == 0)
                    {
                        Console.WriteLine($"Pinging {targets[0]}:");
                    }

                    if (reply.Status == IPStatus.Success)
                    {
                        if (switches.Contains("-D"))
                        {
                            DateTime timestamp = DateTime.Now;
                            Console.Write($"[{timestamp}] ");
                        }
                        Console.WriteLine($"{reply.Address}: Success time={reply.RoundtripTime}ms");
                        if (switches.Contains("-b"))
                        {
                            Console.Beep();
                        }
                        roundTripTimes.Add(reply.RoundtripTime);
                        count++;
                        success++;
                    }
                    else
                    {
                        if (switches.Contains("-D"))
                        {
                            DateTime timestamp = DateTime.Now;
                            Console.Write($"[{timestamp}] ");
                        }
                        String spaced  = Regex.Replace(reply.Status.ToString(), @"([a-z])([A-Z])", "$1 $2");
                        Console.WriteLine($"{reply.Address}: {spaced}");
                        if (switches.Contains("-B"))
                        {
                            Console.Beep();
                        }
                        count++;
                    }
                    Thread.Sleep(1000);
                    }
                catch (Exception exception)
                {
                    if (Regex.IsMatch(exception.ToString(), hostNotKnownRegex))
                    {
                        Console.WriteLine($"Could not find host {targets[0]}. Please check the name and try again.");
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong.");
                    }
                    Environment.Exit(1);
                }
            }

        }
    }
}
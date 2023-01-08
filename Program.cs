using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace Pong
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize variables
            String optionRegex = "-[a-zA-Z]";
            List<String> targets = new List<String> ();
            List<String> options = new List<String> ();

            // validates args
            foreach (String arg in args)
            {
                if (Regex.IsMatch(arg, optionRegex))
                {
                    options.Add(arg);
                }
                else 
                {
                    targets.Add(arg);
                }
            }

            // validate that only one hostname / ip was entered
            if (targets.Count != 1)
            {
                Console.WriteLine("Invalid parameters.");
                Environment.Exit(1);
            }

            // ping something
            Ping pong = new Ping();
            //TODO allow for some standard PingOptions options = new PingOptions();

            Console.WriteLine($"Pinging {targets[0]} with 32 bytes of data:");
            while (true)
            {
                try
                {
                    var reply = pong.Send(targets[0]);
                    if (reply.Status == IPStatus.Success)
                    {
                        Console.WriteLine($"{reply.Buffer.Length} bytes from {reply.Address}: ttl={reply.Options.Ttl} time={reply.RoundtripTime}ms");
                        if (options.Contains("-b"))
                        {
                            Console.Beep();
                        }
                    }
                    else if (reply.Status == IPStatus.DestinationHostUnreachable)
                    {
                        Console.WriteLine($"From {reply.Address}: Destination host unreachable.");
                    }
                    Thread.Sleep(1000);
                    }
                catch (Exception exception)
                {
                    if(exception.InnerException.Message == "No such host is known.")
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
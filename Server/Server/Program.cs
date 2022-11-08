using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SimpleTcpSrvr
{

    class Program
    {
      
        //setup connect client ke server
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            int recv;

            byte[] data = new byte[1024];

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            newsock.Bind(ipep);

            newsock.Listen(10);

            //Deklarasi untuk attribute player
            int HP = 15;
            int Attack = 5;
            int Heal = 2;
            bool turn = true;

            //server side
            Console.WriteLine("Ready to battle!!!");
            Console.WriteLine("Your Health Point = " + HP);
            Console.WriteLine("\n(Type A to Attack or H to Heal)\n");

            Socket client = newsock.Accept();

            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;

            //Console.WriteLine("Connected with {0} at port {1}", clientep.Address, clientep.Port);

            //text for player2
            string welcome = "Enemy Found!!!" +
                "\nYour Health Point = " + HP +
                "\n\n=== First Turn === " + 
                "\n\n(Type A to Attack or H to Heal)\n";

            data = Encoding.UTF8.GetBytes(welcome);

            client.Send(data, data.Length, SocketFlags.None);

            string input;

            while (turn == true)
            {

                data = new byte[1024];

                recv = client.Receive(data);

                if (recv == 0)

                    break;

                Console.WriteLine("\nP1: " + Encoding.UTF8.GetString(data, 0, recv));

                // untuk command Attack
                if (Encoding.UTF8.GetString(data, 0, recv) == "A")
                {
                    HP = HP - Attack;
                    Console.WriteLine("Your Health Point = " + HP);

                    //lost condition
                    if (HP <= 0)
                    {
                        Console.WriteLine("You Lost\n");
                        turn = false;
                    }

                }

                input = Console.ReadLine();

                Console.WriteLine("You: " + input);

                client.Send(Encoding.UTF8.GetBytes(input));

                // untuk command Heal
                if (input == "H")
                {
                    HP = HP + Heal;
                    Console.WriteLine("Your Health Point = " + HP);

                    //lost condition
                    if (HP <= 0)
                    {
                        Console.WriteLine("You Lost\n");
                        turn = false;
                    }

                }
            }

            client.Close();

            newsock.Close();

            Console.WriteLine("Exit Battle Arena....\n");
            Console.ReadLine();

        }
    }
}
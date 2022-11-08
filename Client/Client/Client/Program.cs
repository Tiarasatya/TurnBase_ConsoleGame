using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SimpleTcpClient
{
    class Program
    {
      
        static void Main(string[] args)
        {
            //Deklarasi untuk attribute player
            int HP = 15;
            int Attack = 5;
            int Heal = 2;
            bool turn= true;


            Console.OutputEncoding = Encoding.UTF8;

            byte[] data = new byte[1024];

            string input, stringData;

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ipep);

            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to server.");

                Console.WriteLine(e.ToString());

                return;
            }


            //command permainan

            int recv = server.Receive(data);

            stringData = Encoding.UTF8.GetString(data, 0, recv);

            Console.WriteLine(stringData);

            while (turn == true)
            {
                input = Console.ReadLine();

                if (input == "exit")

                    break;
                // input dari client

                Console.WriteLine("\nYou: " + input);

                server.Send(Encoding.UTF8.GetBytes(input));

                data = new byte[1024];

                // untuk command Heal
                if (input == "H")
                {
                    HP = HP + Heal;
                    Console.WriteLine("Your Health Point = " + HP);
                }

                //data dari server
                recv = server.Receive(data);

                stringData = Encoding.UTF8.GetString(data, 0, recv);

                byte[] utf8string = System.Text.Encoding.UTF8.GetBytes(stringData);

                Console.WriteLine("\nP2: " + stringData);

                // untuk command Heal
                if (Encoding.UTF8.GetString(data, 0, recv) == "A")
                {
                    HP = HP - Attack;
                    Console.WriteLine("Your Health Point = " + HP);
                }

                // loss condition
                if (HP <= 0)
                {
                    Console.WriteLine("You Lost");
                    turn = false;
                }


            }

            Console.WriteLine("\nExit battle arena...");

            server.Shutdown(SocketShutdown.Both);

            server.Close();

            Console.ReadLine();

        }
    }
}
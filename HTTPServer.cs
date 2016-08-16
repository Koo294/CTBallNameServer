using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPListServer
{
    public class HTTPServer
    {

        public const String VERSION = "HTTP/1.1";
        public const String NAME = "ListServer";

        private bool running = false;

        private TcpListener listener;


        public HTTPServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        private void Run()
        {
            running = true;
            listener.Start();

            while (running)
            {
                Console.WriteLine("Waiting for connection...");

                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine("Client connected!");

                HandleClient(client);

                client.Close();
            }

            listener.Stop();
        }

        private void HandleClient(TcpClient client)
        {

            StreamReader reader = new StreamReader(client.GetStream());
            

            String msg = "";
            while (reader.Peek() != -1)
            {
                //doesn't like ReadLine, do it a char at a time
                msg += (char)reader.Read();
            }

            Debug.WriteLine("Request: \n" + msg);

            Request req = Request.GetRequest(msg);
            req.Post();

            Response resp = Response.From(req);
            resp.Post(client.GetStream());
        }
    }
}

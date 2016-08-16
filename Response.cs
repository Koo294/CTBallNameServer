using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace HTTPListServer
{
    public class Response
    {
        private bool SendServers;

        private Response(bool SendServers)
        {
            this.SendServers = SendServers;
        }

        public static Response From(Request request)
        {
            if (request.Type=="GET")
            {
                return new Response(true);
            }
            return MakeNullRequest();
        }

        private static Response MakeNullRequest()
        {
            return new Response(false);
        }

        public void Post(NetworkStream stream)
        {
            if (SendServers)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(NameServersResponse));
                NameServersResponse test = NameServers.Get().GetResponse();

                Console.WriteLine("NameServers: ");
                for (int i = 0; i < test.Names.Length; i++)
                {
                    Console.WriteLine(test.Names[i]);
                    Console.WriteLine(test.IPs[i]);
                }
                
                serializer.WriteObject(stream, test);
            }
        }
    }
}

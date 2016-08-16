using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace HTTPListServer
{
    public class Request
    {
        public String Type { get; private set; }
        public NameServer NameServerContent { get; private set; }

        private Request(String type, NameServer content)
        {
            Type = type;
            NameServerContent = content;
        }

        public static Request GetRequest(String request)
        {
            if (String.IsNullOrEmpty(request))
            {
                return null;
            }
            String[] tokens = request.Split(' ', '\n');
            String[] ContentJson = request.Split('{');

            String type = tokens[0];
            NameServer content = null;
            if (ContentJson.Length == 2 && (type == "POST" || type == "DELETE"))
            {
                String Json = "{" + ContentJson[1];
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(Json));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(NameServer));
                content = (NameServer)serializer.ReadObject(ms);
            }

            return new Request(type, content);
        }

        public void Post()
        {
            if (NameServerContent == null) return;
            NameServers ns = NameServers.Get();
            if (Type == "POST")
            {
                ns.Add(NameServerContent);
            }
            if (Type == "DELETE")
            {
                ns.Remove(NameServerContent);
            }
        }
    }
}

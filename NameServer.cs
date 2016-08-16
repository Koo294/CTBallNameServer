using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPListServer
{
    public class NameServer
    {
        public string Name { get; set; }
        public string IP { get; set; }
    }

    public class NameServersResponse
    {
        public string[] Names { get; set; }
        public string[] IPs { get; set; }
    }

    public class NameServers
    {
        public string[] Names { get; private set; }
        public string[] IPs { get; private set; }

        private static NameServers Instance;

        private NameServers()
        {
            Names = new string[0];
            IPs = new string[0];
        }

        public static NameServers Get()
        {
            if (Instance == null)
            {
                Instance = new NameServers();
            }
            return Instance;
        }

        public void Add(NameServer AddServer)
        {
            string[] NewNames = new string[Names.Length + 1];
            string[] NewIPs = new string[IPs.Length + 1];
            for (int i = 0; i < Names.Length; i++)
            {
                NewNames[i] = Names[i];
                NewIPs[i] = IPs[i];
            }
            NewNames[Names.Length] = AddServer.Name;
            NewIPs[IPs.Length] = AddServer.IP;
            Names = NewNames;
            IPs = NewIPs;
        }
        public bool Remove(NameServer RemoveServer)
        {
            bool ElementRemoved = false;
            string[] NewNames = new string[Names.Length - 1];
            string[] NewIPs = new string[IPs.Length - 1];
            for (int i = 0; i < Names.Length; i++)
            {
                if (ElementRemoved)
                {
                    NewNames[i - 1] = Names[i];
                    NewIPs[i - 1] = IPs[i];
                }
                else
                {
                    if (RemoveServer.Name == Names[i])
                    {
                        ElementRemoved = true;
                    }
                    else
                    {
                        NewNames[i] = Names[i];
                        NewIPs[i] = IPs[i];
                    }
                }
            }
            if (ElementRemoved)
            {
                Names = NewNames;
                IPs = NewIPs;
            }
            return ElementRemoved;
        }
        public NameServersResponse GetResponse()
        {
            NameServersResponse response = new NameServersResponse();
            response.Names = Names;
            response.IPs = IPs;
            return response;
        }
    }
}

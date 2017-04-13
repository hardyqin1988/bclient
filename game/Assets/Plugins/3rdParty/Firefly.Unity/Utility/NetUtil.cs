using Firefly.Core.Utility;
using System;
using System.Net;
using System.Net.Sockets;

namespace Firefly.Unity.Utility
{
    public static class NetUtil
    {
        public static byte[] IntToNetBytes(int i)
        {
            return System.BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
        }

        public static int NetBytesToInt(byte[] bytes)
        {
            return IPAddress.NetworkToHostOrder(System.BitConverter.ToInt32(bytes, 0));
        }

        public static string Resolve(string ipOrHost)
        {
            if (string.IsNullOrEmpty(ipOrHost))
                throw new ArgumentException("Supplied string must not be empty", "ipOrHost");

            ipOrHost = ipOrHost.Trim();

            IPAddress ipAddress = null;
            if (!IPAddress.TryParse(ipOrHost, out ipAddress))
            {
                try
                {
                    IPAddress[] addresses = Dns.GetHostAddresses(ipOrHost);
                    if (addresses == null || addresses.Length == 0)
                        return ipOrHost;

                    int index = Core.Utility.MathUtil.GetRandom(0, addresses.Length - 1);
                    return addresses[index].ToString();
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.HostNotFound)
                    {
                        return ipOrHost;
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }

            return ipOrHost;
        }
    }
}


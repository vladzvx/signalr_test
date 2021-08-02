using Chat.Common;
using System;

namespace client2
{
    class Program
    {
        static void Main(string[] args)
        {
            string url;
            if (args.Length == 0)
            {
                url = "http://87.247.157.64:5002/datahub";
            }
            else
            {
                url = args[0];
            }
            Client client = new Client(url);
            client.StartText();
        }
    }
}

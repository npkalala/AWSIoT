using System;

// The script to gen X.509 pfx
// winpty openssl pkcs12 -export -out certificate.pfx -inkey  MyIoT.private.key -in MyIoT.cert.pem

namespace MQTT
{
    class Program
    {
        public static void Main(string[] args)
        {
            var publisher = new AuMQTT("August");
            //publisher.Publish();
            publisher.Subscribe();
            string read;
            int index = 1;
            while (true)
            {
                read = Console.ReadLine();
                if (read.Equals("s"))
                    publisher.Publish("the value is " + index++);
                if (read.Equals("c"))
                {
                    var res = publisher.IsConnect;
                    Console.WriteLine("MQTT IsConnect : " + res);
                }
            }
        }
    }
}

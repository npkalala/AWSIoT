using System;
using System.Text;

// The script to gen X.509 pfx
// winpty openssl pkcs12 -export -out certificate.pfx -inkey  MyIoT.private.key -in MyIoT.cert.pem

namespace MQTT
{
    class Program
    {
        public static void Main(string[] args)
        {
            var publisher = new AuMQTT("YourDeviceId");
            publisher.Subscribe();
            publisher.ReceiveMqttMsgEvent += Publisher_ReceiveMqttMsgEvent;
            string read;
            int index = 1;
            while (true)
            {
                Console.WriteLine("1.Press 'c' to check MQTT IsConnect 2.Press 's' to publish message");

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

        private static void Publisher_ReceiveMqttMsgEvent(uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            Console.WriteLine(Encoding.UTF8.GetChars(e.Message));
        }
    }
}

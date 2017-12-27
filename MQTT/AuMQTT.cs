using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTT
{
    public class AuMQTT
    {
        /// <summary>
        /// AWS IoT endpoint - replace with your own
        /// </summary>
        public const string IotEndpoint = "a2ozbiqpxuh70j.iot.us-east-1.amazonaws.com";
        /// <summary>
        /// TLS1.2 port used by AWS IoT
        /// </summary>
        private const int BrokerPort = 8883;
        /// <summary>
        /// this must match - partially - what the subscribed is subscribed too
        /// nicksthings = the THING i created in AWS IoT
        /// t1/t555 is just an arbitary topic that i'm publishing to. (It needs 2 parts for the rule I'm using to work)
        /// </summary>
        public string Topic = "test";

        public MqttClient client;

        public AuMQTT(string DeviceId)
        {
            try
            {
                if (client == null)
                {
                    //convert to pfx using openssl
                    //you'll need to add these two files to the project and copy them to the output
                    var clientCert = new X509Certificate2("certificate.pfx", "123");
                    //this is the AWS caroot.pem file that you get as part of the download
                    var caCert = X509Certificate.CreateFromSignedFile("MyIoT.cert.pem"); // this doesn't have to be a new X509 type...

                    client = new MqttClient(IotEndpoint, BrokerPort, true, caCert, clientCert, MqttSslProtocols.TLSv1_2 /*this is what AWS IoT uses*/);

                    client.Connect(DeviceId);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Show MQTT Connect Status 
        /// </summary>
        public bool IsConnect
        {
            get
            {
                return client.IsConnected;
            }
        }

        //https://gist.github.com/adrenalinehit/a4e2684a0b3b0a49b48e#file-mqtt-publisher-cs
        /// <summary>
        /// publish a message
        /// </summary>
        public bool Publish(string message)
        {
            try
            {
                //publish to the topic
                client.Publish(Topic, Encoding.UTF8.GetBytes(message));
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        //https://gist.github.com/adrenalinehit/ccfeba90264a02fb629f#file-mqtt-subscriber-cs
        /// <summary>
        /// Set up the client and listen for inbound messages
        /// </summary>
        public bool Subscribe()
        {
            try
            {
                //event handler for inbound messages
                client.MqttMsgPublishReceived += ClientMqttMsgPublishReceived;

                // '#' is the wildcard to subscribe to anything under the 'root' topic
                // the QOS level here - I only partially understand why it has to be this level - it didn't seem to work at anything else.
                client.Subscribe(new[] { Topic }, new[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        /// <summary>
        /// MqttMsgPublishReceived
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClientMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine("We received a message...");
            Console.WriteLine(Encoding.UTF8.GetChars(e.Message));
        }

        #region
        //public void Subscribe()
        //{
        //    try
        //    {
        //        var clientCert = new X509Certificate2("certificate.pfx", "123");
        //        var caCert = X509Certificate.CreateFromSignedFile("MyIoT.cert.pem");
        //        // create the client
        //        var client = new MqttClient(IotEndpoint, BrokerPort, true, caCert, clientCert, MqttSslProtocols.TLSv1_2, client_RemoteCertificateValidationCallback);
        //        client.ProtocolVersion = MqttProtocolVersion.Version_3_1;
        //        ushort msgId = client.Subscribe(new string[] { Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        //        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
        //        client.MqttMsgSubscribed += Client_MqttMsgSubscribed;

        //        //client = new MqttClient(IotEndpoint, BrokerPort, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);
        //        //byte code = client.Connect(Guid.NewGuid().ToString());
        //        //ushort msgId = client.Subscribe(new string[] { Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        //        //client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

        //    }
        //    catch(Exception ex)
        //    {
        //        var x = ex;
        //    }
        //}

        //bool client_RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    // logic for validation here
        //    var x = 1;
        //    return true;
        //}

        //private void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        //{
        //    var x = e;
        //}

        //private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        //{
        //    Console.WriteLine("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
        //}
        #endregion
    }
}

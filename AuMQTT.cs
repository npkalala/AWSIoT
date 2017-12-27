using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTT
{
    public delegate void ReceiveMqttMsgEventHadler(MqttMsgPublishEventArgs e);
    public class AuMQTT
    {
        public event ReceiveMqttMsgEventHadler ReceiveMqttMsgEvent;
        /// <summary>
        /// AWS IoT endpoint - replace with your own
        /// </summary>
        public const string IotEndpoint = "**********.us-east-1.amazonaws.com";
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
                    var clientCert = new X509Certificate2("YourCertificate.pfx", "YourPassword");
                    //this is the AWS caroot.pem file that you get as part of the download
                    var caCert = X509Certificate.CreateFromSignedFile("YourPemFile.pem"); // this doesn't have to be a new X509 type...

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
            if (this.ReceiveMqttMsgEvent != null)
                this.ReceiveMqttMsgEvent(e);
        }
    }
}

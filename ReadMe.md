# A Simple Sample of Connect to AWSIoT with C#

This sample code including the following fucntions that you can use.

  - Pushlish Message
  - Subscribe Topics
  - Check MQTT IsConnect

# The Steps to use it

  - A. Download your .pem and private key from AWSIot.
  - B. Create .pfx file in the command line using OpenSSL with above.
  - C. Including the .pfx and .pem files to your project


### The Command to create .pfx with OpenSSL 
For Unix based system
```sh
$ openssl pkcs12 -export -out certificate.pfx -inkey  ***.private.key -in ***.cert.pem

```

For Windowsbased system...

```sh
$ winpty openssl pkcs12 -export -out certificate.pfx -inkey  MyIoT.private.key -in MyIoT.cert.pem
```

### Plugins

This sample is currently extended with the following plugins/references. Instructions on how to use them in your own application are linked below.

| Plugin | README |
| ------ | ------ |
| M2MQTT | [https://m2mqtt.wordpress.com/m2mqtt-and-amazon-aws-iot/] |
| AWSIoT | [http://docs.aws.amazon.com/iot/latest/developerguide/iot-sdks.html] |

#### Development
Create an AuMQTT object :
```sh
$ var publisher = new AuMQTT("YourDeviceId");
```

Settings
```csharp
 //'Qos'
 publisher.MqttPolicy = 1; 
 //'pfx file'
 publisher.YourPfxCertificate = "YourCertificate.pfx";
 //'password'
 publisher.YourPfxCertificatePassword = "YourPassword";
 //'Pem file'
 publisher.YourPemFile = "YourPemFile.pem";
```
Setup Topic :
```csharp
publisher.Topic = "YourTopic";
```
Publish Message :
```csharp
publisher.Publish("Message");
```
Subscribe Topic :
```csharp
publisher.Subscribe();
//implement the delegate event
publisher.ReceiveMqttMsgEvent += Publisher_ReceiveMqttMsgEvent;
```
```csharp
private static void Publisher_ReceiveMqttMsgEvent(uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
{
     Console.WriteLine(Encoding.UTF8.GetChars(e.Message));
}
```

### Todos

 - Write Tests
 - Implement others delagate event

License
----

None


**feel free to use it!**


  

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Threading.Tasks;

namespace MQTTPublisher
{
    class Publisher
    {
        static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            IMqttClient client = mqttFactory.CreateMqttClient();
            //opções que o cliente ira usar para se conectar ao broker
            var options = new MqttClientOptionsBuilder()
                        .WithClientId(Guid.NewGuid().ToString())
                        .WithTcpServer("test.mosquitto.org", 1883) // detalhes do servidor broker para uma conexão TCP/ip usando protocolo MQTT
                        .WithCleanSession()
                        .Build();
            client.UseConnectedHandler(e =>    //evento ao conectar com o broker
            {
                Console.WriteLine("Conectado ao broker com sucesso\n ");
            });

            string x;
            await client.ConnectAsync(options); // estabelecendo a conexão 
            while (true)
            {
                await PublishMessageAsync(client);
                Console.WriteLine("Para sair presione uma tecla - x, ou qualquer tecla para continuar\n");
                x = Console.ReadLine();
                if(x == "x")
                {
                    await client.DisconnectAsync(); //desconecata apos publicao 
                    break;
                }
                
            }

        }

        static async Task PublishMessageAsync(IMqttClient client) // metodo que publica a mensagem 
        {
            Console.WriteLine("Por favor, digite a mensagem\n");
            var mensagem = Console.ReadLine();
            string messagePayLoad = $"{mensagem} ";
            var message = new MqttApplicationMessageBuilder()
                        .WithTopic("KAUAN") // definição do topico para aonde a mensagem será publicada 
                        .WithPayload(messagePayLoad)
                        .WithAtLeastOnceQoS()
                        .Build();

            if (client.IsConnected)  //verificando se o cliente está conectado 
            {
                await client.PublishAsync(message);
                Console.WriteLine($"mensagem publicada - {messagePayLoad}\n");
            }
        }
    }
}

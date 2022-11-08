using System;
using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;


string queue = "letterbox";
var message = "Message ";
int msgNumber = 100000;
var stopWatch = new Stopwatch();
var factory = new ConnectionFactory { HostName = "localhost" };


stopWatch.Start();
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.QueueDeclare(queue: "letterbox"
, durable: false
, exclusive: false
, autoDelete: false
, arguments: null
);

Parallel.For(0, msgNumber, (i) =>
//for(int i=0; i < msgNumber; i++)
{
    var msg = message + $"#{i}";
    var encodedMessage = Encoding.UTF8.GetBytes(msg);

    channel.BasicPublish("", queue, null, encodedMessage);
    Console.WriteLine($"Message {msg} Pushed, duration: {stopWatch.ElapsedMilliseconds}");
});
stopWatch.Stop();

Console.WriteLine($"Published Message {message}, duration: {stopWatch.ElapsedMilliseconds}");
using Dummy;
using Greet;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        const string target = "127.0.0.1:50051";
        static async Task Main(string[] args)
        {
            Channel channel = new Channel(target, ChannelCredentials.Insecure);
            await channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("The client connected sussesfully");
            });

            //con dummy.proto
            //var client = new DummyService.DummyServiceClient(channel);

            //con greeting.proto
            var client = new GreetingService.GreetingServiceClient(channel);

            //var greeting = new Greeting() {
            //    FirstName = "Edison",
            //    LastName = "Plaza"
            //};

            //1. Unary
            //var request = new GreetingRequest() { Greeting = greeting };
            //var response = client.Greet(request);
            //Console.WriteLine(response.Result);
            //-------------------------------------


            //2. Server streaming
            //var request = new GreetManyTimesRequest() { Greeting = greeting };
            //var response = client.GreatManyTimes(request);

            //while ( await response.ResponseStream.MoveNext())
            //{
            //    Console.WriteLine(response.ResponseStream.Current.Result);
            //    await Task.Delay(200);
            //}
            //-------------------------------------


            //3. Client streaming
            //var request = new LongGreetRequest() { Greeting = greeting };
            //var stream = client.LongGreet();
            //foreach (int i in Enumerable.Range(1, 10))
            //{
            //    await stream.RequestStream.WriteAsync(request);
            //}

            //await stream.RequestStream.CompleteAsync();

            //var response = stream.ResponseAsync;

            //Console.WriteLine(response.Result);


            //4. BIDi
            var stream = client.GreatEveryone();
            var responseReaderTask = Task.Run(async () =>
            {
                while(await stream.ResponseStream.MoveNext())
                {
                    Console.WriteLine("Received : " +  stream.ResponseStream.Current.Result);
                }
            });

            Greeting[] greetings = {
                new Greeting() { FirstName = "Edison", LastName="Plaza"},
                new Greeting() { FirstName = "Lorena", LastName="Ochoa"},
                new Greeting() { FirstName = "Maria C", LastName="Plaza"}
            };

            foreach (var greeting in greetings)
            {
                Console.WriteLine("Sending : " + greeting.ToString());
                await stream.RequestStream.WriteAsync(new GreetEveryoneRequest()
                {
                    Greeting = greeting
                });
            }

            await stream.RequestStream.CompleteAsync();
            await responseReaderTask;

            //-------------------------------------

            channel.ShutdownAsync().Wait();
            Console.ReadKey();

        }
    }
}

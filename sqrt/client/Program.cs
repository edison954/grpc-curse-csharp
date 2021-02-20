using Grpc.Core;
using Sqrt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string target = "127.0.0.1:50051";
            Channel channel = new Channel(target, ChannelCredentials.Insecure);
            await channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("The client connected sussesfully");
            });


            var client = new SqrtService.SqrtServiceClient(channel);
            int number = -1;

            try
            {
                var response = client.sqrt(new SqrtRequest() { Number = number });
                Console.WriteLine(response.SquareRoot);
            }
            catch (RpcException e)
            {
                Console.WriteLine("Error : " + e.Status.Detail);
                throw;
            }



            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}

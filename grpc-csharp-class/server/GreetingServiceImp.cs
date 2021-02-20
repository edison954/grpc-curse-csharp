using Greet;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Greet.GreetingService;

namespace server
{
    public class GreetingServiceImpl: GreetingServiceBase
    {
        // Unary
        public override Task<GreetingResponse> Greet(GreetingRequest request, ServerCallContext context)
        {
            string result = String.Format("Hello {0} {1}", request.Greeting.FirstName, request.Greeting.LastName);

            return Task.FromResult(new GreetingResponse() { Result = result });
        }

        // Server streaming
        public override async Task GreatManyTimes(GreetManyTimesRequest request, IServerStreamWriter<GreetManyTimesResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("The server received the request : ");
            Console.WriteLine(request.ToString());
            string result = String.Format("Hello {0} {1}", request.Greeting.FirstName, request.Greeting.LastName);

            foreach (int i in Enumerable.Range(1,10))
            {
                await responseStream.WriteAsync(new GreetManyTimesResponse() {  Result = result });
            }
        }

        // Client streaming
        public override async Task<LongGreetResponse> LongGreet(IAsyncStreamReader<LongGreetRequest> requestStream, ServerCallContext context)
        {
            string result = "";
            // en este caso esta retornando una respuesta unicamente cuando se terminan los mensajes del cliente, sin embargo la respuesta se puede retornar en cualquiem moment
            while (await requestStream.MoveNext())
            {
                result += string.Format("Hello {0} {1} {2}", requestStream.Current.Greeting.FirstName, requestStream.Current.Greeting.LastName, Environment.NewLine);
            }

            return new LongGreetResponse() { Result = result };
        }

        // BIDi streaming
        public override async Task GreatEveryone(IAsyncStreamReader<GreetEveryoneRequest> requestStream, IServerStreamWriter<GreetEveryoneResponse> responseStream, ServerCallContext context)
        {

            while(await requestStream.MoveNext())
            {
                var result = string.Format("Hellow {0}, {1}",
                    requestStream.Current.Greeting.FirstName,
                    requestStream.Current.Greeting.LastName);

                Console.WriteLine("Received: " + result);
                await responseStream.WriteAsync(new GreetEveryoneResponse() { Result = result });
            }

        }

    }
}

using Blog;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;
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

            var client = new BlogService.BlogServiceClient(channel);


            //1. Create
            //var response = client.CreateBlog(new CreateBlogRequest() 
            //{ 
            //    Blog = new Blog.Blog() { 
            //        AuthorId = "Edison",
            //        Title = "New blog",
            //        Content = "Hellow world, this is a new blog"
            //    }
            //});
            //Console.WriteLine("the blog " + response.Blog.Id + " was created !");

            //2. Read
            try
            {
                var response = client.ReadBlog(new ReadBlogRequest()
                {
                    BlogId = "60348bf79d7fbea245ac5bcc"
                }); ;
                Console.WriteLine(response.Blog.ToString());
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
            }



            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}

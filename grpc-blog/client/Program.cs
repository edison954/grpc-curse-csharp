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
            //try
            //{
            //    var response = client.ReadBlog(new ReadBlogRequest()
            //    {
            //        BlogId = "60348bf79d7fbea245ac5bcc"
            //    }); ;
            //    Console.WriteLine(response.Blog.ToString());
            //}
            //catch (RpcException e)
            //{
            //    Console.WriteLine(e.Status.Detail);
            //}


            var newBlog = CreateBlog(client);
            //ReadBlog(client);

            //UpdateBlog(client, newBlog);
            //DeleteBlog(client, newBlog);

            await ListBlog(client);

            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }

        private static Blog.Blog CreateBlog(BlogService.BlogServiceClient client)
        {
            var response = client.CreateBlog(new CreateBlogRequest()
            {
                Blog = new Blog.Blog()
                {
                    AuthorId = "Edison",
                    Title = "New blog",
                    Content = "Hellow world, this is a new blog"
                }
            });

            Console.WriteLine("The blog " + response.Blog.Id + " was created!");

            return response.Blog;
        }

        private static void ReadBlog(BlogService.BlogServiceClient client)
        {
            try
            {
                var response = client.ReadBlog(new ReadBlogRequest()
                {
                    BlogId = "60348bf79d7fbea245ac5bcc"
                });

                Console.WriteLine(response.Blog.ToString());
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
            }
        }

        private static void UpdateBlog(BlogService.BlogServiceClient client, Blog.Blog blog)
        {
            try
            {
                blog.AuthorId = "Maria camila";
                blog.Title = "Blog de cami";
                blog.Content = "barbies y muñecos";

                var response = client.UpdateBlog(new UpdateBlogRequest()
                {
                    Blog = blog
                });

                Console.WriteLine(response.Blog.ToString());
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
            }
        }

        private static void DeleteBlog(BlogService.BlogServiceClient client, Blog.Blog blog)
        {
            try
            {
                var response = client.DeleteBlog(new DeleteBlogRequest() { BlogId = blog.Id });

                Console.WriteLine("The blog with id " + response.BlogId + " was deleted");
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
            }
        }

        private static async Task ListBlog(BlogService.BlogServiceClient client)
        {
            var response = client.ListBlog(new ListBlogRequest() { });

            while (await response.ResponseStream.MoveNext())
            {
                Console.WriteLine(response.ResponseStream.Current.Blog.ToString());
            }
        }


    }
}

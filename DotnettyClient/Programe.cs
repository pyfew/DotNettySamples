using DotNetty.Buffers;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DotnettyClient
{
    internal class Programe
    {
        static async Task RunClientAsync()
        {
            var group = new MultithreadEventLoopGroup();
            string targetHost = null;
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;

                        pipeline.AddLast("echo", new MessageHandler());
                    }));

                IChannel bootstrapChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6666));


                while (true)
                {
                    string var = Console.ReadLine();

                }
                await bootstrapChannel.CloseAsync();
            }
            catch { 
            
            }
            finally
            {
                //group.ShutdownGracefullyAsync().Wait(1000);
            }

        }

        public static void Main() { RunClientAsync().Wait(); }

    }
}

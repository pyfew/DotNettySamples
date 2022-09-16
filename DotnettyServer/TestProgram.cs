using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;


namespace DotnettyServer
{
    internal class TestProgram
    {
        #region private config
        const int Port=6666;
        #endregion
        enum ConnectionType
        {
            TCP, UDP
        };

        public static void Main()
        {
            Console.WriteLine("选择监听方式（TCP=1,UDP=2）：");
            var str = Console.ReadLine();
            if (str == "1")
            {
                Console.WriteLine("TCP服务:");
                RunServerAsync(ConnectionType.TCP).Wait();
            }
            else
            {
                Console.WriteLine("UDP服务:");
                RunServerAsync(ConnectionType.UDP).Wait();
            }
            //RunServerAsync(ConnectionType.TCP).Wait();
        }
        static IChannel MainChannel;
        static async Task RunServerAsync(ConnectionType type)
        {
            var bossgroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();
           
            try
            {
                if (type == ConnectionType.TCP)
                {
                    var bootstrap = new ServerBootstrap();
                    //tcp
                    bootstrap.Group(bossgroup,workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .Option(ChannelOption.SoKeepalive, true)
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        MainChannel = channel;
                        IChannelPipeline channelHandlers = channel.Pipeline;
                        channelHandlers.AddLast(new LoginHandler());
                        channelHandlers.AddLast(new TCPServerHandler());
                    }));
                    IChannel bootstrapChannel = await bootstrap.BindAsync(Port);
                    Console.WriteLine("监听端口：" + Port);
                    
                }
                else if (type == ConnectionType.UDP)
                {
                    //udp
                    var bootstrap = new Bootstrap();
                    bootstrap.Group(workerGroup)
                    .Channel<SocketDatagramChannel>()
                    .Option(ChannelOption.SoBroadcast, true)  //广播形式获取数据
                    .Option(ChannelOption.SoReuseport, true)//允许复用端口
                    .Handler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        //Console.WriteLine("childhandler");
                        IChannelPipeline channelHandlers = channel.Pipeline;
                        channelHandlers.AddLast(new UDPServerHandler());
                    }));
                    IChannel bootstrapChannel = await bootstrap.BindAsync(Port);
                    Console.WriteLine("监听端口："+ Port);
                    //Console.ReadLine();
                    //await Task.Delay(60000*5);
                    //await bootstrapChannel.CloseAsync();
                }

                while (true)
                {
                    string cmd = Console.ReadLine();
                    if (cmd?.ToLower() == "Exit".ToLower())
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            finally
            {
                Task.WaitAll(bossgroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
            }
            //Console.WriteLine("服务已退出：" );
        }



    }
}

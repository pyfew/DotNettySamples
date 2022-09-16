using Gtk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StackExchange.Redis;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Channels;
using System.Net;
using DotNetty.Buffers;
using System.Net.Sockets;
using Socket = System.Net.Sockets.Socket;

namespace WinFormsForCore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {



        }

        MessageHandler handler;
        private string IP
        {
            get
            {
                return txtIP.Text;
            }
        }
        private int Port
        {
            get { return int.Parse(string.IsNullOrEmpty(txtPort.Text) ? txtPort.Text : "0"); }
        }

        async Task RunClientAsync()
        {
            var group = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    //TCP
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.SoKeepalive, true) //长连接

                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        handler = new MessageHandler()
                        {
                            ctx = pipeline.FirstContext()
                        };
                        handler.OnMessage += Handler_OnMessage;
                        handler.OnConnected += Handler_OnConnected;
                        pipeline.AddLast("echo", handler);

                    }));

                IChannel clientChannel = await bootstrap.ConnectAsync(IPAddress.Parse(txtIP.Text), int.Parse(txtPort.Text));

            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                //group.ShutdownGracefullyAsync().Wait(1000);
            }


        }

        public async Task UdpSendMsg(string content, string ip, string port)
        {
            var group = new MultithreadEventLoopGroup();
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(port))
            {
                return;
            }
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    //UDP
                    .Channel<SocketDatagramChannel>()
                    .Option(ChannelOption.SoBroadcast, true)  //广播形式获取数据
                                                              //.Option(ChannelOption.SoReuseport, true)//允许复用端口
                                                              //.Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    .Handler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        UdpMsgHandler handler = new UdpMsgHandler();
                        handler.OnMessage += Handler_OnMessage;
                        handler.OnConnected += Handler_OnConnected;
                        pipeline.AddLast("echo", handler);

                    }));

                IChannel clientChannel = await bootstrap.BindAsync(IPEndPoint.MinPort);
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                IByteBuffer buffer = Unpooled.WrappedBuffer(bytes);
                await clientChannel.WriteAndFlushAsync(
                    new DatagramPacket(
                        buffer,
                        new IPEndPoint(IPAddress.Parse(ip), int.Parse(port))));

                //3秒收不到回复就关闭
                await Task.Delay(3000);
                await clientChannel.CloseAsync();

            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally
            {
                group.ShutdownGracefullyAsync().Wait(1000);
            }
        }

        private void Handler_OnMessage(string obj)
        {
            lblmsg.Text = $"服务器应答{DateTime.Now.ToLongTimeString()}：{obj}";
        }

        private void Handler_OnConnected(IChannelHandlerContext obj)
        {

            lblmsg.Text = "已连接";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await RunClientAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            handler.WriteMsg(textBox1.Text);
        }

        private async void btnUdpSend_Click(object sender, EventArgs e)
        {
            await UdpSendMsg(txtcontent.Text, txtIP.Text, txtPort.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                EndPoint serverPoint = new IPEndPoint(IPAddress.Parse(IP), Port);
                string message = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(message);
                udpClient.SendTo(data, serverPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}

using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnettyServer
{
    internal class UDPServerHandler : SimpleChannelInboundHandler<DatagramPacket>
    {
        protected override void ChannelRead0(IChannelHandlerContext context, DatagramPacket packet)
        {
            try
            {
                if (packet.Content.IsReadable())
                {
                    IByteBuffer initialMessage = Unpooled.Buffer(256);
                    initialMessage.WriteBytes(Encoding.UTF8.GetBytes("收到了" ?? throw new InvalidOperationException())); // (2)
                    Console.WriteLine(packet.Content.ToString(Encoding.UTF8));
                    context.WriteAsync(new DatagramPacket(initialMessage, packet.Sender));
                }
           
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }

}

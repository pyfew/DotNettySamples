using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Common;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace DotnettyServer
{
    internal class TCPServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            try
            {
                var buffer = message as IByteBuffer;
                if (buffer != null)
                {
                    Console.WriteLine("Received from client: " + buffer.ToString(Encoding.UTF8));
                }
                else Console.WriteLine("Received from client:  message == null");

                IByteBuffer initialMessage = Unpooled.Buffer(256);
                initialMessage.WriteBytes(Encoding.UTF8.GetBytes("收到了" ?? throw new InvalidOperationException())); // (2)
                context.WriteAndFlushAsync(initialMessage);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
            base.ChannelRead(context, message);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.Write(exception.Message);
            base.ExceptionCaught(context, exception);

        }
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ChannelActive(IChannelHandlerContext context)
        {
            Console.WriteLine(context.Name+",");
            IByteBuffer initialMessage = Unpooled.Buffer(8);
            initialMessage.WriteBytes(Encoding.UTF8.GetBytes("0" ?? throw new InvalidOperationException())); // (2)
            context.WriteAndFlushAsync(initialMessage);
            base.ChannelActive(context);
        }
    }
}

using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnettyClient
{
    internal class MessageHandler : SimpleChannelInboundHandler<object>
    {
        readonly IByteBuffer initialMessage;

        public MessageHandler()
        {
            this.initialMessage = Unpooled.Buffer(256);
            byte[] messageBytes = Encoding.UTF8.GetBytes("Hello world");
            this.initialMessage.WriteBytes(messageBytes);
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            var context = msg.ToString();
           
        }

        IChannelHandlerContext ctx;
        byte[] array;

        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            this.array = new byte[256];
            this.ctx = ctx;

            // Send the initial messages.
            this.GenerateTraffic();
        }

        async void GenerateTraffic()
        {
            try
            {
                IByteBuffer buffer = Unpooled.WrappedBuffer(this.array);
                // Flush the outbound buffer to the socket.
                // Once flushed, generate the same amount of traffic again.
                await this.ctx.WriteAndFlushAsync(buffer);
                this.GenerateTraffic();
            }
            catch
            {
                await this.ctx.CloseAsync();
            }
        }
    }
}

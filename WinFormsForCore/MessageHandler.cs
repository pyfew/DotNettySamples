using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsForCore
{
    internal class MessageHandler : ChannelHandlerAdapter
    {
        readonly IByteBuffer initialMessage;

        public event Action<string> OnMessage;
        public event Action<IChannelHandlerContext> OnConnected;

        public MessageHandler()
        {
            this.initialMessage = Unpooled.Buffer(256);
            byte[] messageBytes = Encoding.UTF8.GetBytes("Hello world");
            this.initialMessage.WriteBytes(messageBytes);
        }

        public void WriteMsg(string text) {
            IByteBuffer imsg = Unpooled.Buffer(256);
            imsg.WriteBytes(Encoding.UTF8.GetBytes(text ?? throw new InvalidOperationException())); // (2)
            this.ctx?.WriteAndFlushAsync(imsg);          
        }

        public override void ChannelRead(IChannelHandlerContext ctx, object msg)
        {
            IByteBuffer buffer = msg as IByteBuffer;
            string message = buffer.ToString(Encoding.UTF8);
            //WriteMsg(context);
            OnMessage?.Invoke(message);
           
        }
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();
        public IChannelHandlerContext ctx;

        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            this.ctx = ctx;
            ctx.WriteAndFlushAsync(this.initialMessage);
            OnConnected?.Invoke(ctx);
        }

        public override Task DisconnectAsync(IChannelHandlerContext context)
        {
            Console.WriteLine("disconnected");
            return base.DisconnectAsync(context);
        }
    }
}

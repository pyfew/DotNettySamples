using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsForCore
{
    internal class UdpMsgHandler : SimpleChannelInboundHandler<DatagramPacket>
    {

        public event Action<IChannelHandlerContext> OnConnected;
        public event Action<string> OnMessage;
        public IChannelHandlerContext ctx;

        protected override void ChannelRead0(IChannelHandlerContext ctx, DatagramPacket msg)
        {
            OnMessage?.Invoke(msg.Content.ToString(Encoding.UTF8));
        }

        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            this.ctx = ctx;
           
            OnConnected?.Invoke(ctx);
        }
    }
}

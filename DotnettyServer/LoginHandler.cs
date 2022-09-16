using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnettyServer
{
    internal class LoginHandler : ChannelHandlerAdapter
    {
        /// <summary>
        /// 验证登录是否
        /// </summary>
        bool bVerifyLogin = false;

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            //context.Channel.DisconnectAsync().Wait();
            if (bVerifyLogin)
            {
                Console.WriteLine("已经登录");
                ReferenceCountUtil.Release(message);
            }
            else
            {
                context.FireChannelRead(message);
            }
            //base.ChannelRead(context, message);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.Write("ex");
            base.ExceptionCaught(context, exception);

        }
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

       
    }
}

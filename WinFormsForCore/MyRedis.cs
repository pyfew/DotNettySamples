using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsForCore
{
    public static class MyRedis
    {
        public static string url = "192.168.255.128:6379";
        public static int defaultdb = 0;
        public static string psw = "123456";
        private static IDatabase _Db;

        public static IDatabase Db
        {
            get
            {
                if (_Db == null)
                {
                    ConnectionMultiplexer redis = ConnectionMultiplexer.Connect($"{url},defaultDatabase={defaultdb},password={psw}");
                    return redis.GetDatabase(0);
                }
                else return _Db;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craxx
{
    public static class TimeUtil
    {
        private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        public static long GetUnixTime(DateTime targetTime)
        {
            // UTC時間に変換
            targetTime = targetTime.ToUniversalTime();

            // UNIXエポックからの経過時間を取得
            TimeSpan elapsedTime = targetTime - UNIX_EPOCH;

            // 経過秒数に変換
            return (long)elapsedTime.TotalSeconds;
        }
    }
}

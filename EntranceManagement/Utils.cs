using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranceManagement
{
    /// <summary>
    /// ユーティリティ関数
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// 引数の日時の00:00を返す
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime startTime(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
        }

        /// <summary>
        /// 引数の日時の23:59を返す
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime endTime(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, 23, 59, 59);
        }
    }
}

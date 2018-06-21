using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 处理时间的帮助类
    /// </summary>
    public class TimeHelper
    {
        /// <summary>
        /// 每一天的时间秒数
        /// </summary>
        public const long TimePerDay = 86400;

        /// <summary>
        ///时间戳Timestamp转换成日期  
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime Second2DateTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = ((long)timeStamp * 10000000);
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime targetDt = dtStart.Add(toNow);
            return targetDt;
        }


        /// <summary>
        /// DataTime 转换成时间戳Timestamp(秒)
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long DateTime2Secend(DateTime time)
        {
            DateTime dateStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1,0,0,0));
            int timeStamp = Convert.ToInt32((time - dateStart).TotalSeconds);
            return timeStamp;
        }

        /// <summary>
        /// 当前系统时间转成秒数
        /// </summary>
        /// <returns></returns>
        public static long DateTime2Secend_Now()
        {
            DateTime dt = System.DateTime.Now;  //当前系统时间
            return DateTime2Secend(dt);
        }




        /// <summary>
        /// 根据秒数获取显示的时间(分：秒) 04：23
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetShowTimeFromSecond_Second(int time)
        {
            if (time >= 3600)
                return GetShowTimeFromSecond_Hour(time);  //超过一个小时

            int mins = time / 60;
            int seconds = time % 60;
            string timer_string = "";
            if (mins < 10)
                timer_string += "0" + mins.ToString() + ":";
            else
                timer_string += mins.ToString() + ":";


            if (seconds < 10)
                timer_string += "0" + seconds.ToString();
            else
                timer_string += seconds.ToString();
            return timer_string;
        }

        /// <summary>
        /// 根据秒数获取显示的时间(时：分：秒) 01:04：23
        /// </summary>
        /// <param name="time">秒</param>
        /// <returns></returns>
        public static string GetShowTimeFromSecond_Hour(int time)
        {
            int hours = time / 3600;  //小时
            int mins = (time - hours * 3600) / 60;//分
            int seconds = time - hours * 3600 - mins * 60;  //秒

            string timer_string = "";
            if (hours < 10)
                timer_string += "0" + hours.ToString() + ":";
            else
                timer_string += hours.ToString() + ":";


            if (mins < 10)
                timer_string += "0" + mins.ToString() + ":";
            else
                timer_string += mins.ToString() + ":";


            if (seconds < 10)
                timer_string += "0" + seconds.ToString();
            else
                timer_string += seconds.ToString();
            return timer_string;
        }



    }
}

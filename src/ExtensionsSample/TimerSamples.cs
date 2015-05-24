﻿using System;
using WebJobs.Extensions.Timers;

namespace WebJobsSandbox
{
    public class TimerSamples
    {
        /// <summary>
        /// Example job triggered by a crontab schedule.
        /// </summary>
        public void CronJob([TimerTrigger("*/10 * * * * *")] TimerInfo timer)
        {
            Console.WriteLine("Scheduled job fired!");
        }

        /// <summary>
        /// Example job triggered by a timespan schedule.
        /// </summary>
        public void TimerJob([TimerTrigger("00:00:10")] TimerInfo timer)
        {
            Console.WriteLine("Scheduled job fired!");
        }

        /// <summary>
        /// Example job triggered by a custom schedule.
        /// </summary>
        public void WeeklyTimerJob([TimerTrigger(typeof(MyWeeklySchedule))] TimerInfo timer)
        {
            Console.WriteLine("Scheduled job fired!");
        }
    }

    public class MyWeeklySchedule : WeeklySchedule
    {
        public MyWeeklySchedule()
        {
            // Every Monday at 8 AM
            Add(DayOfWeek.Monday, new TimeSpan(8, 0, 0));

            // Twice on Wednesdays at 9:30 AM and 9:00 PM
            Add(DayOfWeek.Wednesday, new TimeSpan(9, 30, 0));
            Add(DayOfWeek.Wednesday, new TimeSpan(21, 30, 0));

            // Every Friday at 10:00 AM
            Add(DayOfWeek.Friday, new TimeSpan(10, 0, 0));
        }
    }

    public class MyDailySchedule : DailySchedule
    {
        public MyDailySchedule() :
            base("8:00:00", "12:00:00", "22:00:00")
        {
        }
    }
}

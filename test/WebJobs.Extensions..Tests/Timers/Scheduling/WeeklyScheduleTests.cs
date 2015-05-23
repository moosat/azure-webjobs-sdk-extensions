﻿using System;
using WebJobs.Extensions.Timers;
using Xunit;

namespace WebJobs.Extensions.Tests.Timers.Scheduling
{
    public class WeeklyScheduleTests
    {
        [Fact]
        public void GetNextOccurrence_EmptySchedule_Throws()
        {
            WeeklySchedule schedule = new WeeklySchedule();

            Assert.Throws<InvalidOperationException>(() => schedule.GetNextOccurrence(DateTime.Now));
        }

        [Fact]
        public void GetNextOccurrence_SingleDaySchedule_MultipleScheduleIterations()
        {
            WeeklySchedule schedule = new WeeklySchedule();

            // simple MWF schedule
            Tuple<DayOfWeek, TimeSpan>[] scheduleData = new Tuple<DayOfWeek, TimeSpan>[]
            {
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Monday, new TimeSpan(9, 0, 0))
            };

            // set now to be before the first occurrence
            DateTime now = new DateTime(2015, 5, 23, 7, 30, 0);
            VerifySchedule(scheduleData, now);
        }

        [Fact]
        public void GetNextOccurrence_ThreeDaySchedule_MultipleScheduleIterations()
        {
            WeeklySchedule schedule = new WeeklySchedule();

            // simple MWF schedule
            Tuple<DayOfWeek, TimeSpan>[] scheduleData = new Tuple<DayOfWeek, TimeSpan>[]
            {
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Monday, new TimeSpan(9, 0, 0)),
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Wednesday, new TimeSpan(18, 0, 0)),
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Friday, new TimeSpan(9, 30, 0))
            };

            // set now to be before the first occurrence
            DateTime now = new DateTime(2015, 5, 23, 7, 30, 0);
            VerifySchedule(scheduleData, now);
        }

        [Fact]
        public void GetNextOccurrence_ComplicatedSchedule_MultipleScheduleIterations()
        {
            // schedule with multiple times per day
            Tuple<DayOfWeek, TimeSpan>[] scheduleData = new Tuple<DayOfWeek, TimeSpan>[]
            {
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Monday, new TimeSpan(8, 0, 0)),
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Monday, new TimeSpan(20, 30, 0)),
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Tuesday, new TimeSpan(12, 0, 0)),
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0)),
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Wednesday, new TimeSpan(15, 25, 15)),
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Wednesday, new TimeSpan(22, 30, 00)),
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Friday, new TimeSpan(9, 30, 0)),
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Saturday, new TimeSpan(5, 30, 0)),
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Saturday, new TimeSpan(17, 30, 0)),
                new Tuple<DayOfWeek, TimeSpan>(DayOfWeek.Sunday, new TimeSpan(10, 00, 0))
            };

            // set now to be before the first occurrence
            DateTime now = new DateTime(2015, 5, 24, 12, 0, 0);
            VerifySchedule(scheduleData, now);
        }

        private void VerifySchedule(Tuple<DayOfWeek, TimeSpan>[] scheduleData, DateTime now)
        {
            WeeklySchedule schedule = new WeeklySchedule();
            foreach (var occurrence in scheduleData)
            {
                schedule.Add(occurrence.Item1, occurrence.Item2);
            }

            // loop through the full schedule a few times, ensuring we cross over
            // a month boundary ensuring day handling is correct
            for (int i = 0; i < 10; i++)
            {
                // run through the entire schedule once
                for (int j = 0; j < scheduleData.Length; j++)
                {
                    DateTime nextOccurrence = schedule.GetNextOccurrence(now);
                    var expectedOccurrence = scheduleData[j];
                    Assert.Equal(expectedOccurrence.Item1, nextOccurrence.DayOfWeek);
                    Assert.Equal(expectedOccurrence.Item2, nextOccurrence.TimeOfDay);

                    now = nextOccurrence + TimeSpan.FromSeconds(1);
                }
            }
        }
    }
}
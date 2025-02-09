using System;
using FplBot.Tests.Helpers;
using Slackbot.Net.Extensions.FplBot.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace FplBot.Tests
{
    public class DateTimeUtilsTests
    {
        private readonly ITestOutputHelper _helper;
        private DateTimeUtils _deadlineChecker;

        public DateTimeUtilsTests(ITestOutputHelper helper)
        {
            _helper = helper;
            _deadlineChecker = Factory.Create<DateTimeUtils>();
        }
        
        [Fact]
        public void WhenDayBefore()
        {
            _deadlineChecker.NowUtc = new DateTime(2005, 5, 24, 19, 0, 0);
            var deadline = new DateTime(2005, 5, 25, 20, 0, 0);
            Assert.False(_deadlineChecker.IsWithinMinutesToDate(60, deadline));
        }
        
        [Fact]
        public void WhenBeforeTheMinute()
        {
            _deadlineChecker.NowUtc = new DateTime(2005, 5, 25, 19, 59, 59);
            var deadline = new DateTime(2005, 5, 25, 20, 0, 0);
            Assert.False(_deadlineChecker.IsWithinMinutesToDate(60, deadline));
        }
        
        [Fact]
        public void WhenIsAnySecondWithTheMinute()
        {
            var deadline = new DateTime(2005, 5, 25, 20, 0, 0);
            
            for(var i = 0; i < 60; i++)
            {
                _deadlineChecker.NowUtc = new DateTime(2005, 5, 25, 19, 0, i);
                var isTheMinute = _deadlineChecker.IsWithinMinutesToDate(60, deadline);
                if (!isTheMinute)
                {
                    _helper.WriteLine($"Not true for {i} - {_deadlineChecker.NowUtc-deadline}");
                }
                
                Assert.True(isTheMinute);
            }
        }

        [Fact]
        public void WhenPassedTheMinute()
        {
            _deadlineChecker.NowUtc = new DateTime(2005, 5, 25, 19, 1, 0);
            var deadline = new DateTime(2005, 5, 25, 20, 0, 0);
            Assert.False(_deadlineChecker.IsWithinMinutesToDate(60, deadline));
        }
        
        [Fact]
        public void WhenAnotherHourTheSameDayButSameMinute()
        {
            _deadlineChecker.NowUtc = new DateTime(2005, 5, 25, 20, 0, 0);
            var deadline = new DateTime(2005, 5, 25, 20, 0, 0);
            Assert.False(_deadlineChecker.IsWithinMinutesToDate(60, deadline));
        }

        [Fact]
        public void WhenTheDayAfterButMinute()
        {
            _deadlineChecker.NowUtc = new DateTime(2005, 5, 26, 19, 0, 0);
            var deadline = new DateTime(2005, 5, 25, 20, 0, 0);
            Assert.False(_deadlineChecker.IsWithinMinutesToDate(60, deadline));
        }
    }
}
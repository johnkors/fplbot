using System;
using FplBot.ConsoleApps.Clients;
using SlackConnector.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slackbot.Net.Workers.Handlers;
using Slackbot.Net.Workers.Publishers;

namespace FplBot.ConsoleApps
{
    public class FplCommandHandler : IHandleMessages
    {
        private readonly IEnumerable<IPublisher> _publishers;
        private readonly IFplClient _fplClient;

        public FplCommandHandler(IEnumerable<IPublisher> publishers, IFplClient fplClient)
        {
            _publishers = publishers;
            _fplClient = fplClient;
        }

        public Tuple<string, string> GetHelpDescription()
        {
            return new Tuple<string, string>("fpl", "Henter stillingen fra Blank-liga");
        }

        public async Task<HandleResponse> Handle(SlackMessage message)
        {
            var standings = await _fplClient.GetStandings("579157");

            foreach (var p in _publishers)
            {
                await p.Publish(new Notification
                {
                    Recipient = message.ChatHub.Id,
                    Msg = standings
                });
            }

            return new HandleResponse(standings);
        }

        public bool ShouldHandle(SlackMessage message)
        {
            return message.MentionsBot && message.Text.Contains("fpl");
        }

        public bool ShouldShowInHelp => true;
    }
}
﻿using Fpl.Client.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Slackbot.Net.Extensions.FplBot.Helpers
{
    internal class GameweekHelper : IGameweekHelper
    {
        private readonly IGameweekClient _gameweekClient;

        public GameweekHelper(IGameweekClient gameweekClient)
        {
            _gameweekClient = gameweekClient;
        }

        public async Task<int?> ExtractGameweekOrFallbackToCurrent(MessageHelper helper,string messageText, string pattern)
        {
            var extractedGw = helper.ExtractGameweek(messageText, pattern);
            return extractedGw ?? (await _gameweekClient.GetGameweeks()).SingleOrDefault(x => x.IsCurrent)?.Id;
        }
    }

    internal interface IGameweekHelper
    {
        /// <summary>
        /// Extracts gameweek number from message text using pattern "some text here {gw}". E.g. "captains {gw}". Returns current gameweek if not found in text.
        /// </summary>
        /// <param name="messageText">Message text</param>
        /// <param name="pattern">Pattern (excluding bot handle). E.g. "captains {gw}"</param>
        /// <returns>Extracted gameweek if found, else current gameweek.</returns>
        Task<int?> ExtractGameweekOrFallbackToCurrent(MessageHelper helper, string messageText, string pattern);
    }

}

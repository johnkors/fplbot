﻿using Fpl.Client.Abstractions;
using Slackbot.Net.Extensions.FplBot.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Slackbot.Net.Extensions.FplBot.Helpers
{
    internal class ChipsPlayed : IChipsPlayed
    {
        private readonly IEntryHistoryClient _historyClient;

        public ChipsPlayed(IEntryHistoryClient historyClient)
        {
            _historyClient = historyClient;
        }

        public async Task<bool> GetHasUsedTripleCaptainForGameWeek(int gameweek, int teamCode)
        {
            var entryHistory = await _historyClient.GetHistory(teamCode);

            var tripleCapChip = entryHistory.Chips.FirstOrDefault(chip => chip.Name == Constants.ChipNames.TripleCaptain);
            if (tripleCapChip == null)
            {
                return false;
            }
            return tripleCapChip.Event == gameweek;
        }
    }
}
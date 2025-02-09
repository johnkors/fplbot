using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fpl.Client.Abstractions;
using Microsoft.Extensions.Options;
using Slackbot.Net.Extensions.FplBot.Abstractions;
using Slackbot.Net.Extensions.FplBot.Extensions;

namespace Slackbot.Net.Extensions.FplBot.Helpers
{
    internal class CaptainsByGameWeek : ICaptainsByGameWeek
    {
        private readonly IOptions<FplbotOptions> _options;
        private readonly IEntryClient _entryClient;
        private readonly IPlayerClient _playerClient;
        private readonly ILeagueClient _leagueClient;
        private readonly IChipsPlayed _chipsPlayed;

        public CaptainsByGameWeek(IOptions<FplbotOptions> options, IEntryClient entryClient, IPlayerClient playerClient, ILeagueClient leagueClient, IChipsPlayed chipsPlayed)
        {
            _options = options;
            _entryClient = entryClient;
            _playerClient = playerClient;
            _leagueClient = leagueClient;
            _chipsPlayed = chipsPlayed;
        }
        
        public async Task<string> GetCaptainsByGameWeek(int gameweek)
        {
            try
            {
                var league = await _leagueClient.GetClassicLeague(_options.Value.LeagueId);
                var players = await _playerClient.GetAllPlayers();

                var sb = new StringBuilder();

                sb.Append($":boom: *Captain picks for gameweek {gameweek}*\n");

                foreach (var team in league.Standings.Entries)
                {
                    try
                    {
                        var entry = await _entryClient.GetPicks(team.Entry, gameweek);

                        var hasUsedTripleCaptainForGameWeek = await _chipsPlayed.GetHasUsedTripleCaptainForGameWeek(gameweek, team.Entry);

                        var captainPick = entry.Picks.SingleOrDefault(pick => pick.IsCaptain);
                        var captain = players.SingleOrDefault(player => player.Id == captainPick.PlayerId);

                        var viceCaptainPick = entry.Picks.SingleOrDefault(pick => pick.IsViceCaptain);
                        var viceCaptain = players.SingleOrDefault(player => player.Id == viceCaptainPick.PlayerId);

                        sb.Append($"*{team.GetEntryLink(gameweek)}* - {captain.FirstName} {captain.SecondName} ({viceCaptain.FirstName} {viceCaptain.SecondName}) ");
                        if (hasUsedTripleCaptainForGameWeek)
                        {
                            sb.Append("TRIPLECAPPED!! :rocket::rocket::rocket::rocket:");
                        }
                        sb.Append("\n");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return $"Oops: {e.Message}";
            }
        }
    }
}
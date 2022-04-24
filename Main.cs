using BTMovementTracker.Helpers;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using ShimmyMySherbet.DiscordWebhooks.Embeded;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace BTMovementTracker
{
    public class Main : RocketPlugin<MovementTrackerConfiguration>
    {
        public static Main Instance;
        protected override void Load()
        {
            Instance = this;
            Logger.Log("#############################################", ConsoleColor.Yellow);
            Logger.Log("###           BTMovementTracker           ###", ConsoleColor.Yellow);
            Logger.Log("###   Plugin Created By blazethrower320   ###", ConsoleColor.Yellow);
            Logger.Log("###            Join my Discord:           ###", ConsoleColor.Yellow);
            Logger.Log("###     https://discord.gg/YsaXwBSTSm     ###", ConsoleColor.Yellow);
            Logger.Log("#############################################", ConsoleColor.Yellow);
            //
            //UnturnedPlayerEvents.OnPlayerUpdatePosition += OnPlayerUpdatePosition;
            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            StopCoroutine(CheckPosition(player));
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            StartCoroutine(CheckPosition(player));
        }
        public IEnumerator CheckPosition(UnturnedPlayer player)
        {
            while (true && Provider.clients.Contains(player.SteamPlayer()))
            {
                yield return new WaitForSeconds(30f);
                if (!player.HasPermission(Main.Instance.Configuration.Instance.TrackPermission))
                {
                    yield break;
                }
                if (Main.Instance.Configuration.Instance.LogInConsole)
                {
                    Logger.Log(player.CharacterName + " Position Update: " + player.Position);
                }
                if (Main.Instance.Configuration.Instance.useWebhook)
                {
                    ThreadHelper.RunAsynchronously(() => {
                        WebhookMessage PositionUpdate = new WebhookMessage()
                        .PassEmbed()
                        .WithTitle("MovementTracker - Position Update")
                        .WithColor(EmbedColor.BlueViolet)
                        .WithURL("https://steamcommunity.com/profiles/" + player.CSteamID)
                        .WithTimestamp(DateTime.Now)
                        .WithField("Username", player.CharacterName)
                        .WithField("SteamId", player.CSteamID.ToString())
                        .WithField("New Position", player.Position.ToString())
                        .WithField("Delay", Main.Instance.Configuration.Instance.TrackDelay.ToString())
                        .Finalize();
                        DiscordWebhookService.PostMessageAsync(Main.Instance.Configuration.Instance.WebhookURL, PositionUpdate);
                    });
                }
            }
        }
        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            Logger.Log("BTMovementTracker Unloaded");
        }
    }
}

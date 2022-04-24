using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTMovementTracker
{
    public class MovementTrackerConfiguration : IRocketPluginConfiguration
    {
        public int TrackDelay { get; set; }
        public string TrackPermission { get; set; }
        public bool LogInConsole { get; set; }
        public bool useWebhook { get; set; }
        public string WebhookURL { get; set; }
        public void LoadDefaults()
        {
            TrackDelay = 30;
            TrackPermission = "MovementTracker.Track";
            LogInConsole = true;
            useWebhook = false;
            WebhookURL = "https://discordapp.com/api/webhooks/{webhook.id}/{webhook.api}";
        }
    }
}

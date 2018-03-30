using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GoogleCast;
using GoogleCast.Channels;
using GoogleCast.Models.Media;
using Karpach.Remote.Commands.Base;
using Karpach.Remote.Commands.Interfaces;
using NLog;
using Image = System.Drawing.Image;

namespace Karpach.Remote.Chromecast.Command
{
    [Export(typeof(IRemoteCommand))]
    public class ChromecastCommand : CommandBase
    {
        public ChromecastCommand():base(null)
        {
        }

        public ChromecastCommand(Guid? id) : base(id)
        {
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected override Type SettingsType => typeof(ChromecastCommandSettings);
        public override string CommandTitle => ConfiguredValue ? ((ChromecastCommandSettings)Settings).CommandName : $"Chromecast Command - {NotConfigured}";
        public override Image TrayIcon => Resources.Icon.ToBitmap();
        private Lazy<Task<Sender>> _sender = null;

        public override void RunCommand(params object[] parameters)
        {
            if (!Configured)
            {
                return;
            }
            int? delay = ((ChromecastCommandSettings)Settings).ExecutionDelay;
            if (delay.HasValue)
            {
                Thread.Sleep(delay.Value);
            }
            if (parameters != null && parameters.Length >= 1)
            {
                string videoId = parameters[0].ToString();
                string contentType = parameters.Length > 1 ? parameters[1].ToString() : null;
                PlayVideoAsync(videoId, contentType).Wait();
            }
            else
            {
                PlayVideoAsync("KpllAjxOIUU").Wait();
            }            
        }

        private Task PlayVideoAsync(string videoId, string contentType = null)
        {
            return Task.Run(async () =>
            {
                ChromecastCommandSettings settings = (ChromecastCommandSettings)Settings;
                IReceiver[] receivers = (await new DeviceLocator().FindReceiversAsync().ConfigureAwait(false)).ToArray();
                if (!receivers.Any())
                {
                    Logger.Log(LogLevel.Error, "No ChromeCasts found.");
                    return;
                }
                IReceiver chromeCast;
                if (!string.IsNullOrEmpty(settings.ChromeCastName))
                {
                    chromeCast = receivers.FirstOrDefault(r => string.Equals(settings.ChromeCastName, r.FriendlyName, StringComparison.InvariantCultureIgnoreCase)) ??
                                 receivers.First();
                }
                else
                {
                    chromeCast = receivers.First();
                }

                Lazy<Task<Sender>> sender = GetSender(chromeCast);

                if (Regex.IsMatch(videoId, "^https?://"))
                {                    
                    await PlayVideoAsync(sender, videoId, contentType, settings.Volume).ConfigureAwait(false);
                }
                else
                {
                    await PlayYoutubeAsync(sender, chromeCast.IPEndPoint.Address.ToString(), videoId, settings.Volume).ConfigureAwait(false);
                }                
            });
        }

        private async Task PlayYoutubeAsync(Lazy<Task<Sender>> sender, string ip, string videoId, float? volume)
        {
            string url = $"http://{ip}:8008/apps/YouTube";
            HttpClient client = new HttpClient();
            StringContent httpContent = new StringContent($"v={videoId}", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await client.PostAsync(url, httpContent).ConfigureAwait(false);
            HttpResponseMessage response = httpResponseMessage;
            if (response.StatusCode != HttpStatusCode.Created)
            {
                Logger.Log(LogLevel.Error, response.Content);
            }
            await SetVolumeAsync(sender, volume).ConfigureAwait(false);
        }

        private async Task PlayVideoAsync(Lazy<Task<Sender>> sender, string url, string contentType, float? volume)
        {            
            // Launch the default media receiver application
            Sender actualSender = await sender.Value.ConfigureAwait(false);
            var mediaChannel = actualSender.GetChannel<IMediaChannel>();
            await actualSender.LaunchAsync(mediaChannel).ConfigureAwait(false);                       

            // Load and play video or audio over http          
            await mediaChannel.LoadAsync(new Media
            {
                ContentId = url,
                ContentType = contentType
            }).ConfigureAwait(false);
            await SetVolumeAsync(sender, volume).ConfigureAwait(false);
        }

        private Lazy<Task<Sender>> GetSender(IReceiver chromeCast)
        {
            return _sender ?? (_sender = new Lazy<Task<Sender>>(async () =>
            {
                var s = new Sender();
                await s.ConnectAsync(chromeCast).ConfigureAwait(false);
                return s;
            }));
        }

        private async Task SetVolumeAsync(Lazy<Task<Sender>> sender, float? volume)
        {
            if (!volume.HasValue)
            {
                return;
            }
            Sender actualSender = await sender.Value.ConfigureAwait(false);
            var receiverChannel = actualSender.GetChannel<IReceiverChannel>();
            await receiverChannel.SetVolumeAsync(volume.Value).ConfigureAwait(false);
            await actualSender.DisconnectAsync().ConfigureAwait(false);
            _sender = null;
        }

        public override void ShowSettings()
        {
            var dlg = new ChromecastCommandSettingsForm((ChromecastCommandSettings)Settings);
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                LibSettings[Id] = dlg.Settings;
                ConfiguredValue = true;
            }
        }

        public override IRemoteCommand Create(Guid id)
        {
            return new ChromecastCommand(id);
        }
    }
}

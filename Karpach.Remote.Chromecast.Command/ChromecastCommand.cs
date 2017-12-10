using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Karpach.Remote.Commands.Base;
using Karpach.Remote.Commands.Interfaces;
using NLog;
using SharpCaster.Services;

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
            if (parameters != null && parameters.Length == 1)
            {
                string parameter = parameters[0].ToString();
                PlayYoutubeVideoAsync(parameter).Wait();
            }
            else
            {
                PlayYoutubeVideoAsync("KpllAjxOIUU").Wait();
            }            
        }

        private Task PlayYoutubeVideoAsync(string videoId)
        {
            return Task.Run(async () =>
            {
                string ip = ((ChromecastCommandSettings) Settings).ChromeCastIP;
                if (string.IsNullOrEmpty(ip))
                {
                    ObservableCollection<SharpCaster.Models.Chromecast> chromecasts = await ChromecastService.Current
                        .StartLocatingDevices()
                        .ConfigureAwait(false);
                    if (!chromecasts.Any())
                    {
                        Logger.Log(LogLevel.Error, "No ChromeCasts found, try to specify ip of chromecast manually.");
                        return;
                    }
                    var chromecast = chromecasts.First();                                        
                    ip = chromecast.DeviceUri.Host;
                }                
                string url = $"http://{ip}:8008/apps/YouTube";
                HttpClient client = new HttpClient();
                StringContent httpContent = new StringContent($"v={videoId}", Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await client.PostAsync(url, httpContent).ConfigureAwait(false);
                HttpResponseMessage response = httpResponseMessage;
                if (response.StatusCode != HttpStatusCode.Created)
                {
                    Logger.Log(LogLevel.Error, response.Content);
                }
            });
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

using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChromecastYoutubeCaster;
using Karpach.Remote.Commands.Base;
using Karpach.Remote.Commands.Interfaces;
using NLog;
using SampleCommand;
using SharpCaster.Services;

namespace Karpach.Remote.Chromecast.Command
{
    public class ChromecastCommand : CommandBase
    {
        public ChromecastCommand():base(null)
        {
        }

        public ChromecastCommand(Guid? id) : base(id)
        {
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
                string parameter = parameters[0].ToString().ToLower();
                PlayYoutubeVideo(parameter).Wait();
            }
            else
            {
                PlayYoutubeVideo("KpllAjxOIUU").Wait();
            }            
        }

        private async Task PlayYoutubeVideo(string videoId)
        {
            ObservableCollection<SharpCaster.Models.Chromecast> chromecasts = await ChromecastService.Current.StartLocatingDevices();
            var chromecast = chromecasts.First();
            var chromecastYouTubeCaster = new ChromecastYouTubeCaster(chromecast.DeviceUri.Host);
            var response = await chromecastYouTubeCaster.PlayVideo(videoId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Logger.Log(LogLevel.Error, response.Content);
            }
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

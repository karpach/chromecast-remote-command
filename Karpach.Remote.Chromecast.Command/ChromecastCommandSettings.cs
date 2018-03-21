using Karpach.Remote.Commands.Base;

namespace Karpach.Remote.Chromecast.Command
{
    public class ChromecastCommandSettings : CommandSettingsBase
    {
        public string CommandName { get; set; }
        public int? ExecutionDelay { get; set; }
        public string ChromeCastName { get; set; }
    }
}   
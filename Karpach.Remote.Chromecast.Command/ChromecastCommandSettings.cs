using Karpach.Remote.Commands.Base;

namespace SampleCommand
{
    public class ChromecastCommandSettings : CommandSettingsBase
    {
        public string CommandName { get; set; }
        public int? ExecutionDelay { get; set; }
    }
}   
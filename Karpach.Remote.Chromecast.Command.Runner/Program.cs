using System;
using System.IO;
using System.Reflection;

namespace Karpach.Remote.Chromecast.Command.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = typeof(ChromecastCommandSettings).Assembly;
            string iniFilePath = $"{(object)Path.GetDirectoryName(assembly.Location)}\\{(object)assembly.GetName().Name}.ini";
            File.WriteAllText(iniFilePath, @"
[Karpach.Remote.Chromecast.Command.ChromecastCommandSettings]
CommandName = Chromecast
ExecutionDelay = 1000
Id = 41d5f7ea-6e02-4c91-8c25-5100e1240952             
            ");
            var command = new ChromecastCommand();
            //command.ShowSettings();
            command.RunCommand("hDz2F5EDZZI");         
            Console.ReadKey();
            command.Dispose();
        }       
    }
}

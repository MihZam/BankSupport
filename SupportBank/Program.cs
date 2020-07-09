using System;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;


namespace SupportBank
{
    class Program
    {
        
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget { FileName = @"C:\Work\Logs\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;

            var DM = new DataManager();
            
            // DM.readDataJSON(@"C:\Work\Training\SupportBank\Transactions2013.json");

            // DM.readDataCSV(@"C:\Work\Training\SupportBank\Transactions2014.csv");
            
            // DM.readDataCSV(@"C:\Work\Training\SupportBank\DodgyTransactions2015.csv");

            // DM.readDataXML(@"C:\Work\Training\SupportBank\Transactions2012.xml");

            // Creates all the different bank accounts
            var AM = new AccountsManager();
            foreach (var name in DM.getFrom())
            {
                AM.addAccount(name);
            }
            AM.applyTransactions(DM.getFrom(), DM.getTo(), DM.getAmount());
            
            var user = new UserInput();
            while (true)
            {
                user.askUser(AM, DM);
            }
        }
    }
}
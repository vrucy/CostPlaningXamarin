using System;
using System.IO;
//using System.IO;
using System.Reflection;
using Android;
using Android.Content;
using CostPlaningXamarin.Interfaces;
using Java.IO;
using NLog;
using NLog.Config;
using Xamarin.Forms;
using static Android.Manifest;

namespace CostPlaningXamarin.Services
{
    public class LogService : ILogService
    {
        public void Initialize(Assembly assembly, string assemblyName)
        {
            string resourcePrefix;
            if (Device.RuntimePlatform == Device.Android)
                resourcePrefix = "CostPlaningXamarin.Droid";
            else
                throw new Exception("Could not initialize Logger: Unknonw Platform");

            //var location = $"{assemblyName}.NLog.config";
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string location = $"{resourcePrefix}.NLog.config";
           
            Stream stream = assembly.GetManifestResourceStream(location);
            if (stream == null)
                throw new Exception($"The resource '{location}' was not loaded properly.");
            LogManager.Configuration = new XmlLoggingConfiguration(System.Xml.XmlReader.Create(stream), null);
        }
    }
}


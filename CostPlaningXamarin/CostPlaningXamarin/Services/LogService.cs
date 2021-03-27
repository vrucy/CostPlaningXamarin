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
                //test();
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string location = $"{resourcePrefix}.NLog.config";
           
            Stream stream = assembly.GetManifestResourceStream(location);
            if (stream == null)
                throw new Exception($"The resource '{location}' was not loaded properly.");
            LogManager.Configuration = new XmlLoggingConfiguration(System.Xml.XmlReader.Create(stream), null);
            var x = LogManager.Configuration.AllTargets;
        }
        private void test()
        {
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath + "/Log");

                dir.Mkdirs();
                Java.IO.File file = new Java.IO.File(dir, "log.txt");
                if (!file.Exists())
                {
                    file.CreateNewFile();
                    file.Mkdir();
                    FileWriter writer = new FileWriter(file);
                    // Writes the content to the file
                    writer.Write("ja");
                    writer.Flush();
                    writer.Close();
                }            
        }
    }
}


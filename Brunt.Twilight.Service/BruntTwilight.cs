﻿using Brunt.Twilight.API;
using Brunt.Twilight.Sky;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Brunt.Twilight.Service
{
    public partial class BruntTwilight : ServiceBase
    {
        public BruntTwilight()
        {
            InitializeComponent();
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(AssemblyDirectory + "\\appsettings.json"));

            eventLog = new EventLog();
            if (!EventLog.SourceExists(config.EventSource))
            {
                EventLog.CreateEventSource(config.EventSource, config.EventLog);
            }
            eventLog.Source = config.EventSource;
            eventLog.Log = config.EventLog;
        }

        Config config;
        EventLog eventLog;
        private int eventId = 1;
        private DateTime today;
        private Timer timer = new Timer();

        protected override void OnStart(string[] args)
        {
            Start();
        }

        public void OnTimer(object sender, ElapsedEventArgs args, bool isSunset)
        {
            eventLog.WriteEntry("Twilight!", EventLogEntryType.Information, eventId++);
            timer.Stop();
            timer.Elapsed -= (send, e) => this.OnTimer(send, e, isSunset);

            // Set BruntPosition
            var bruntClient = new BruntClient();
            var login = bruntClient.Login(new BruntLoginCredz() { ID = config.ID, PASS = config.PASS }).Result;
            if (login == null)
            {
                eventLog.WriteEntry("Login failed!", EventLogEntryType.Error, eventId++);
                return;
            }
            var devices = bruntClient.GetDevices().Result;
            if (devices == null)
            {
                eventLog.WriteEntry("Error getting devices!", EventLogEntryType.Error, eventId++);
                return;
            }
            foreach (var d in devices.Where(d=> config.Devices.Select(cd=>cd.Name.ToLower().Trim()).Contains(d.NAME.ToLower().Trim())))
            {
                var deviceConfig = config.Devices.SingleOrDefault(dc => dc.Name.ToLower().Trim() == d.NAME.ToLower().Trim());
                var changed = bruntClient.SetDevicePosition(
                    new BruntDevicePositionChange()
                    {
                        DeviceName = d.thingUri,
                        requestPosition = isSunset ? deviceConfig.SunsetPosition : deviceConfig.SunrisePosition
                    }).Result;

                if(changed == null)
                {
                    eventLog.WriteEntry("Error updating device position!", EventLogEntryType.Error, eventId++);
                    return;
                }
                else
                    eventLog.WriteEntry($"Changed {d.NAME} position to {(isSunset ? "sunset" : "sunrise")}", EventLogEntryType.Information, eventId++);
            }

            var twilightClient = GetSSClient();
            var twilightInfo = twilightClient.GetSunriseSunsetForDate().Result;
            var interval = twilightClient.GetIntervalTillNextTwilight(twilightInfo, today);

            eventLog.WriteEntry($"{(isSunset ? "Sunset" : "Sunrise")} Twilight in {interval.Item1} milliseconds.", EventLogEntryType.Information, eventId++);

            timer.Interval = interval.Item1;
            timer.Elapsed += (send, e) => this.OnTimer(send, e, interval.Item2);
            timer.Start();
        }

        public void Start()
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            var interval = GetNextTwilight();
            eventLog.WriteEntry($"{(interval.Item2 ? "Sunset" : "Sunrise")} Twilight in {interval.Item1} milliseconds.", EventLogEntryType.Information, eventId++);

            Timer timer = new Timer();
            timer.Interval = interval.Item1;
            timer.Elapsed += (sender,e) => this.OnTimer(sender, e, interval.Item2);
            timer.Start();

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog.WriteEntry("Brunt Twilight stopped.");
        }

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

        private SunsetSunriseClient GetSSClient()
        {
            today = DateTime.UtcNow.AddMinutes(5);
            // Get Sunset and Sunrise for day
            return new SunsetSunriseClient(
                config.TwilightUri,
                config.lng,
                config.Lat,
                today
                );
        }

        private Tuple<double,bool> GetNextTwilight()
        {
            var twilightClient = GetSSClient();

            var twilightInfo = twilightClient.GetSunriseSunsetForDate().Result;

            if (today > twilightInfo.sunrise && today > twilightInfo.sunset)
            {
                twilightClient._date = new DateTime(today.Year, today.Month, today.Day, 0, 0, 5).AddDays(1);
                twilightInfo = twilightClient.GetSunriseSunsetForDate().Result;
            }

            return twilightClient.GetIntervalTillNextTwilight(twilightInfo, today);
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}

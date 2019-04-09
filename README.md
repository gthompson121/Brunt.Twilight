# Brunt.Twilight
Service to set Brunt Engine Blind Position at sunrise/sunset based on Long/Lat

Project configuration is saved in the appsettings.json file:
```
{
  // Event log Source Name
  "EventSource": "BruntTwilightSource",
  // Event Log Name
  "EventLog": "BruntTwilightLog",
  // Brunt App EmailAddress or Phone Number
  "ID": "",
  // Brunt App Password
  "PASS": "",
  // Latitude position for sunrise/sunset
  "Lat": "55.953251",
  // Longtitude position for sunrise/sunset
  "Lng": "-3.188267",
  // Uri of the sunrise/sunset API
  "TwilightUri": "https://api.sunrise-sunset.org/json?lat={0}&lng={1}&date={2}&formatted=0",
  // List of Brunt Engine Devices to adjust
  "Devices": [
    {
	  // Device Name
      "Name": "{DEVICE NAME}",
	  // Position of the blind for sunrise
      "SunrisePosition": "0",
	  // Position of the blind for sunset
      "SunsetPosition": "100"
    },
    {
	   ...
    }
  ]
}
```

You can install the windows service using the command:
```
InstallUtil.exe Brunt.Twilight.Service.exe
```

Note, installutil.exe can be found in the C:\Windows\Microsoft.NET\Framework\{version}\InstallUtil.exe

You can uninstall the windows service using the command:
```
InstallUtil.exe /u Brunt.Twilight.Service.exe
```

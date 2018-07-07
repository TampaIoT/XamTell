using Windows.Devices.WiFi;
using XamarinTelloApp.Core.Interfaces;

namespace XamarinTelloApp.UWP.Services
{
    public class WiFiConnection : IWiFiConnection
    {
        WiFiAvailableNetwork _network;
        public WiFiConnection(WiFiAvailableNetwork network)
        {
            _network = network;
        }

        public string SSID => _network.Ssid;

        public byte Signal => _network.SignalBars;
    }
}

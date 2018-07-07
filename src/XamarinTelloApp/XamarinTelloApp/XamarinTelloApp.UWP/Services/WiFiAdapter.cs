using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core;
using LagoVista.Core.UWP.Services;
using LagoVista.Core.Validation;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using XamarinTelloApp.Core.Interfaces;

namespace XamarinTelloApp.UWP.Services
{
    public class WiFiAdapter : IWiFiAdapter
    {
        private bool _isConnected = false;
        IDispatcherServices _dispatcherService;

        public Windows.Devices.WiFi.WiFiAdapter _wifiAdapter { get; private set; }
        
        public event EventHandler<IWiFiConnection> Connected;
        public event EventHandler Disconnected;

        public WiFiAdapter(IDispatcherServices dispatcherService )
        {
            _dispatcherService = dispatcherService;
        }

        public async Task<bool> CheckAuthroizationAsync()
        {
            var access = await Windows.Devices.WiFi.WiFiAdapter.RequestAccessAsync();
            return access == WiFiAccessStatus.Allowed;
        }

        public async Task<InvokeResult> InitAsync()
        {
            var wifiAdapterResults = await DeviceInformation.FindAllAsync(Windows.Devices.WiFi.WiFiAdapter.GetDeviceSelector());
            if (wifiAdapterResults.Count >= 1)
            {
                _wifiAdapter = await Windows.Devices.WiFi.WiFiAdapter.FromIdAsync(wifiAdapterResults[0].Id);


                _wifiAdapter.AvailableNetworksChanged += _wifiAdapter_AvailableNetworksChanged;
                return await RefreshAdaptersAsync();
            }
            else
            {
                return InvokeResult.FromError("No wireless connections");
            }
        }

        private async void _wifiAdapter_AvailableNetworksChanged(Windows.Devices.WiFi.WiFiAdapter sender, object args)
        {
            await RefreshAdaptersAsync();
        }

        public async Task<InvokeResult> RefreshAdaptersAsync()
        {
            if (!_isConnected)
            {
                await _wifiAdapter.ScanAsync();
                foreach (var networks in _wifiAdapter.NetworkReport.AvailableNetworks)
                {
                    if (networks.Ssid.StartsWith("TELLO"))
                    {
                        await _wifiAdapter.ConnectAsync(networks, WiFiReconnectionKind.Automatic);
                        _dispatcherService.Invoke(() => Connected?.Invoke(this, new WiFiConnection(networks)));
                        Debug.WriteLine("CONNEcTED TO TELLO!");
                        _isConnected = true;
                        return InvokeResult.Success;
                    }
                }
            }

            _dispatcherService.Invoke(() => Disconnected?.Invoke(this, null));
            return InvokeResult.FromError("Could not connected"); 
        }


        public Task<InvokeResult> ConnectAsync(IWiFiConnection connection)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IWiFiConnection>> GetAvailableConnections()
        {
            throw new NotImplementedException();
        }
    }
}

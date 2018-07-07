using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Commanding;
using LagoVista.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using XamarinTelloApp.Core.Interfaces;

namespace XamarinTelloApp.Core.ViewModels
{
    public class MainViewModel : AppViewModelBase
    {
        public IWiFiAdapter _wifiAdapter { get; private set; }

        private bool _running;

        readonly RelayCommand _takeoffCommand;
        readonly RelayCommand _landCommand;
        readonly RelayCommand _refreshNetworks;

        public MainViewModel(IWiFiAdapter wifiAdapter)
        {
            _wifiAdapter = wifiAdapter;
            _wifiAdapter.Connected += _wifiAdapter_Connected;
            _landCommand = new RelayCommand(Land);
            _takeoffCommand = new RelayCommand(Takeoff);
            _refreshNetworks = new RelayCommand(RefreshNetworks);
        }

        private void _wifiAdapter_Connected(object sender, IWiFiConnection e)
        {
            StartListener();
        }

        public async override Task InitAsync()
        {
            await base.InitAsync();
            if(await _wifiAdapter.CheckAuthroizationAsync())
            {
                if((await _wifiAdapter.InitAsync()).Successful)
                {

                }
            }
        }

        private async Task SendCommand(String cmd)
        {
            using (UdpClient udpClient = new UdpClient(8889))
            {
                var msg = System.Text.ASCIIEncoding.ASCII.GetBytes(cmd);
                await udpClient.SendAsync(msg, msg.Length, "192.168.10.1", 8889);
                Debug.WriteLine($"Sending command [{cmd}] {msg.Length}");
            }
        }

        private void StartListener()
        {
            lock (this)
            {
                if (_running)
                {
                    return;
                }

                _running = true;
            }

            Task.Run(async () =>
            {
                 await SendCommand("command");

                var udpClient = new UdpClient(8888);
                while (true)
                {                    
                    var result = await udpClient.ReceiveAsync();
                    Debug.WriteLine("Recieved something.");
                    Debug.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(result.Buffer));
                }
            });
        }



        public async void Land()
        {
            await SendCommand("land");
        }

        public async void Takeoff()
        {
            await SendCommand("takeoff");
        }

        public async void RefreshNetworks()
        {
            await _wifiAdapter.RefreshAdaptersAsync();
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set => Set(ref _isConnected, value);
        }

        public RelayCommand TakeoffCommand => _takeoffCommand;
        public RelayCommand LandCommand => _landCommand;
        public RelayCommand RefreshNetworksCommand => _refreshNetworks;
    }
}

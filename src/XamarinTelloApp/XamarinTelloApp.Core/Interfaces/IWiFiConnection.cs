using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTelloApp.Core.Interfaces
{
    public interface IWiFiConnection
    {
        String SSID { get; }
        byte Signal {get;}
    }
}

using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinTelloApp.Core.Interfaces
{
    public interface IWiFiAdapter
    {
        Task<bool> CheckAuthroizationAsync();
        event EventHandler<IWiFiConnection> Connected;
        event EventHandler Disconnected;

        Task<IEnumerable<IWiFiConnection>> GetAvailableConnections();
        Task<InvokeResult> ConnectAsync(IWiFiConnection connection);
        Task<InvokeResult> InitAsync();
        Task<InvokeResult> RefreshAdaptersAsync();
    }
}

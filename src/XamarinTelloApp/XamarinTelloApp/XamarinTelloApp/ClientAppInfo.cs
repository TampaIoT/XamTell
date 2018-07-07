using LagoVista.Client.Core;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinTelloApp.Core.ViewModels;

namespace XamarinTelloApp
{
    public class ClientAppInfo : IClientAppInfo
    {
        public Type MainViewModel => typeof(MainViewModel);
    }
}

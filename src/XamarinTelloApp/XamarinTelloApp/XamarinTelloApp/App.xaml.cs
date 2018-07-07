#define ENV_MASTER

using LagoVista.Client.Core;
using LagoVista.Client.Core.Models;
using LagoVista.Client.Core.ViewModels;
using LagoVista.Client.Core.ViewModels.Auth;
using LagoVista.Core.Interfaces;
using LagoVista.Core.IOC;
using LagoVista.Core.ViewModels;
using LagoVista.XPlat.Core.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTelloApp.Core.ViewModels;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace XamarinTelloApp
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            MainPage = new MainPage();

#if ENV_STAGE
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "api.nuviot.com",
            };
#elif ENV_DEV
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "dev-api.nuviot.com",
            };
#elif ENV_LOCALDEV
            var serverInfo = new ServerInfo()
            {
                SSL = false,
                RootUrl = "localhost:5001",
            };
#elif ENV_MASTER
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "api.nuviot.com",
            };
#endif
            var clientAppInfo = new ClientAppInfo();

            var _appConfig = new AppConfig();
            SLWIOC.RegisterSingleton<IClientAppInfo>(clientAppInfo);
            SLWIOC.RegisterSingleton<IAppConfig>(_appConfig);


            var navigation = new ViewModelNavigation(this);
            LagoVista.XPlat.Core.Startup.Init(this, navigation);
            LagoVista.Client.Core.Startup.Init(serverInfo);

            navigation.Add<MainViewModel, Views.MainPage>();
            navigation.Add<SplashViewModel, Views.SplashView>();

            navigation.Start<SplashViewModel>();            

            SLWIOC.RegisterSingleton<IViewModelNavigation>(navigation);

        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

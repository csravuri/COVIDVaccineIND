using COVIDVaccineIND.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COVIDVaccineIND
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            NavigationService navigationService = new NavigationService();
            navigationService.SetAsMainPage(new Views.LoadingResources(navigationService));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

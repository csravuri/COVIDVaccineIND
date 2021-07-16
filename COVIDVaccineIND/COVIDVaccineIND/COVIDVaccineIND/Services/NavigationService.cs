using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace COVIDVaccineIND.Services
{
    public class NavigationService
    {
        private INavigation navigation;
        public NavigationService(INavigation navigation)
        {
            this.navigation = navigation;
        }
        public NavigationService()
        {
        }
        public void SetAsMainPage(Page page)
        {
            Application.Current.MainPage = new NavigationPage(page)
            {
                BarBackgroundColor = Color.FromHex("#a71cfc")
            };

            navigation = page.Navigation;
        }

        internal void SetNavigation(INavigation navigation)
        {
            this.navigation = navigation;
        }

        public async Task PushAsync(Page page)
        {
            await navigation.PushAsync(page);
        }

        public async Task PopAsync()
        {
            await navigation.PopAsync();
        }

        public async Task PopToRootAsync()
        {
            await navigation.PopToRootAsync();
        }
    }
}

using COVIDVaccineIND.Database;
using COVIDVaccineIND.Services;
using COVIDVaccineIND.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace COVIDVaccineIND.ViewModels
{
    public class LoadingResourcesViewModel
    {
        private readonly NavigationService navigationService;

        public ICommand LoadingResourcesCommand { get; private set; }


        public LoadingResourcesViewModel(NavigationService navigationService)
        {
            LoadingResourcesCommand = new Command(() => ExecuteLoadingResourcesCommand());
            this.navigationService = navigationService;
        }

        private void ExecuteLoadingResourcesCommand()
        {
            DBConnect dBConnect = DBConnect.GetDBConnect(DBCreated);

            if (dBConnect.IsDBCreated)
            {
                DBCreated();
            }
        }

        private async void DBCreated()
        {
            navigationService.SetAsMainPage(new VaccineSearchResult(navigationService));   
        }
    }
}

using COVIDVaccineIND.Services;
using COVIDVaccineIND.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COVIDVaccineIND.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingResources : ContentPage
    {
        private LoadingResourcesViewModel viewModel;

        public LoadingResources(NavigationService navigationService)
        {
            InitializeComponent();
            navigationService.SetNavigation(Navigation);
            BindingContext = this.viewModel = new LoadingResourcesViewModel(navigationService);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadingResourcesCommand.Execute(null);
        }
    }
}
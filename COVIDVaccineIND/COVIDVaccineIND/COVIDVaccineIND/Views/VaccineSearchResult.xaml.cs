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
    public partial class VaccineSearchResult : ContentPage
    {
        private VaccineSearchResultViewModel viewModel;

        public VaccineSearchResult(NavigationService navigationService)
        {
            InitializeComponent();
            BindingContext = viewModel = new VaccineSearchResultViewModel(navigationService);
        }

        private void PinCode_Completed(object sender, EventArgs e)
        {
            viewModel.SearchClickedCommand.Execute(null);
        }

        private void Session_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            viewModel.SessionSelectedCommand.Execute(e.SelectedItem);
            
            if (sender is ListView listView)
            {
                listView.SelectedItem = null;
            }
        }
    }
}
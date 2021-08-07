using COVIDVaccineIND.Models;
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
    public partial class VaccineDetails : ContentPage
    {
        private readonly Session session;

        public VaccineDetails(Session session)
        {
            InitializeComponent();
            BindingContext = this.session = session;
        }

        private async void GoToBooking_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VaccineRegistration());
        }
    }
}
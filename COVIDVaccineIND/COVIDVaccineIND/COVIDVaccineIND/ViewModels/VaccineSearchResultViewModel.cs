using COVIDVaccineIND.Models;
using COVIDVaccineIND.Services;
using COVIDVaccineIND.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace COVIDVaccineIND.ViewModels
{
    public class VaccineSearchResultViewModel : BindableObject
    {
        public ICommand SearchClickedCommand { get; private set; }
        public ICommand SessionSelectedCommand { get; private set; }

        private BindableProperty fromDateProperty = BindableProperty.Create(nameof(FromDate), typeof(DateTime), typeof(VaccineSearchResultViewModel), DateTime.Today);
        private BindableProperty toDateProperty = BindableProperty.Create(nameof(ToDate), typeof(DateTime), typeof(VaccineSearchResultViewModel), DateTime.Today.AddDays(7));
        private BindableProperty pinCodeProperty = BindableProperty.Create(nameof(PinCode), typeof(string), typeof(VaccineSearchResultViewModel), string.Empty);
        private BindableProperty stausMsgProperty = BindableProperty.Create(nameof(StatusMsg), typeof(string), typeof(VaccineSearchResultViewModel), string.Empty);
        private BindableProperty isLoadingProperty = BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(VaccineSearchResultViewModel), true);
        private BindableProperty isLoadedProperty = BindableProperty.Create(nameof(IsLoaded), typeof(bool), typeof(VaccineSearchResultViewModel), false);
        private readonly NavigationService navigationService;

        public DateTime FromDate
        {
            get => (DateTime)GetValue(fromDateProperty);
            set => SetValue(fromDateProperty, value);
        }
        public DateTime ToDate
        {
            get => (DateTime)GetValue(toDateProperty);
            set => SetValue(toDateProperty, value);
        }
        public string PinCode
        {
            get => (string)GetValue(pinCodeProperty);
            set => SetValue(pinCodeProperty, value);
        }
        public string StatusMsg
        {
            get => (string)GetValue(stausMsgProperty);
            set
            {
                SetValue(stausMsgProperty, value);
                IsLoading = !string.IsNullOrWhiteSpace(value);
            }
        }
        public bool IsLoading
        {
            get => (bool)GetValue(isLoadingProperty);
            set
            {
                SetValue(isLoadingProperty, value);
                IsLoaded = !value;
            }
        }
        public bool IsLoaded
        {
            get => (bool)GetValue(isLoadedProperty);
            set => SetValue(isLoadedProperty, value);
        }
        public ObservableCollection<Session> Sessions { get; private set; }

        public string DateFormat { get; } = "dd-MMM-yyyy";

        public VaccineSearchResultViewModel(NavigationService navigationService)
        {
            SearchClickedCommand = new Command(async () => await ExecuteSearchClickedCommand());
            SessionSelectedCommand = new Command<Session>(async (session) => await ExecuteSessionSelectedCommand(session));
            Sessions = new ObservableCollection<Session>();
            this.navigationService = navigationService;
            StatusMsg = "Enter Indian Pincode and select dates.";
        }

        private async Task ExecuteSessionSelectedCommand(Session session)
        {
            if (session != null)
            {
                await navigationService.PushAsync(new VaccineDetails(session));
            }
        }

        private async Task ExecuteSearchClickedCommand()
        {
            Sessions.Clear();
            if (IsValid())
            {
                StatusMsg = "Please wait while fetching slots.";
                FetchVaccineSlots();
            }
        }

        private async void FetchVaccineSlots()
        {
            CoWinService service = new CoWinService();
            try
            {
                CoWinResponseModel response = await service.GetVaccineSlots(PinCode, FromDate, ToDate);

                if (response != null)
                {
                    foreach (Session session in response.sessions)
                    {
                        Sessions.Add(session);
                    }
                }

                if (Sessions.Count == 0)
                {
                    StatusMsg = "No slots available.";
                }
                else
                {
                    StatusMsg = string.Empty;
                }
            }
            catch
            {
                StatusMsg = "Something went wrong please try again later.";
            }
        }

        private bool IsValid()
        {
            if (FromDate <= DateTime.Today.AddDays(-1))
            {
                StatusMsg = "From date cannot be older.";
                return false;
            }

            if (ToDate - FromDate > TimeSpan.FromDays(30))
            {
                StatusMsg = "Date range cannot be more than 30.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(PinCode))
            {
                StatusMsg = "PinCode is required.";
                return false;
            }

            if (FromDate > ToDate)
            {
                StatusMsg = "From date cannot be more than To date.";
                return false;
            }

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Phoneword
{
    public partial class MainPage : ContentPage
    {

        Entry phonewordEntry;
        Button buttonTranslate, buttonCall;
        string translated = "";
        const string callString = "Call";

        public MainPage()
        {

            Label label = new Label
            {
                Text = "Enter a Phoneword",
            };

            phonewordEntry = new Entry
            {
                Text = "1-855-XAMARIN"
            };

            buttonTranslate = new Button
            {
                Text = "Translate",
            };

            buttonTranslate.Clicked += OnTranslate;

            buttonCall = new Button
            {
                Text = callString,
                IsEnabled = false
            };

            buttonCall.Clicked += OnCall;

            StackLayout stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 15,
                Children = { label, phonewordEntry, buttonTranslate, buttonCall }
            };

            this.Content = stackLayout;


            // InitializeComponent();   This loads from XAML which isn;t what I want to do here
        }

        private void OnTranslate(object Sender, EventArgs args)
        {
            string enteredNumber = phonewordEntry.Text;

            translated = Core.PhonewordTranslator.ToNumber(enteredNumber);

            if (!string.IsNullOrEmpty(translated))
            {
                // Display the number on the call button and enable 
                buttonCall.Text = callString + " " + translated;
                buttonCall.IsEnabled = true;
            }
            else
            {
                // Disable the call button and display text "Call"
                buttonCall.Text = callString;
                buttonCall.IsEnabled = false;
            }
        }


        // asynchrolous method. 
        async void OnCall(object Sender, EventArgs args)
        {
            if (await this.DisplayAlert("Dial a Number", "Would you like to call " + translated, "Yes", "No"))
            {
                // dial
                // Uses DependencyService to get the implementation of the IDialer depending on the platform. 
                // note that each concrete implementation of "PhoneDialer" class has a "[assembly: Dependency(typeof(PhoneDialer))]" which has declared the PhoneDialer
                // as a dependency. We are now getting that dependency 
                var dialer = DependencyService.Get<IDialer>();
                if (dialer != null)
                {
                    await dialer.DialAsync(translated);
                }

            }

        }
    }
}

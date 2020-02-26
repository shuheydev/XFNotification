using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XFNotification
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        INotificationManager notificationManager;
        int notificationNumber = 0;

        public MainPage()
        {
            InitializeComponent();

            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArg)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }

        private void ShowNotification(string title, string message)
        {
            MainThread.BeginInvokeOnMainThread(()=> {
                var msg = new Label()
                {
                    Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
                };
                stackLayout.Children.Add(msg);
            });
        }

        private void OnScheduleClick(object sender, EventArgs e)
        {
            notificationNumber++;
            string title = $"Local Notification #{notificationNumber}";
            string message = $"You have now receive {notificationNumber} nogirications";
            notificationManager.ScheduleNotification(title, message);
        }
    }
}

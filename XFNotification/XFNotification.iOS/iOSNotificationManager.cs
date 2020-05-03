using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Foundation;
using UIKit;
using UserNotifications;

[assembly: Xamarin.Forms.Dependency(typeof(XFNotification.iOS.iOSNotificationManager))]
namespace XFNotification.iOS
{
    public class iOSNotificationManager : INotificationManager
    {
        int _messageId = -1;

        bool _hasNotificationsPermission;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            //Request the permission to use local notifications.
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
             {
                 _hasNotificationsPermission = approved;
             });
        }

        public int ScheduleNotification(string title, string message)
        {
            if (!_hasNotificationsPermission)
            {
                return -1;
            }

            _messageId++;

            var content = new UNMutableNotificationContent()
            {
                Title = title,
                Subtitle = "",
                Body = message,
                Badge = 1,
            };

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.25, false);

            var request = UNNotificationRequest.FromIdentifier(_messageId.ToString(), content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    throw new Exception($"Failed to schedule notification: {err}");
                }
            });

            return _messageId;
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArg()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }
    }
}
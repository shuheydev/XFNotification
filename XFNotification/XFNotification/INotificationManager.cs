using System;
using System.Collections.Generic;
using System.Text;

namespace XFNotification
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;

        void Initialize();

        int ScheduleNotification(string title, string message);
        void ReceiveNotification(string title, string message);
    }

    public class NotificationEventArg:EventArgs
    {
        public string Title { get; set; }
        public string  Message { get; set; }

    }
}

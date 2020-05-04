using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using System;
using AndroidApp = Android.App.Application;

[assembly: Xamarin.Forms.Dependency(typeof(XFNotification.Droid.AndroidNotificationManager))]
namespace XFNotification.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        readonly string _channelId = "default";
        readonly string _channelName = "Default";
        readonly string _channelDescription = "The default channel for notifications.";
        readonly int _pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool _channelInitialized = false;
        int _messageId = -1;
        NotificationManager _manager;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            CreateNotificationChannel();
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

        public int ScheduleNotification(string title, string message)
        {
            if (!_channelInitialized)
            {
                CreateNotificationChannel();
            }

            _messageId++;

            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, _pendingIntentId, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, _channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.icon))
                .SetSmallIcon(Resource.Drawable.icon)
                .SetAutoCancel(true)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            var notification = builder.Build();
            _manager.Notify(_messageId, notification);

            return _messageId;
        }

        private void CreateNotificationChannel()
        {
            _manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(_channelName);
                var channel = new NotificationChannel(_channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = _channelDescription
                };
                _manager.CreateNotificationChannel(channel);
            }

            _channelInitialized = true;
        }
    }
}
using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.Core.App;
using Xamarin.Forms;
using CovTracer.Class.Messaging;
using Android.Content;
using CovTracer.Droid.BroadcastReceivers;

namespace CovTracer.Droid
{
    [Activity(Label = "CovTracer", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static readonly int NOTIFICATION_ID = 1000;
        static readonly string CHANNEL_ID = "location_notification";
        internal static readonly string COUNT_KEY = "count";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            SubscribeAlarmMessaging();
            CreateNotificationChannel();
            ShowNotif();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            //var name = Resources.GetString(Resource.String.channel_name);
            //var description = GetString(Resource.String.channel_description);
            var channel = new NotificationChannel(CHANNEL_ID, "CovTracer", NotificationImportance.Default)
            {
                Description = "Hello"
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        void ShowNotif()
        {
            // Build the notification:
            var builder = new NotificationCompat.Builder(this, CHANNEL_ID)
                          .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                          .SetContentTitle("Button Clicked") // Set the title
                          .SetSmallIcon(Resource.Drawable.icon_about)
                          .SetContentText($"Hello!"); // the message to display.


            // Finally, publish the notification:
            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(NOTIFICATION_ID, builder.Build());
        }

        private void StartAlarm(int intervalInMilli)
        {
            try
            {
                AlarmManager manager = (AlarmManager)GetSystemService(Context.AlarmService);
                Intent myIntent;
                PendingIntent pendingIntent;

                myIntent = new Intent(this, typeof(AlarmBroadcastReceiver));
                pendingIntent = PendingIntent.GetBroadcast(this, 0, myIntent, PendingIntentFlags.Immutable);

                manager.SetRepeating(AlarmType.RtcWakeup, SystemClock.ElapsedRealtime() + intervalInMilli, intervalInMilli, pendingIntent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        private void SubscribeAlarmMessaging()
        {
            MessagingCenter.Subscribe<AlarmMessage>(this, "Start" + nameof(AlarmMessage), message =>
            {
                StartAlarm(message.TimeVal * 1000);
            });
        }
    }
}
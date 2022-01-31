using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using CovTracer.Droid.BroadcastReceivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CovTracer.Droid
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted })]
    public class BootBroadcastReceiver : BroadcastReceiver
    {
        static readonly int NOTIFICATION_ID = 1001;
        static readonly string CHANNEL_ID = "location_notification";
        internal static readonly string COUNT_KEY = "count";

        public override void OnReceive(Context context, Intent intent)
        {
            ShowNotif(context);

            StartAlarm(context,1);
        }

        void ShowNotif(Context context)
        {
            // Build the notification:
            var builder = new NotificationCompat.Builder(context, CHANNEL_ID)
                          .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                          .SetContentTitle("Reboot") // Set the title
                          .SetSmallIcon(Resource.Drawable.icon_about)
                          .SetContentText($"Hello I started!"); // the message to display.


            // Finally, publish the notification:
            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(NOTIFICATION_ID, builder.Build());
        }

        private void StartAlarm(Context context,int intervalInMilli)
        {
            try
            {
                
                AlarmManager manager = context.GetSystemService(Context.AlarmService) as AlarmManager;
                Intent myIntent;
                PendingIntent pendingIntent;

                myIntent = new Intent(context, typeof(AlarmBroadcastReceiver));
                pendingIntent = PendingIntent.GetBroadcast(context, 0, myIntent, PendingIntentFlags.Immutable);

                manager.SetRepeating(AlarmType.RtcWakeup, SystemClock.ElapsedRealtime() + intervalInMilli, intervalInMilli, pendingIntent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
    }
}
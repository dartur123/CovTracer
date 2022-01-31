using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CovTracer.Droid.BroadcastReceivers
{
    [BroadcastReceiver]
    public class AlarmBroadcastReceiver : BroadcastReceiver
    {
        static readonly int NOTIFICATION_ID = 1002;
        static readonly string CHANNEL_ID = "location_notification";
        internal static readonly string COUNT_KEY = "count";
        public override void OnReceive(Context context, Intent intent)
        {
            ShowNotif(context);
        }


        void ShowNotif(Context context)
        {
            // Build the notification:
            var builder = new NotificationCompat.Builder(context, CHANNEL_ID)
                          .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                          .SetContentTitle("Alarm") // Set the title
                          .SetSmallIcon(Resource.Drawable.icon_about)
                          .SetContentText($"this is an alarm! " + DateTime.Now); // the message to display.


            // Finally, publish the notification:
            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(NOTIFICATION_ID, builder.Build());
        }

    }
}
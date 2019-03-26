using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Android.Content;

namespace FCMClient
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        static readonly string TAG = "MainActivity";

        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        TextView msgText;

        protected override void OnCreate(Bundle bundle)
        {
            Log.Debug(TAG, "google app id: " + GetString(Resource.String.common_google_play_services_update_title));
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);
            msgText = FindViewById<TextView>(Resource.Id.msgText);

            IsPlayServicesAvailable();

            CreateNotificationChannel();

            var logTokenButton = FindViewById<Button>(Resource.Id.logTokenButton);
            logTokenButton.Click += delegate
            {
                Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token);

                //if (Intent.Extras != null)
                //{
                //    foreach (var key in Intent.Extras.KeySet())
                //    {
                //        var value = Intent.Extras.GetString(key);
                //        Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                //    }
                //}
            };
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    msgText.Text = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                msgText.Text = "Google Play Services is available.";
                return true;
            }
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

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
            //notificationManager.NotificationPolicy{ get;JavaSet }
            //notificationManager(new Intent(Android.se.NOTIFICATION_POLICY_ACCESS_SETTINGS), 0);

            //NotificationManager aNotificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            //aNotificationManager.SetInterruptionFilter(NotificationManager.ImportanceNone);

            NotificationManager mNotificationManager = (NotificationManager)GetSystemService(Context.NotificationService);

            // Check if the notification policy access has been granted for the app.
            if (!mNotificationManager.IsNotificationPolicyAccessGranted)
            {
                Intent intent = new Intent(Android.Provider.Settings.ActionNotificationPolicyAccessSettings);
                StartActivity(intent);
            }
        }
    }
}
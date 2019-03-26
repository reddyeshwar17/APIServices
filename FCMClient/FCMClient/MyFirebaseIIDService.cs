using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using Firebase.Messaging;

namespace FCMClient
{
    [Service(Name = "Xamarin.NotificationUpdate.MyFirebaseIIDService")]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    [IntentFilter(new[] { global::Android.Content.Intent.ActionBootCompleted })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            // SendRegistrationToServer(refreshedToken);
        }
        void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.
        }
    }

    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            string title = message.GetNotification().Title;
            string text = message.GetNotification().Body;
            string image = message.GetNotification().Icon;
            string sound = message.GetNotification().Sound;

            // TODO: Upgrade Notifier to include sound if needed
            // DependencyService.Get<INotifier>().Notify(title, text, image);

            base.OnMessageReceived(message);
        }

        public override void OnMessageSent(string msgId)
        {
            base.OnMessageSent(msgId);
        }
    }
}
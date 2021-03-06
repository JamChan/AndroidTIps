﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Android.OS;
using Android.App;
using Android.Views;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Android.Support.V4.App;

using Debug = System.Diagnostics.Debug;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;

namespace AndroidTips
{
	[Activity (Label = "LocalNotificationActivity")]			
	public class LocalNotificationActivity : Activity
	{
		private static readonly int ButtonClickNotificationId = 1000;



		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your application here
			SetContentView( Resource.Layout.LocalNotificationView );

			var txtMessage = FindViewById<EditText> (Resource.Id.txtMessage);
			var btnSend = FindViewById<Button> (Resource.Id.btnSend );

			btnSend.Click += (object sender, EventArgs e) => {

				//
				Bundle valuesForActivity = new Bundle();
				valuesForActivity.PutString ("message", txtMessage.Text );

				Intent resultIntent = new Intent(this, typeof (NotificationProcessingActivity));
				resultIntent.PutExtras (valuesForActivity);

				// Construct a back stack for cross-task navigation:
				TaskStackBuilder stackBuilder = TaskStackBuilder.Create (this);
				stackBuilder.AddParentStack (
					Java.Lang.Class.FromType(typeof(NotificationProcessingActivity )));
				stackBuilder.AddNextIntent (resultIntent);

				// Create the PendingIntent with the back stack:            
				PendingIntent resultPendingIntent = 
					stackBuilder.GetPendingIntent (0, (int)PendingIntentFlags.UpdateCurrent);
				

				//

				NotificationCompat.Builder builder = new NotificationCompat.Builder (this)
					.SetAutoCancel (true)                    // 
					.SetContentIntent (resultPendingIntent)  // 使用者按下通知後，負責處理的 Activity
					.SetContentTitle ("Android Tips")      // Title
					.SetNumber (1)                       // Display the count in the Content Info
					.SetSmallIcon(Resource.Drawable.ic_stat_button_click)  // Icon，必備
					.SetContentText (txtMessage.Text); // 要顯示的訊息

				// 註冊系統服務
				NotificationManager notificationManager = 
					(NotificationManager)GetSystemService(Context.NotificationService);
				
				notificationManager.Notify(ButtonClickNotificationId, builder.Build());


			};


		}
	}
}


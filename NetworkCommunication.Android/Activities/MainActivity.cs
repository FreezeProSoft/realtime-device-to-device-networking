using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using NetworkCommunication.Core;
using Android.Graphics;
using Android.Views.InputMethods;
using Android.Content.PM;

namespace NetworkCommunication.Android
{
    [Activity(Label = "Main", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait )]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.main_layout);

            var btnServer = FindViewById<Button>(Resource.Id.btnServer);

            var btnClient = FindViewById<Button>(Resource.Id.btnClient);


            btnServer.Click += (sender, e) => 
                {
                    StartActivity(typeof(ServerActivity));
                };

            btnClient.Click += (sender, e) => 
                {
                    StartActivity(typeof(ClientActivity));
                };
        }
    }
}



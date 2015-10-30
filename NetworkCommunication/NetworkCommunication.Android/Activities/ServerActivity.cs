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
    [Activity(Label = "Server", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ServerActivity : Activity, TextView.IOnEditorActionListener
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.server_layout);

            lblServerStatus = FindViewById<TextView>(Resource.Id.lblServerStatus);
			
            txtPort = FindViewById<EditText>(Resource.Id.txtPort);

            btnStart = FindViewById<Button>(Resource.Id.btnStart);

            txtMessages = FindViewById<TextView>(Resource.Id.txtMessages);

            btnStart.Click += BtnStart_Click;

            txtPort.SetOnEditorActionListener(this);

            NetworkServer.ReceiveMessage += NetworkServer_ReceiveMessage;

            NetworkServer.Started += NetworkServer_Started;

            NetworkServer.Stop += NetworkServer_Stop;
        }
            
        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkServer.ReceiveMessage -= NetworkServer_ReceiveMessage;

            NetworkServer.Started -= NetworkServer_Started;

            NetworkServer.Stop -= NetworkServer_Stop;
        }

        protected void NetworkServer_Stop (object sender, EventArgs e)
        {
            RunOnUiThread(() =>
                {
                    lblServerStatus.Text = "Stopped";

                    lblServerStatus.SetTextColor(Color.Red);

                    btnStart.Text = "Start";
                });
        }

        protected void NetworkServer_Started (object sender, EventArgs e)
        {
            RunOnUiThread(() =>
                {
                    lblServerStatus.Text = "Started";

                    lblServerStatus.SetTextColor(Color.Green);

                    btnStart.Text = "Stop";
                });
        }

        protected void BtnStart_Click (object sender, EventArgs e)
        {
            if (!NetworkServer.isRunning)
            {
                NetworkServer.TryStart("6000"); 
            }
            else
            {
                NetworkServer.TryStop();
            }
        }

        protected void NetworkServer_ReceiveMessage (object sender, string e)
        {
            RunOnUiThread(() =>
                {
                    txtMessages.Text = txtMessages.Text.Insert(0, e + "\n"); 
                });
        }

        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            if (actionId == ImeAction.Done && e.Action == KeyEventActions.Down)
            {
                var inputMethodManager = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);

                inputMethodManager.HideSoftInputFromWindow(v.WindowToken, 0);

                return true;
            }

            return false;
        }

        private TextView txtMessages;

        private TextView lblServerStatus;

        private EditText txtPort;

        private Button btnStart;
    }
}



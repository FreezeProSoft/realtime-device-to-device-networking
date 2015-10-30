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
    [Activity(Label = "Client", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ClientActivity : Activity, TextView.IOnEditorActionListener
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.client_layout);

            lblConnectStatus = FindViewById<TextView>(Resource.Id.lblConnectStatus);
			
            txtIPAddress = FindViewById<EditText>(Resource.Id.txtIPAddress);

            txtPort = FindViewById<EditText>(Resource.Id.txtPort);

            txtMessage = FindViewById<EditText>(Resource.Id.txtMessage);

            btnConnect = FindViewById<Button>(Resource.Id.btnConnect);

            txtIPAddress.SetOnEditorActionListener(this);

            txtPort.SetOnEditorActionListener(this);

            txtMessage.SetOnEditorActionListener(this);

            btnConnect.Click += BtnConnect_Click;

            NetworkClient.Connected += NetworkClient_Connected;

            NetworkClient.Disconnected += NetworkClient_Disconnected;
        }

        protected void NetworkClient_Connected (object sender, EventArgs e)
        {
            RunOnUiThread(() =>
                {
                    lblConnectStatus.Text = "Connected";

                    btnConnect.Text = "Disconnect";

                    lblConnectStatus.SetTextColor(Color.Green);
                });
        }

        protected void NetworkClient_Disconnected (object sender, EventArgs e)
        {
            RunOnUiThread(() =>
                {
                    lblConnectStatus.Text = "Disconnected";

                    btnConnect.Text = "Connect";

                    lblConnectStatus.SetTextColor(Color.Red);
                });
        }

        protected void BtnConnect_Click (object sender, EventArgs e)
        {
            if (!NetworkClient.isConnected)
            {
                NetworkClient.TryConnect(txtIPAddress.Text, txtPort.Text);
            }
            else
            {
                NetworkClient.TryDisconnect();
            }      
        }
            
        protected void SendMessage()
        {
            if (NetworkClient.isConnected)
            {
                NetworkClient.TrySendMessage(txtMessage.Text);

                txtMessage.Text = string.Empty;
            } 
        }
   
        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkClient.TryDisconnect();

            NetworkClient.Disconnected -= NetworkClient_Disconnected;

            NetworkClient.Connected -= NetworkClient_Connected;
        }
                 
        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            if (actionId == ImeAction.Done && e.Action == KeyEventActions.Down)
            {
                var inputMethodManager = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);

                inputMethodManager.HideSoftInputFromWindow(v.WindowToken, 0);

                if (v == txtMessage)
                {
                    SendMessage();
                }

                return true;
            }

            return false;
        }
            
        private TextView lblConnectStatus;

        private EditText txtPort;

        private EditText txtIPAddress; 

        private EditText txtMessage; 

        private Button btnConnect;
    }
}



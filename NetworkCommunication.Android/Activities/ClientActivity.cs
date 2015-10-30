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
using System.Text;

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


            socketClient = new SocketClient();

            socketClient.StateChanged += SocketClient_StateChanged;
        }

        protected void BtnConnect_Click (object sender, EventArgs e)
        {
            if (socketClient.State == SocketClientState.Disconnected)
            {
                int port = 6000;

                int.TryParse(txtPort.Text, out port);

                socketClient.Connect(txtIPAddress.Text, port);
            }
            else
            {
                socketClient.Disconnect();
            }      
        }

        protected void SocketClient_StateChanged (object sender, SocketClientState state)
        {
            RunOnUiThread(() =>
                {
                    switch (state)
                    {
                        case SocketClientState.Connected:

                            lblConnectStatus.Text = "Connected";

                            btnConnect.Text = "Disconnect";

                            lblConnectStatus.SetTextColor(Color.Green);

                            break;

                        case SocketClientState.Disconnected:

                            lblConnectStatus.Text = "Disconnected";

                            btnConnect.Text = "Connect";

                            lblConnectStatus.SetTextColor(Color.Red);

                            break;
                    }
                });
        }
            
        protected void SendMessage()
        {
            if (socketClient.State == SocketClientState.Connected)
            {
                socketClient.SendMessage(Encoding.ASCII.GetBytes(txtMessage.Text));

                txtMessage.Text = string.Empty;
            } 
        }
   
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (socketClient != null)
            {
                if (socketClient.State != SocketClientState.Disconnected)
                {
                    socketClient.Disconnect();
                }

                socketClient.StateChanged -= SocketClient_StateChanged;
            }
        }
                 
        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            if (actionId == ImeAction.Done)
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

        private SocketClient socketClient;
    }
}



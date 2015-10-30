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


            socketServer = new SocketServer();

            socketServer.StateChanged += SocketServer_StateChanged;

            socketServer.ReceivedMessage += SocketServer_ReceivedMessage;

            socketServer.HostAcceptConnection += SocketServer_HostAcceptConnection;

            socketServer.HostClosedConnection += SocketServer_HostClosedConnection;
        }

        protected void BtnStart_Click (object sender, EventArgs e)
        {
            if (socketServer.State == SocketServerState.Stopped)
            {
                int port = 6000;

                int.TryParse(txtPort.Text, out port);

                socketServer.Run(port); 
            }
            else
            {
                socketServer.Stop();
            }
        }

        protected void SocketServer_HostAcceptConnection (object sender, RemoteHost host)
        {
            AddMessage(string.Format("Host : {0} - Status : Connected ", host.Address));
        }

        protected void SocketServer_HostClosedConnection (object sender, RemoteHost host)
        {
            AddMessage(string.Format("Host : {0} - Status : Closed ", host.Address));
        }

        protected void SocketServer_ReceivedMessage (object sender, ReceiveMessageEventArgs e)
        {
            AddMessage(string.Format("Host : {0} - Message : {1} ", e.Host.Address, Encoding.ASCII.GetString(e.Message)));
        }

        protected void AddMessage (string message)
        {
            RunOnUiThread(() =>
                {
                    txtMessages.Text = txtMessages.Text.Insert(0, string.Format("{0}\n", message));
                });
        }

        protected void SocketServer_StateChanged (object sender, SocketServerState state)
        {
            RunOnUiThread(() =>
                {
                    switch (state)
                    {
                        case SocketServerState.Starting:

                            lblServerStatus.Text = "Starting";
                            
                            lblServerStatus.SetTextColor(Color.Green);
                            
                            btnStart.Text = "Stop";

                            break;

                        case SocketServerState.Running:

                            lblServerStatus.Text = "Running";

                            lblServerStatus.SetTextColor(Color.Green);

                            btnStart.Text = "Stop";

                            break;

                        case SocketServerState.Stopped:
                            
                            lblServerStatus.Text = "Stopped";

                            lblServerStatus.SetTextColor(Color.Red);

                            btnStart.Text = "Start";

                            break;
                    }
                });
        }
            
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (socketServer != null)
            {
                if (socketServer.State != SocketServerState.Stopped)
                {
                    socketServer.Stop();
                }

                socketServer.StateChanged -= SocketServer_StateChanged;

                socketServer.ReceivedMessage -= SocketServer_ReceivedMessage;

                socketServer.HostAcceptConnection -= SocketServer_HostAcceptConnection;

                socketServer.HostClosedConnection -= SocketServer_HostClosedConnection;
            }
        }
            
        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            if (actionId == ImeAction.Done)
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

        private SocketServer socketServer;
    }
}



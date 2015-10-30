using System;
using UIKit;
using NetworkCommunication.Core;
using System.Text;

namespace NetworkCommunication.iOS
{
	public partial class SendMassageController : UIViewController
	{
		public SendMassageController (IntPtr handle) : base (handle)
		{
            
		}
            
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            btnConnect.TouchUpInside += BtnConnect_TouchUpInside;

            txtIPAddress.ShouldReturn = SearchShouldReturn;

            txtPort.ShouldReturn = SearchShouldReturn;

            txtMessage.ShouldReturn = SearchShouldReturn;


            socketClient = new SocketClient();

            socketClient.StateChanged += SocketClient_StateChanged;
        }

        protected void BtnConnect_TouchUpInside (object sender, EventArgs e)
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

        protected void SendMessage()
        {
            if (socketClient.State == SocketClientState.Connected)
            {
                socketClient.SendMessage(Encoding.ASCII.GetBytes(txtMessage.Text));

                txtMessage.Text = string.Empty;
            }
        }

        protected void SocketClient_StateChanged (object sender, SocketClientState state)
        {
            InvokeOnMainThread(() =>
                {
                    switch (state)
                    {
                        case SocketClientState.Connected:

                            lblConnectStatus.Text = "Connected";

                            btnConnect.SetTitle("Disconnect", UIControlState.Normal);

                            lblConnectStatus.TextColor = UIColor.Green;

                            break;

                        case SocketClientState.Disconnected:

                            lblConnectStatus.Text = "Disconnected";

                            btnConnect.SetTitle("Connect", UIControlState.Normal);

                            lblConnectStatus.TextColor = UIColor.Red;

                            break;
                    }
                });
        }

        protected bool SearchShouldReturn (UITextField view)
        {
            view.ResignFirstResponder ();

            if (view == txtMessage)
            {
                SendMessage();
            }

            return true;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (socketClient != null)
            {
                if (socketClient.State != SocketClientState.Disconnected)
                {
                    socketClient.Disconnect();
                }

                socketClient.StateChanged -= SocketClient_StateChanged;
            }
        }

        private SocketClient socketClient;
	}
}

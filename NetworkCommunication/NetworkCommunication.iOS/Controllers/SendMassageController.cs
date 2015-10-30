using System;
using UIKit;
using NetworkCommunication.Core;

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
        }

        protected void BtnConnect_TouchUpInside (object sender, EventArgs e)
        {
            if (!NetworkClient.IsConnected)
            {
                if (NetworkClient.TryConnect(txtIPAddress.Text, txtPort.Text))
                {
                    lblConnectStatus.Text = "Connected";

                    btnConnect.SetTitle("Disconnect", UIControlState.Normal);

                    lblConnectStatus.TextColor = UIColor.Green;
                }
            }
            else
            {
                if (NetworkClient.TryDisconnect())
                {
                    lblConnectStatus.Text = "Disconnected";

                    btnConnect.SetTitle("Connect", UIControlState.Normal);

                    lblConnectStatus.TextColor = UIColor.Red;
                }
            }          
        }

        protected void SendMessage()
        {
            if (NetworkClient.TrySendMessage(txtMessage.Text))
            {
                txtMessage.Text = string.Empty;
            } 
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

            NetworkClient.TryDisconnect();
        }
	}
}

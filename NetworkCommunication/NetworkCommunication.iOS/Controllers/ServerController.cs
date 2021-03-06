// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using NetworkCommunication.Core;

namespace NetworkCommunication.iOS
{
	public partial class ServerController : UIViewController
	{
		public ServerController (IntPtr handle) : base (handle)
		{
            
		}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            btnStart.TouchUpInside += BtnStart_TouchUpInside;

            txtPort.ShouldReturn = SearchShouldReturn;

            NetworkServer.ReceiveMessage += NetworkServer_ReceiveMessage;

            NetworkServer.Started += NetworkServer_Started;

            NetworkServer.Stop += NetworkServer_Stop;
        }

        protected void NetworkServer_Stop (object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
                {
                    lblServerStatus.Text = "Stopped";

                    lblServerStatus.TextColor = UIColor.Red;

                    btnStart.SetTitle("Start", UIControlState.Normal);
                });
        }

        protected void NetworkServer_Started (object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
                {
                    lblServerStatus.Text = "Started";
            
                    lblServerStatus.TextColor = UIColor.Green;

                    btnStart.SetTitle("Stop", UIControlState.Normal);
                });
        }

        protected void BtnStart_TouchUpInside (object sender, EventArgs e)
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
            InvokeOnMainThread(() =>
                {
                    txtMessages.Text = txtMessages.Text.Insert(0, e + "\n"); 
                });
        }
            
        protected bool SearchShouldReturn (UITextField view)
        {
            view.ResignFirstResponder ();

            return true;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            NetworkServer.ReceiveMessage -= NetworkServer_ReceiveMessage;

            NetworkServer.Started -= NetworkServer_Started;

            NetworkServer.Stop -= NetworkServer_Stop;

            NetworkServer.TryStop();
        }
	}
}

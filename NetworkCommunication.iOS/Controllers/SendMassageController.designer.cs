// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NetworkCommunication.iOS
{
	[Register ("SendMassageController")]
	partial class SendMassageController
	{
		[Outlet]
		UIKit.UIButton btnConnect { get; set; }

		[Outlet]
		UIKit.UIButton btnSend { get; set; }

		[Outlet]
		UIKit.UILabel lblConnectStatus { get; set; }

		[Outlet]
		UIKit.UITextField txtIPAddress { get; set; }

		[Outlet]
		UIKit.UITextField txtMessage { get; set; }

		[Outlet]
		UIKit.UITextField txtPort { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnSend != null) {
				btnSend.Dispose ();
				btnSend = null;
			}

			if (txtMessage != null) {
				txtMessage.Dispose ();
				txtMessage = null;
			}

			if (lblConnectStatus != null) {
				lblConnectStatus.Dispose ();
				lblConnectStatus = null;
			}

			if (btnConnect != null) {
				btnConnect.Dispose ();
				btnConnect = null;
			}

			if (txtIPAddress != null) {
				txtIPAddress.Dispose ();
				txtIPAddress = null;
			}

			if (txtPort != null) {
				txtPort.Dispose ();
				txtPort = null;
			}
		}
	}
}

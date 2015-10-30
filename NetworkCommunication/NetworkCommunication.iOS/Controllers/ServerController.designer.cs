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
	[Register ("ServerController")]
	partial class ServerController
	{
		[Outlet]
		UIKit.UIButton btnStart { get; set; }

		[Outlet]
		UIKit.UILabel lblServerStatus { get; set; }

		[Outlet]
		UIKit.UITextView txtMessages { get; set; }

		[Outlet]
		UIKit.UITextField txtPort { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblServerStatus != null) {
				lblServerStatus.Dispose ();
				lblServerStatus = null;
			}

			if (txtPort != null) {
				txtPort.Dispose ();
				txtPort = null;
			}

			if (btnStart != null) {
				btnStart.Dispose ();
				btnStart = null;
			}

			if (txtMessages != null) {
				txtMessages.Dispose ();
				txtMessages = null;
			}
		}
	}
}

using System;
using UnityEngine.Networking;

namespace Smooth
{
	public class MsgType : UnityEngine.Networking.MsgType
    {
		public static short SmoothSyncFromServerToNonOwners = 32765;

		public static short SmoothSyncFromOwnerToServer = 32766;
	}
}

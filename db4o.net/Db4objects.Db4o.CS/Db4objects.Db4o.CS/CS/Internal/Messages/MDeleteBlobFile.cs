/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Types;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MDeleteBlobFile : MsgBlob, IServerSideMessage
	{
		public virtual void ProcessAtServer()
		{
			try
			{
				IBlob blob = this.ServerGetBlobImpl();
				if (blob != null)
				{
					blob.DeleteFile();
				}
			}
			catch (Exception)
			{
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public override void ProcessClient(Socket4Adapter sock)
		{
		}
		// nothing to do here
	}
}

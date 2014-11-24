/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MClassMeta : MsgObject, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			Unmarshall();
			try
			{
				lock (ContainerLock())
				{
					ClassInfo classInfo = (ClassInfo)ReadObjectFromPayLoad();
					ClassInfoHelper classInfoHelper = ServerMessageDispatcher().ClassInfoHelper();
					GenericClass genericClass = classInfoHelper.ClassMetaToGenericClass(Container().Reflector
						(), classInfo);
					if (genericClass != null)
					{
						Transaction trans = Container().SystemTransaction();
						ClassMetadata classMetadata = Container().ProduceClassMetadata(genericClass);
						if (classMetadata != null)
						{
							Container().CheckStillToSet();
							classMetadata.SetStateDirty();
							classMetadata.Write(trans);
							trans.Commit();
							StatefulBuffer returnBytes = Container().ReadStatefulBufferById(trans, classMetadata
								.GetID());
							return Msg.ObjectToClient.GetWriter(returnBytes);
						}
					}
				}
			}
			catch (Exception e)
			{
			}
			return Msg.Failed;
		}
	}
}

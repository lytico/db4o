/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.IO;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Extensions.Fixtures
{
	public class Db4oDefragSolo : Db4oSolo
	{
		protected override IObjectContainer CreateDatabase(IConfiguration config)
		{
			Sharpen.IO.File origFile = new Sharpen.IO.File(GetAbsolutePath());
			if (origFile.Exists())
			{
				try
				{
					string backupFile = GetAbsolutePath() + ".defrag.backup";
					IIdMapping mapping = new InMemoryIdMapping();
					// new
					// BTreeIDMapping(getAbsolutePath()+".defrag.mapping",4096,1,1000);
					DefragmentConfig defragConfig = new DefragmentConfig(GetAbsolutePath(), backupFile
						, mapping);
					defragConfig.ForceBackupDelete(true);
					// FIXME Cloning is ugly - wrap original in Decorator within
					// DefragContext instead?
					IConfiguration clonedConfig = (IConfiguration)((IDeepClone)config).DeepClone(null
						);
					defragConfig.Db4oConfig(clonedConfig);
					Db4objects.Db4o.Defragment.Defragment.Defrag(defragConfig, new _IDefragmentListener_30
						());
				}
				catch (IOException e)
				{
					Sharpen.Runtime.PrintStackTrace(e);
				}
			}
			return base.CreateDatabase(config);
		}

		private sealed class _IDefragmentListener_30 : IDefragmentListener
		{
			public _IDefragmentListener_30()
			{
			}

			public void NotifyDefragmentInfo(DefragmentInfo info)
			{
				Sharpen.Runtime.Err.WriteLine(info);
			}
		}

		public override bool Accept(Type clazz)
		{
			return base.Accept(clazz) && !typeof(IOptOutDefragSolo).IsAssignableFrom(clazz);
		}

		public override string Label()
		{
			return "Defrag-" + base.Label();
		}
	}
}

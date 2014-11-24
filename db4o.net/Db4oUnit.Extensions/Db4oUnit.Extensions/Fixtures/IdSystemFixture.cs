/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Internal.Ids;

namespace Db4oUnit.Extensions.Fixtures
{
	public class IdSystemFixture : Db4oSolo
	{
		private readonly byte _idSystemType;

		public IdSystemFixture(byte idSystemType)
		{
			_idSystemType = idSystemType;
		}

		public IdSystemFixture()
		{
			_idSystemType = StandardIdSystemFactory.StackedBtree;
		}

		protected override IObjectContainer CreateDatabase(IConfiguration config)
		{
			IEmbeddedConfiguration embeddedConfiguration = Db4oLegacyConfigurationBridge.AsEmbeddedConfiguration
				(config);
			switch (_idSystemType)
			{
				case StandardIdSystemFactory.PointerBased:
				{
					embeddedConfiguration.IdSystem.UsePointerBasedSystem();
					break;
				}

				case StandardIdSystemFactory.StackedBtree:
				{
					embeddedConfiguration.IdSystem.UseStackedBTreeSystem();
					break;
				}

				case StandardIdSystemFactory.InMemory:
				{
					embeddedConfiguration.IdSystem.UseInMemorySystem();
					break;
				}

				default:
				{
					throw new InvalidOperationException();
				}
			}
			// embeddedConfiguration.file().freespace().useBTreeSystem();
			return base.CreateDatabase(config);
		}

		public override string Label()
		{
			string idSystemType = string.Empty;
			switch (_idSystemType)
			{
				case StandardIdSystemFactory.PointerBased:
				{
					idSystemType = "PointerBased";
					break;
				}

				case StandardIdSystemFactory.StackedBtree:
				{
					idSystemType = "BTree";
					break;
				}

				case StandardIdSystemFactory.InMemory:
				{
					idSystemType = "InMemory";
					break;
				}

				default:
				{
					throw new InvalidOperationException();
				}
			}
			return "IdSystem-" + idSystemType + " " + base.Label();
		}

		public override bool Accept(Type clazz)
		{
			return base.Accept(clazz) && !typeof(IOptOutIdSystem).IsAssignableFrom(clazz);
		}
	}
}

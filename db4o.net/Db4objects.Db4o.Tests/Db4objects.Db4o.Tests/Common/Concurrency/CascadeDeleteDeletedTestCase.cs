/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class CascadeDeleteDeletedTestCase : Db4oClientServerTestCase
	{
		public class Item
		{
			public Item(string name)
			{
				this.name = name;
			}

			public string name;

			public object untypedMember;

			public CascadeDeleteDeletedTestCase.CddMember typedMember;
		}

		public static void Main(string[] args)
		{
			new CascadeDeleteDeletedTestCase().RunConcurrency();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupBeforeStore()
		{
			ConfigureThreadCount(10);
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(CascadeDeleteDeletedTestCase.Item)).CascadeOnDelete(true
				);
		}

		protected override void Store()
		{
			IExtObjectContainer oc = Db();
			MembersFirst(oc, "membersFirst commit");
			MembersFirst(oc, "membersFirst");
			TwoRef(oc, "twoRef");
			TwoRef(oc, "twoRef commit");
			TwoRef(oc, "twoRef delete");
			TwoRef(oc, "twoRef delete commit");
		}

		private void MembersFirst(IExtObjectContainer oc, string name)
		{
			CascadeDeleteDeletedTestCase.Item item = new CascadeDeleteDeletedTestCase.Item(name
				);
			item.untypedMember = new CascadeDeleteDeletedTestCase.CddMember();
			item.typedMember = new CascadeDeleteDeletedTestCase.CddMember();
			oc.Store(item);
		}

		private void TwoRef(IExtObjectContainer oc, string name)
		{
			CascadeDeleteDeletedTestCase.Item item1 = new CascadeDeleteDeletedTestCase.Item(name
				);
			item1.untypedMember = new CascadeDeleteDeletedTestCase.CddMember();
			item1.typedMember = new CascadeDeleteDeletedTestCase.CddMember();
			CascadeDeleteDeletedTestCase.Item item2 = new CascadeDeleteDeletedTestCase.Item(name
				);
			item2.untypedMember = item1.untypedMember;
			item2.typedMember = item1.typedMember;
			oc.Store(item1);
			oc.Store(item2);
		}

		public virtual void Conc(IExtObjectContainer oc, int seq)
		{
			if (seq == 0)
			{
				TMembersFirst(oc, "membersFirst commit");
			}
			else
			{
				if (seq == 1)
				{
					TMembersFirst(oc, "membersFirst");
				}
				else
				{
					if (seq == 2)
					{
						TTwoRef(oc, "twoRef");
					}
					else
					{
						if (seq == 3)
						{
							TTwoRef(oc, "twoRef commit");
						}
						else
						{
							if (seq == 4)
							{
								TTwoRef(oc, "twoRef delete");
							}
							else
							{
								if (seq == 5)
								{
									TTwoRef(oc, "twoRef delete commit");
								}
							}
						}
					}
				}
			}
		}

		public virtual void Check(IExtObjectContainer oc)
		{
			Assert.AreEqual(0, CountOccurences(oc, typeof(CascadeDeleteDeletedTestCase.CddMember
				)));
		}

		private void TMembersFirst(IExtObjectContainer oc, string name)
		{
			bool commit = name.IndexOf("commit") > 1;
			IQuery q = oc.Query();
			q.Constrain(typeof(CascadeDeleteDeletedTestCase.Item));
			q.Descend("name").Constrain(name);
			IObjectSet objectSet = q.Execute();
			CascadeDeleteDeletedTestCase.Item cdd = (CascadeDeleteDeletedTestCase.Item)objectSet
				.Next();
			oc.Delete(cdd.untypedMember);
			oc.Delete(cdd.typedMember);
			if (commit)
			{
				oc.Commit();
			}
			oc.Delete(cdd);
			if (!commit)
			{
				oc.Commit();
			}
		}

		private void TTwoRef(IExtObjectContainer oc, string name)
		{
			bool commit = name.IndexOf("commit") > 1;
			bool delete = name.IndexOf("delete") > 1;
			IQuery q = oc.Query();
			q.Constrain(typeof(CascadeDeleteDeletedTestCase.Item));
			q.Descend("name").Constrain(name);
			IObjectSet objectSet = q.Execute();
			CascadeDeleteDeletedTestCase.Item item1 = (CascadeDeleteDeletedTestCase.Item)objectSet
				.Next();
			CascadeDeleteDeletedTestCase.Item item2 = (CascadeDeleteDeletedTestCase.Item)objectSet
				.Next();
			if (delete)
			{
				oc.Delete(item1.untypedMember);
				oc.Delete(item1.typedMember);
			}
			oc.Delete(item1);
			if (commit)
			{
				oc.Commit();
			}
			oc.Delete(item2);
			if (!commit)
			{
				oc.Commit();
			}
		}

		public class CddMember
		{
			public string name;
		}
	}
}
#endif // !SILVERLIGHT

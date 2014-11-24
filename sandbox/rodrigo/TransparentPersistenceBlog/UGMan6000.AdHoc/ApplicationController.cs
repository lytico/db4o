using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using UGMan6000.AdHoc.Model;
using User=UGMan6000.AdHoc.Model.User;

namespace UGMan6000.AdHoc
{
	internal class ApplicationController : IDisposable
	{
		private readonly IObjectContainer _container;

		public ApplicationController()
		{
			// It's also possible to use configuration options:

//			IConfiguration config = Db4oFactory.NewConfiguration();
//			config.ObjectClass(typeof(User)).UpdateDepth(2);
//			config.ObjectClass(typeof(Group)).UpdateDepth(2);
//
//			_container = Db4oFactory.OpenFile(config, "adhoc.db4o");
//
			_container = Db4oFactory.OpenFile("adhoc.db4o");
		}

		public void AddUser(User user)
		{
			_container.Store(user);
			_container.Commit();
		}

		public void AddGroup(Group group)
		{
			_container.Store(group);
			_container.Commit();
		}

		public void AddUserToGroup(User user, Group group)
		{
			group.AddUser(user);
			_container.Ext().Store(user, 2);
			_container.Ext().Store(group, 2);
			_container.Commit();
		}

		public IEnumerable<User> Users
		{
			get { return _container.Query<User>(); }
		}

		public IEnumerable<Group> Groups
		{
			get { return _container.Query<Group>(); }
		}

		public void Dispose()
		{
			_container.Dispose();
		}
	}
}
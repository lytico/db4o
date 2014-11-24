using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using UGMan6000.Extensible.Model;
using User=UGMan6000.Extensible.Model.User;

namespace UGMan6000.Extensible
{
	internal class ApplicationController : IDisposable
	{
		private readonly IObjectContainer _container;

		public ApplicationController()
		{
			_container = Db4oFactory.OpenFile("extensible.db4o");
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
			List<object> affected = UnitOfWork.Run(delegate { group.AddUser(user); });
			foreach (object o in affected)
			{
				_container.Store(o);
			}
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
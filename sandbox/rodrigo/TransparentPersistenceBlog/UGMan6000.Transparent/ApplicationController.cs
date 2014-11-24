using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using UGMan6000.Transparent.Model;
using User = UGMan6000.Transparent.Model.User;

namespace UGMan6000.Transparent
{
	internal class ApplicationController : IDisposable
	{
		private readonly IObjectContainer _container;

		public ApplicationController()
		{	
			_container = Db4oFactory.OpenFile(TransparentConfiguration(), "transparent.db4o");
		}

		private IConfiguration TransparentConfiguration()
		{
			IConfiguration configuration = Db4oFactory.NewConfiguration();
			configuration.Add(new TransparentPersistenceSupport());
			return configuration;
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
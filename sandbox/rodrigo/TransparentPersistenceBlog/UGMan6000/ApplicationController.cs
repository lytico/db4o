using System.Collections.Generic;
using UGMan6000.Model;

namespace UGMan6000
{
	internal class ApplicationController
	{
		private List<User> _users = new List<User>();
		private List<Group> _groups = new List<Group>();

		public void AddUser(User user)
		{
			_users.Add(user);
		}

		public void AddGroup(Group group)
		{
			_groups.Add(group);
		}

		public void AddUserToGroup(User user, Group group)
		{
			group.AddUser(user);
		}

		public IEnumerable<User> Users
		{
			get { return _users;  }
		}

		public IEnumerable<Group> Groups
		{
			get { return _groups; }
		}
	}
}
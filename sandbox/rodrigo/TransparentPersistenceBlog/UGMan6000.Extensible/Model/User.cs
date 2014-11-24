using System.Collections.Generic;

namespace UGMan6000.Extensible.Model
{
	public class Group
	{
		private List<User> _users = new List<User>();
		private string _name;

		public Group(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name;  }
		}

		public IEnumerable<User> Users
		{
			get { return _users; }
		}

		public void AddUser(User user)
		{
			if (_users.Contains(user)) return;

			user.AddedToGroup(this);
			_users.Add(user);
			UnitOfWork.Affected(_users);
		}

		public override string ToString()
		{
			return _name;
		}
	}

	public class User
	{
		private List<Group> _groups = new List<Group>();
		private string _name;

		public User(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}

		public IEnumerable<Group> Groups
		{
			get { return _groups; }
		}

		internal void AddedToGroup(Group g)
		{
			_groups.Add(g);
			UnitOfWork.Affected(_groups);
		}

		public override string ToString()
		{
			return _name;
		}
	}
}

using System.Collections.Generic;
using Db4objects.Db4o.Collections;

namespace UGMan6000.Transparent.Model
{
	/// <summary> 
	/// A Group has a name and a list of users that belong to it.
	/// </summary>
	public class Group
	{
		private string _name;
		private ArrayList4<User> _users = new ArrayList4<User>();

		public Group(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
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
		}

		public override string ToString()
		{
			return _name;
		}
	}

	/// <summary>
	/// An User has a name and a list of group it belongs to.
	/// </summary>
	public class User
	{
		private string _name;
		private ArrayList4<Group> _groups = new ArrayList4<Group>();

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

		public override string ToString()
		{
			return _name;
		}

		/// <summary>
		/// Notifies the user it was added to a group.
		/// </summary>
		internal void AddedToGroup(Group g)
		{
			_groups.Add(g);
		}
	}
}

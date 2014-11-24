using System;

namespace GuidTypeHandlerTester
{
	class GuidItem
	{
		public GuidItem(string name, Guid guid)
		{
			_name = name;
			_guid = guid;
		}

		public Guid Guid { get { return _guid; } }
		public string Name { get { return _name;} }

		private Guid _guid;
		private string _name;
	}
}

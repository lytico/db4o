/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */
using System;
using System.Text;

namespace Db4objects.Db4o.Tests.CLI1.CrossPlatform
{
	internal class Movies
	{
#if !CF
		private readonly String[][] _notes;
		private int _counter;

		public Movies()
		{
			_notes = new String[3][];
		}

		public void Add(string movie, string genre)
		{
			if (_counter < _notes.Length)
			{
				_notes[_counter] = new string[] { movie, genre };
				_counter++;
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < _counter; i++)
			{
				sb.AppendFormat("{0}/{1}\r\n", _notes[i][0], _notes[i][1]);
			}

			return sb.ToString();
		}
#endif
	}
}

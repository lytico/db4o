/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2009  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
namespace Db4objects.Drs.Foundation
{
	public class ObjectSetCollection4Facade : ObjectSetAbstractFacade
	{
        internal Db4objects.Db4o.Foundation.Collection4 _delegate;

        private System.Collections.IEnumerator _currentIterator;

        enum Enumerator_Status
        {
            RESET,
            MOVING,
            EOF,
        };

        Enumerator_Status _status;

        public ObjectSetCollection4Facade(Db4objects.Db4o.Foundation.Collection4 delegate_
            )
        {
            this._delegate = delegate_;
        }


        public override object Next()
        {
            object obj;
            if (HasNext())
            {
                obj = CurrentIterator().Current;
                MoveNext();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public override bool HasNext()
        {
            if (_status == Enumerator_Status.RESET)
            {
                MoveNext();
            }

            return _status != Enumerator_Status.EOF;
        }

        public override void Reset()
        {
            CurrentIterator().Reset();
            _status = Enumerator_Status.RESET;
        }

        public override int Size()
        {
            return this._delegate.Size();
        }

        public override bool Contains(object value)
        {
            return this._delegate.Contains(value);
        }

        private System.Collections.IEnumerator CurrentIterator()
        {
            if (_currentIterator == null)
            {
                _currentIterator = _delegate.GetEnumerator();
                _status = Enumerator_Status.RESET;
            }
            return _currentIterator;
        }

        private void MoveNext()
        {
            if (CurrentIterator().MoveNext())
            {
                _status = Enumerator_Status.MOVING;
            }
            else
            {
                _status = Enumerator_Status.EOF;
            }

        }
	}
}
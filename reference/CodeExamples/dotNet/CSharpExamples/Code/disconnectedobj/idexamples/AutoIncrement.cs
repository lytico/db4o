using System;
using System.Collections.Generic;
using Db4objects.Db4o;

namespace Db4oDoc.Code.DisconnectedObj.IdExamples
{
    public class AutoIncrement
    {
        private PersistedAutoIncrements state = null;
        private readonly IObjectContainer container;
        private readonly object dataLock = new object();

        public AutoIncrement(IObjectContainer container)
        {
            this.container = container;
        }


        // #example: getting the next id and storing state
        public int GetNextID(Type forClass)
        {
            lock (dataLock)
            {
                PersistedAutoIncrements incrementState = EnsureLoadedIncrements();
                return incrementState.NextNumber(forClass);
            }
        }

        public void StoreState()
        {
            lock (dataLock)
            {
                if (null != state)
                {
                    container.Ext().Store(state,2);
                }
            }
        }

        // #end example

        // #example: load the state from the database
        private PersistedAutoIncrements EnsureLoadedIncrements()
        {
            if (null == state)
            {
                state = LoadOrCreateState();
            }
            return state;
        }

        private PersistedAutoIncrements LoadOrCreateState()
        {
            IList<PersistedAutoIncrements> existingState = container.Query<PersistedAutoIncrements>();
            if (0 == existingState.Count)
            {
                return new PersistedAutoIncrements();
            }
            else if (1 == existingState.Count)
            {
                return existingState[0];
            }
            else
            {
                throw new InvalidOperationException("Cannot have more than one state stored in database");
            }
        }

        // #end example

        // #example: persistent auto increment
        private class PersistedAutoIncrements
        {
            private readonly IDictionary<Type, int> currentHighestIds = new Dictionary<Type, int>();

            public int NextNumber(Type forClass)
            {
                int number;
                if (!currentHighestIds.TryGetValue(forClass, out number))
                {
                    number = 0;
                }
                number += 1;
                currentHighestIds[forClass] = number;
                return number;
            }
        }

        // #end example
    }
}
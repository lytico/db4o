using System;
using System.Collections.Generic;
using System.IO;

using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;

namespace TasksSample
{
    public class TaskManager
    {
        public Task Add(Task parent, String label, String description)
        {
            Task newTask = new Task(label, description);
            _db.Set(newTask);

            Task directParent = null;

            if (!AreThereOtherTasks())
            {
                _rootTask.FirstChild = newTask;
            }
            else if (parent == null)
            {
                directParent = FindLastBrother(_rootTask.FirstChild);
                directParent.Next = newTask;
            }
            else
            {
                directParent = FindLastChild(parent);
                if (directParent == null)
                {
                    parent.FirstChild = newTask;
                    directParent = parent;
                }
                else
                {
                    directParent.Next = newTask;
                }
            }

            if (directParent != null) _db.Set(directParent);
            _db.Commit();

            return newTask;
        }

        public void Close()
        {
            if (_db != null)
            {
                if (_rootTask.FirstChild != null) _db.Set(_rootTask);
                _db.Close();
                _db = null;
            }
        }

        public void Load(String filePath)
        {
            IConfiguration config = Db4oFactory.NewConfiguration();
            if (UseSpecificActivationDepth())
            {
                config.ActivationDepth(_activationDepth);
            }

            if (ActiveTransparently) config.Add(new TransparentActivationSupport());
            _db = Db4oFactory.OpenFile(config, filePath);

            IList<RootTask> tasks = _db.Query<RootTask>(typeof(RootTask));
            if (tasks.Count > 1) throw new InvalidOperationException("Corrupted databse.");

            _rootTask = tasks.Count == 1 ? tasks[0] : new RootTask();
        }

        public bool Activate(Task task)
        {
            bool needActivation = !_db.Ext().IsActive(task);
            if (needActivation)
            {
                _db.Activate(task, 1);
            }
            return needActivation;
        }

        public RootTask RootTask
        {
            get { return _rootTask; }
        }

        public Task Tasks
        {
            get { return _rootTask.FirstChild; }
        }

        public int ActivationDepth
        {
            set { _activationDepth = value; }
        }
        
        public bool ActiveTransparently
        {
            get { return typeof(IActivatable).IsAssignableFrom(typeof(Task)); }
        }

        #region Helper Methods

        private bool UseSpecificActivationDepth()
        {
            return _activationDepth != -1;
        }

        private bool AreThereOtherTasks()
        {
            return _rootTask.FirstChild != null;
        }

        private static Task FindLastChild(Task parent)
        {
            return internalFind(parent.FirstChild);
        }

        private static Task FindLastBrother(Task task)
        {
            return internalFind(task);
        }

        private static Task internalFind(Task searchIn)
        {
            Task temp = null;
            if (searchIn != null)
            {
                for (temp = searchIn; temp.Next != null; temp = temp.Next)
                    ;
            }

            return temp;
        }

        #endregion

        private RootTask _rootTask = null;
        private IObjectContainer _db;
        private int _activationDepth = -1;
    }

    public class Task
    {
        public Task() : this(null, null)
        {
        }

        public Task(String label, String description)
        {
            _next = null;
            _firstChild = null;
            _description = description;
            _label = label;
        }

        public String Label
        {
            get { return _label; }
        }

        public String Description
        {
            get { return _description; }
        }

        public Task Next
        {
            get { return _next; }
            set { _next = value; }
        }

        public Task FirstChild
        {
            get { return _firstChild; }
            set { _firstChild = value; }
        }

        public override string ToString()
        {
            return _label ?? "(Task)";
        }

        private Task _firstChild;
        private Task _next;
        private String _description;
        private String _label;
    }

    public class RootTask : Task
    {
    }
}

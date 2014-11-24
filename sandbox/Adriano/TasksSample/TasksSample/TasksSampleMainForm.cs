using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TasksSample
{
    public partial class frmTasks : Form
    {
        public frmTasks()
        {
            InitializeComponent();

            InitializeApplication();

            SetTitle();
        }

        #region Helper Methods

        private void SetTitle()
        {
            Text = String.Format("{0} [Transparent Activation Enabled: {1}]", Text, _taskManager.ActiveTransparently);
        }

        private void InitializeApplication()
        {
            ResetTaskView();
            ResetActivationDepth();
        }

        private void ResetActivationDepth()
        {
            cbActivationDepth.SelectedIndex = 0;
        }

        private void LoadDatabase()
        {
            _taskManager.Load(DATABASE_FILE);
            PopulateTaskView(_taskManager.RootTask);
            UpdateMenuState(true);
        }

        private void CloseDatabase()
        {
            _taskManager.Close();
            ResetTaskView();
            UpdateMenuState(false);
        }

        private void ResetTaskView()
        {
            tvTasks.Nodes.Clear();
            TreeNode root = tvTasks.Nodes.Add("Tasks");
        }

        private void PopulateTaskView(Task rootTask)
        {
            ResetTaskView();

            TreeNode root = tvTasks.Nodes[0];
            root.Tag = rootTask;
            AddTaskToTreeView(root, new GhostTask());
        }

        private void AddNewTaskToCurrentTask(string label, string description)
        {
            TreeNode currentNode = tvTasks.SelectedNode ?? tvTasks.Nodes[0];
            RemoveGhostNodeFromParent(currentNode);
            AddTaskToTreeView(currentNode, _taskManager.Add(currentNode.Tag as Task, label, description));
        }

        private TreeNode AddTaskToTreeView(TreeNode parentNode, Task task)
        {
            TreeNode node = parentNode.Nodes.Add(task.Label);
            node.Tag = task;

            return node;
        }

        private void RemoveGhostNodeFromParent(TreeNode parent)
        {
            if (parent.Nodes.Count > 0)
            {
                GhostTask gt = parent.Nodes[0].Tag as GhostTask;
                if (gt != null) parent.Nodes[0].Remove();
            }
        }
        private void ExpandChildren(TreeNode parent)
        {
            if (parent.Nodes.Count > 0 && parent.Nodes[0].Tag is GhostTask)
            {
                RemoveGhostNodeFromParent(parent);
                Task task = (Task)parent.Tag;
                for (task = task.FirstChild; task != null; task = task.Next)
                {
                    if (RequiresExplicityActivation())
                    {
                        _taskManager.Activate(task);
                    }

                    TreeNode node = AddTaskToTreeView(parent, task);
                    AddTaskToTreeView(node, new GhostTask());
                }
            }
        }

        private bool RequiresExplicityActivation()
        {
            return _automaticActivation && !_taskManager.ActiveTransparently;
        }

        private void UpdateMenuState(bool enableClose)
        {
            mnuFileOpen.Enabled = !enableClose;
            mnuFileClose.Enabled = enableClose;
        }

        private void ReloadDatabase()
        {
            CloseDatabase();
            LoadDatabase();
        }

        #endregion

        #region Event Handlers

        private void cbActivationDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            int activationDepth;
            if (!Int32.TryParse(cbActivationDepth.Text, out activationDepth))
            {
                activationDepth = -1;
            }
            _taskManager.ActivationDepth = activationDepth;

            ReloadDatabase();

        }

        private void mnuTasksAdd_Click(object sender, EventArgs e)
        {
            using (NewTaskDialog ntd = new NewTaskDialog())
            {
                ntd.StartPosition = FormStartPosition.CenterParent;
                if (ntd.ShowDialog() == DialogResult.OK)
                {
                    AddNewTaskToCurrentTask(ntd.TaskLabel, ntd.TaskDescription);
                }
            }
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            LoadDatabase();
        }

        private void mnuFileClose_Click(object sender, EventArgs e)
        {
            CloseDatabase();
        }

        private void tvTasks_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Task task = e.Node.Tag as Task;
            if (task != null)
            {
                txtDescription.Text = task.Description;
            }
        }

        private void tvTasks_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            ExpandChildren(e.Node);
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void frmTasks_FormClosed(object sender, FormClosedEventArgs e)
        {
            _taskManager.Close();
        }

        private void cmdActiveOnDemand_Click(object sender, EventArgs e)
        {
            _automaticActivation = cmdActiveOnDemand.Checked;
            ReloadDatabase();
        }

        #endregion

        #region Instance Variables & Constants

        private TaskManager _taskManager = new TaskManager();
        private bool _automaticActivation = false;
        private const string DATABASE_FILE = "Tasks.db4o";

        #endregion
    }

    class GhostTask : Task
    {
        public GhostTask() : base("...", "")
        {
        }
    }
}
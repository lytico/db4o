using System;
using System.Linq;
using System.Windows.Forms;
using OManager.BusinessLayer.Config;
using OManager.BusinessLayer.UIHelper;


namespace OMControlLibrary
{
    public partial class ChooseAssemblies : Form
    {
        private const string OPEN_FILE_ADDASSEMBLY_FILTER = "Assemblies(*.exe, *.dll)|*.exe;*.dll";
        private ISearchPath _searchPath;

        public ChooseAssemblies(string path)
        {
            dbInteraction.SetPathForConnection(path);
            _searchPath = dbInteraction.GetAssemblySearchPath();
            InitializeComponent();
            InitializePaths();
        }

        private void InitializePaths()
        {
            if (_searchPath == null || _searchPath.Paths.Count() <= 0)
                return;
            foreach (string path in _searchPath.Paths)
            {
                chkListBoxAssemblies.Items.Add(path, CheckState.Checked);
            }

            if (chkListBoxAssemblies.Items.Count > 0)
            {
                chkListBoxAssemblies.SelectedIndex = 0;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = OPEN_FILE_ADDASSEMBLY_FILTER;
            openFileDialog.Title = "Add Assemblies";

            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
            foreach (string path in openFileDialog.FileNames)
            {
                if (_searchPath.Add(path))
                {
                    chkListBoxAssemblies.Items.Add(path, CheckState.Checked);
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (chkListBoxAssemblies.CheckedItems.Count > 0)
            {
                for (int check = chkListBoxAssemblies.CheckedItems.Count - 1;check>=0 ;check-- )
                {
                    _searchPath.Remove((string)chkListBoxAssemblies.CheckedItems[check]);
                    chkListBoxAssemblies.Items.Remove(chkListBoxAssemblies.CheckedItems[check]);
                }
            }
        }
    }


}
    
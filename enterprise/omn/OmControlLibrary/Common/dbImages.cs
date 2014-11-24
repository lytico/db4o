using System;
using System.Text;
using System.Drawing;

namespace OMControlLibrary.Common
{
	public class dbImages
	{
		static Bitmap m_Delete;
		static Bitmap m_Close;
		static Bitmap m_ClearFilter;
		static Bitmap m_Filter;

		static Bitmap m_ClassView;
		static Bitmap m_AssemblyView;
		static Bitmap m_PreviousFilter;
		static Bitmap m_NextFilter;

		//TreeView Images
		static Bitmap m_TreeViewAssembly;
		static Bitmap m_TreeViewClass;
		static Bitmap m_TreeViewCollection;
		static Bitmap m_TreeViewPrimitive;
		static Bitmap m_OpenFolder;
		static Bitmap m_ClosedFolder;

		public static Bitmap PreviousFilter
		{
			get
			{
				if (m_PreviousFilter == null)
					m_PreviousFilter = new Bitmap(Properties.Resources.previous_filter);

				return m_PreviousFilter;
			}
		}

		public static Bitmap openFolder
		{
			get
			{
				if (m_OpenFolder == null)
					m_OpenFolder = new Bitmap(Properties.Resources.Openfolder);

				return m_OpenFolder;
			}
		}
		public static Bitmap closedFolder
		{
			get
			{
				if (m_ClosedFolder == null)
					m_ClosedFolder = new Bitmap(Properties.Resources.Closefolder);

				return m_ClosedFolder;
			}
		}

		public static Bitmap NextFilter
		{
			get
			{
				if (m_NextFilter == null)
					m_NextFilter = new Bitmap(Properties.Resources.next_filter);

				return m_NextFilter;
			}
		}


		public static Bitmap Delete
		{
			get
			{
				if (m_Delete == null)
					m_Delete = new Bitmap(Properties.Resources.delete);

				return m_Delete;
			}
		}

		public static Bitmap Close
		{
			get
			{
				if (m_Close == null)
					m_Close = new Bitmap(Properties.Resources.close);

				return m_Close;
			}
		}

		public static Bitmap ClearFilter
		{
			get
			{
				if (m_ClearFilter == null)
					m_ClearFilter = new Bitmap(Properties.Resources.clear_filter);

				return m_ClearFilter;
			}
		}

		public static Bitmap Filter
		{
			get
			{
				if (m_Filter == null)
					m_Filter = new Bitmap(Properties.Resources.filter);

				return m_Filter;
			}
		}

		public static Bitmap ClassView
		{
			get
			{
				if (m_ClassView == null)
					m_ClassView = new Bitmap(Properties.Resources.flat_view);

				return m_ClassView;
			}
		}

		public static Bitmap AssemblyView
		{
			get
			{
				if (m_AssemblyView == null)
					m_AssemblyView = new Bitmap(Properties.Resources.assembly_view);

				return m_AssemblyView;
			}
		}


		#region TreeView Related Images

		public static Bitmap TreeViewAssembly
		{
			get
			{
				if (m_TreeViewAssembly == null)
					m_TreeViewAssembly = new Bitmap(Properties.Resources.treeview_assembly);

				return m_TreeViewAssembly;
			}
		}

		public static Bitmap TreeViewClass
		{
			get
			{
				if (m_TreeViewClass == null)
					m_TreeViewClass = new Bitmap(Properties.Resources.treeview_class);

				return m_TreeViewClass;
			}
		}

		public static Bitmap TreeViewCollection
		{
			get
			{
				if (m_TreeViewCollection == null)
					m_TreeViewCollection = new Bitmap(Properties.Resources.treeview_collection);

				return m_TreeViewCollection;
			}
		}

		public static Bitmap TreeViewPrimitive
		{
			get
			{
				if (m_TreeViewPrimitive == null)
					m_TreeViewPrimitive = new Bitmap(Properties.Resources.treeview_primitive);

				return m_TreeViewPrimitive;
			}
		}


		#endregion

	}
}

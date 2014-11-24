using System;
using OManager.Business.Config;
using OManager.DataLayer.Reflection;

namespace OMAddinDataTransferLayer.TypeMauplation
{
	[Serializable ]
	public class ProxyType
	{
		public string DisplayName { get; set; }
		public string FullName { get; set; }
		public bool HasIdentity { get; set; }
		public bool IsEditable { get; set; }
		public bool IsPrimitive { get; set; }
		public bool IsCollection { get; set; }
		public bool IsArray { get; set; }
		public bool IsNullable { get; set; }
		public string ContainingClassName { get; set; }

		public ProxyType CovertITypeToProxyType(IType type)
		{
			DisplayName = type.DisplayName;
			FullName = type.FullName; 
			IsCollection = type.IsCollection;
			IsEditable = type.IsEditable;
			HasIdentity = type.HasIdentity;
			IsNullable = type.IsNullable;
			IsPrimitive = type.IsPrimitive;
			IsArray = type.IsArray;
			IsNullable = type.IsNullable;
			return this; 
        }

	}
}

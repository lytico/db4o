/* Copyright (C) 2004 - 2009  Versant Inc.  http://www.db4o.com */

namespace OManager.Business.Config
{
    public interface IType
    {
        object Cast(object value);
    	void SetField(object onObject, string fieldName, object value);
        string DisplayName { get; }
        string FullName { get; }
        bool HasIdentity { get; }
        bool IsEditable { get; }
        bool IsPrimitive { get; }
        bool IsCollection { get; }
        bool IsArray { get; }
        bool IsNullable { get; }
        bool IsSameAs(System.Type other);
		IType UnderlyingType { get; }
    }
}

using System;

namespace Db4oDoc.Code.Tp.Enhancement
{
    // #example: Annotation to mark persisted classes
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TransparentPersistedAttribute : Attribute
    {
    }
    // #end example

}
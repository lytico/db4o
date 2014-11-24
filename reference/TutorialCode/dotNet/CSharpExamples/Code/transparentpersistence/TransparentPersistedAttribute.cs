using System;

namespace Db4oTutorialCode.Code.TransparentPersistence
{
    // #example: Annotation to mark persisted classes
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = true)]
    public class TransparentPersistedAttribute : Attribute
    {
    }
    // #end example
}
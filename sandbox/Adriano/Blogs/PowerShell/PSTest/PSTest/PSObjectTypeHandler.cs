using System.Collections.Generic;
using System.Management.Automation;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace CmdLets.Db4objects
{
	class PSObjectTypeHandler : ITypeHandler4
	{
		public IPreparedComparison PrepareComparison(IContext context, object obj)
		{
			throw new System.NotImplementedException();
		}

		public void Delete(IDeleteContext context)
		{
			context.Delete(TypeHandler());
		}

		public void Defragment(IDefragmentContext context)
		{
			context.Defragment(TypeHandler());
		}

		public object Read(IReadContext context)
		{
			var wrapper = (PSObjectWrapper)context.ReadObject(TypeHandler());
			PSObject obj = (PSObject) ((UnmarshallingContext) context).PersistentObject();
			return wrapper.Original(obj, context.ObjectContainer());
		}

		public void Write(IWriteContext context, object obj)
		{
			context.WriteObject(TypeHandler(), new PSObjectWrapper((PSObject)obj));
		}

		private static FirstClassObjectHandler TypeHandler()
		{
			return new FirstClassObjectHandler();
		}
	}

	internal class PSObjectWrapper
	{
		public PSObjectWrapper(PSObject obj)
		{
			foreach (PSPropertyInfo property in obj.Properties)
			{
				if (property.IsGettable)
				{
					properties[property.Name] = property.Value;
				}
			}

			foreach (PSMethodInfo method in obj.Methods)
			{
				var scriptMethod = method.Value as PSScriptMethod;
				if (null != scriptMethod)
				{
					methods[method.Name] = scriptMethod.Script.ToString();
				}
			}
		}

		public PSObject Original(PSObject obj, IObjectContainer container)
		{
			foreach (var pair in properties)
			{
				obj.Properties.Add(new PSNoteProperty(pair.Key, pair.Value));
			}

			foreach (var pair in methods)
			{
				obj.Methods.Add(new PSScriptMethod(pair.Key, Db4oObjectCommandBase.Instance.InvokeCommand.NewScriptBlock(pair.Value)));
			}

			return obj;
		}

		private readonly IDictionary<string, object> properties = new Dictionary<string, object>();
		private readonly IDictionary<string, string> methods = new Dictionary<string, string>();
	}
}

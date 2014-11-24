/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.TA
{
	/// <summary>
	/// Enables Transparent Persistence and Transparent Activation behaviours for
	/// the current session.
	/// </summary>
	/// <remarks>
	/// Enables Transparent Persistence and Transparent Activation behaviours for
	/// the current session.
	/// <br/><br/>
	/// commonConfiguration.Add(new TransparentPersistenceSupport());
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.TA.TransparentActivationSupport">Db4objects.Db4o.TA.TransparentActivationSupport
	/// </seealso>
	public class TransparentPersistenceSupport : TransparentActivationSupport
	{
		private readonly IRollbackStrategy _rollbackStrategy;

		/// <summary>Creates a new instance of TransparentPersistenceSupport class</summary>
		/// <param name="rollbackStrategy">
		/// RollbackStrategy interface implementation, which
		/// defines the actions to be taken on the object when the transaction is rolled back.
		/// </param>
		public TransparentPersistenceSupport(IRollbackStrategy rollbackStrategy)
		{
			_rollbackStrategy = rollbackStrategy;
		}

		/// <summary>
		/// Creates a new instance of TransparentPersistenceSupport class
		/// with no rollback strategies defined.
		/// </summary>
		/// <remarks>
		/// Creates a new instance of TransparentPersistenceSupport class
		/// with no rollback strategies defined.
		/// </remarks>
		public TransparentPersistenceSupport() : this(null)
		{
		}

		/// <summary>Configures current ObjectContainer to support Transparent Activation and Transparent Persistence
		/// 	</summary>
		/// <seealso cref="TransparentActivationSupport.Apply(Db4objects.Db4o.Internal.IInternalObjectContainer)
		/// 	"></seealso>
		public override void Apply(IInternalObjectContainer container)
		{
			base.Apply(container);
			EnableTransparentPersistenceFor(container);
		}

		private void EnableTransparentPersistenceFor(IInternalObjectContainer container)
		{
			ITransparentActivationDepthProvider provider = (ITransparentActivationDepthProvider
				)ActivationProvider(container);
			provider.EnableTransparentPersistenceSupportFor(container, _rollbackStrategy);
		}

		public override void Prepare(IConfiguration configuration)
		{
			base.Prepare(configuration);
			((Config4Impl)configuration).UpdateDepthProvider(new TPUpdateDepthProvider());
		}
	}
}

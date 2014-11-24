/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.config;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.internal.*;
import com.db4o.cs.internal.messages.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.query.*;

/**
 * @exclude
 * @deprecated Since 8.0
 * @sharpen.ignore
 */
public class DotNetSupportClientServer implements ConfigurationItem {

	public void apply(InternalObjectContainer container) {
		// do nothing.
	}

	public void prepare(Configuration config) {
		config.addAlias(new TypeAlias("System.Exception, mscorlib", ChainedRuntimeException.class.getName()));
		
		//		config.addAlias(new TypeAlias("java.lang.Throwable", FullTypeNameFor(typeof(Exception))));
		//		config.addAlias(new TypeAlias("java.lang.RuntimeException", FullTypeNameFor(typeof(Exception))));
		//		config.addAlias(new TypeAlias("java.lang.Exception", FullTypeNameFor(typeof(Exception))));
		
		
		config.addAlias(new TypeAlias("Db4objects.Db4o.Query.IEvaluation, Db4objects.Db4o", Evaluation.class.getName()));
		config.addAlias(new TypeAlias("Db4objects.Db4o.Query.ICandidate, Db4objects.Db4o", Candidate.class.getName()));
		
		config.addAlias(new WildcardAlias("Db4objects.Db4o.Internal.Query.Processor.*, Db4objects.Db4o", "com.db4o.internal.query.processor.*"));

		config.addAlias(new TypeAlias("Db4objects.Db4o.Foundation.Collection4, Db4objects.Db4o", Collection4.class.getName()));
		config.addAlias(new TypeAlias("Db4objects.Db4o.Foundation.List4, Db4objects.Db4o", List4.class.getName()));
		config.addAlias(new TypeAlias("Db4objects.Db4o.User, Db4objects.Db4o", User.class.getName()));

		config.addAlias(new TypeAlias("Db4objects.Db4o.CS.Internal.ClassInfo, Db4objects.Db4o.CS", ClassInfo.class.getName()));
		config.addAlias(new TypeAlias("Db4objects.Db4o.CS.Internal.FieldInfo, Db4objects.Db4o.CS", FieldInfo.class.getName()));
		
		config.addAlias(
				new TypeAlias(
						"Db4objects.Db4o.CS.Internal.Messages.MUserMessage+UserMessagePayload, Db4objects.Db4o.CS", 
						MUserMessage.UserMessagePayload.class.getName()));
		
		config.addAlias(new WildcardAlias("Db4objects.Db4o.CS.Internal.Messages.*, Db4objects.Db4o.CS", "com.db4o.cs.internal.messages.*"));

		
	}

}

package decaf.builder;

import org.eclipse.jdt.core.dom.*;
import org.eclipse.jdt.core.dom.rewrite.*;

import sharpen.core.framework.*;
import decaf.config.*;
import decaf.core.*;
import decaf.rewrite.*;

public class DecafRewritingContext {
	
	public static DecafRewritingContext current() {
		return currentVariable().value();
    }

	private static DynamicVariable<DecafRewritingContext> currentVariable() {
	    return _current.get();
    }
	
	private static ThreadLocal<DynamicVariable<DecafRewritingContext>> _current = new ThreadLocal<DynamicVariable<DecafRewritingContext>>() {
		@Override
		protected DynamicVariable<DecafRewritingContext> initialValue() {
			return new DynamicVariable<DecafRewritingContext>(null);
		}
	};
	
	private final DecafRewritingServices _rewrite;
	private final DecafASTNodeBuilder _builder;
	private final TargetPlatform _targetPlatform;
	
	public DecafRewritingContext(CompilationUnit unit, ASTRewrite rewrite, TargetPlatform targetPlatform, DecafConfiguration decafConfig) {
		_builder = new DecafASTNodeBuilder(unit, decafConfig);
		_rewrite = new DecafRewritingServices(rewrite, _builder);
		_targetPlatform = targetPlatform;
	}
	
	public TargetPlatform targetPlatform() {
		return _targetPlatform;
	}

	public DecafASTNodeBuilder builder() {
		return _builder;
	}

	public DecafRewritingServices rewrite() {
		return _rewrite;
	}

	/**
	 * Runs the passed runnable in this context ({@link #current()} will return this context
	 * as the runnable executes).
	 * 
	 * @param runnable
	 */
	public void run(Runnable runnable) {
		currentVariable().using(this, runnable);
    }	
}

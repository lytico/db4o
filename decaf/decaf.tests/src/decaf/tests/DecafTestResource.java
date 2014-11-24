package decaf.tests;

import sharpen.ui.tests.*;
import decaf.core.*;

public class DecafTestResource extends TestCaseResource {

	private static final String PATH_SUFFIX_DECAF = ".decaf";
	private static final String PATH_SUFFIX_TXT = ".txt";
	private final TargetPlatform _targetPlatform;
	
	public DecafTestResource(String originalPath) {
		this(originalPath, null);
	}

	public DecafTestResource(String originalPath, TargetPlatform targetPlatform) {
		super(originalPath);
		_targetPlatform = targetPlatform;
	}
	
	@Override
	protected String expectedPathSuffix() {
		return _targetPlatform.appendPlatformId(PATH_SUFFIX_DECAF, ".") + PATH_SUFFIX_TXT;
	}

}

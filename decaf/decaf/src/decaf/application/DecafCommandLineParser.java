package decaf.application;

import sharpen.core.framework.*;
import decaf.core.*;

public class DecafCommandLineParser extends CommandLineParser {

	public static DecafCommandLine parse(String... args) {
		return new DecafCommandLineParser(args).commandLine();
	}

	private final DecafCommandLine _commandLine;
	
	public DecafCommandLineParser(String[] args) {
		super(args);
		_commandLine = new DecafCommandLine();
		parse();
	}

	private DecafCommandLine commandLine() {
		return _commandLine;
	}

	@Override
	protected void processArgument(String arg) {
		_commandLine.project = arg;
	}

	@Override
	protected void processOption(String arg) {
		if (areEqual(arg, "-projectReference")) {
			_commandLine.projectReferences.add(consumeNext());
		} else if (areEqual(arg, "-cp")) {
			_commandLine.classpath.add(consumeNext());
		} else if (areEqual(arg, "-targetPlatform")) {
			_commandLine.targetPlatforms.add(TargetPlatform.valueOf(consumeNext().toUpperCase()));
		} else if (areEqual(arg, "-build")) {
			_commandLine.build = true;
		} else if (areEqual(arg, "-srcFolder")) {
			_commandLine.srcFolders.add(consumeNext());
		} else {
			illegalArgument(arg);
		}
	}
}

package decaf.builder;

import org.eclipse.core.filebuffers.*;
import org.eclipse.core.runtime.*;
import org.eclipse.jdt.core.*;
import org.eclipse.jdt.core.dom.rewrite.*;
import org.eclipse.jface.text.*;

public class FileRewriter {

	public static void rewriteFile(final ASTRewrite rewrite, IPath path)
			throws CoreException, BadLocationException, JavaModelException {
		ITextFileBufferManager bufferManager = FileBuffers.getTextFileBufferManager();
		try {
			bufferManager.connect(path, LocationKind.IFILE, null);
			ITextFileBuffer buffer = bufferManager.getTextFileBuffer(path, LocationKind.IFILE);
			rewrite.rewriteAST().apply(buffer.getDocument(), 0);
			buffer.commit(null, true);
		} finally {
			bufferManager.disconnect(path, LocationKind.IFILE, null);
		}
	}

}

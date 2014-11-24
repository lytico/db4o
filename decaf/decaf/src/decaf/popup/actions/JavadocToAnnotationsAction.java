package decaf.popup.actions;

import java.util.*;

import org.eclipse.core.resources.*;
import org.eclipse.core.runtime.*;
import org.eclipse.jdt.core.*;
import org.eclipse.jdt.core.dom.*;
import org.eclipse.jdt.core.dom.rewrite.*;
import org.eclipse.jface.action.*;
import org.eclipse.jface.viewers.*;
import org.eclipse.ui.*;

import sharpen.core.*;
import sharpen.core.framework.*;
import decaf.*;
import decaf.builder.*;

public class JavadocToAnnotationsAction implements IObjectActionDelegate {

	private IStructuredSelection _selection;
	
	/**
	 * @see IObjectActionDelegate#setActivePart(IAction, IWorkbenchPart)
	 */
	public void setActivePart(IAction action, IWorkbenchPart targetPart) {
	}

	/**
	 * @see IActionDelegate#run(IAction)
	 */
	public void run(IAction action) {
		for (Object o : _selection.toArray()) {
			javadocToAnnotations((IProject)o);
		}
	}

	private void javadocToAnnotations(IProject o) {
		final IJavaProject javaProject = JavaCore.create(o);
		if (javaProject == null) return;
		
		new WorkspaceJob("javadoc to annotations") {

			@Override
            public IStatus runInWorkspace(IProgressMonitor monitor) throws CoreException {
				try {
					final List<ICompilationUnit> units = JavaModelUtility.collectCompilationUnits(javaProject);
					monitor.beginTask(getName(), units.size());
					
					for (ICompilationUnit cu : units) {
						if (monitor.isCanceled())
							return Status.CANCEL_STATUS;
						
						monitor.subTask(cu.getElementName());
						javadocToAnnotations(cu);
						monitor.worked(1);
					}
				} catch (JavaModelException e) {
					return new Status(Status.ERROR, Activator.PLUGIN_ID, e.getMessage(), e);
				}
	            return Status.OK_STATUS;
            }
			
		}.schedule();
		
	}

	private void javadocToAnnotations(ICompilationUnit cu) throws JavaModelException {
		final CompilationUnit compilationUnit = DecafRewriter.parseCompilationUnit(cu, null);
		final AST ast = compilationUnit.getAST();
		final ASTRewrite rewrite = ASTRewrite.create(ast);
		compilationUnit.accept(new ASTVisitor() {
			@Override
			public void endVisit(Javadoc node) {
				final ASTNode parent = node.getParent();
				if (!(parent instanceof BodyDeclaration))
					return;
				
				for (TagElement tag : tags(node)) {
					final String tagName = tag.getTagName();
					if (null == tagName)
						continue;
					
					if (tagName.startsWith("@decaf.ignore") 
						&& !tagName.startsWith("@decaf.ignore.extends")
						&& !tagName.startsWith("@decaf.ignore.implements")) {
						System.out.println(ASTUtility.sourceInformation(compilationUnit, tag) + ": '" + tag + "' removed.");
						rewrite.remove(tag, null);
						
						if (parent instanceof TypeDeclaration) {
							addAnnotationForTag(parent, TypeDeclaration.MODIFIERS2_PROPERTY, tagName);
						} else if (parent instanceof EnumDeclaration) {
							addAnnotationForTag(parent, EnumDeclaration.MODIFIERS2_PROPERTY, tagName);
						} else if (parent instanceof MethodDeclaration) {
							addAnnotationForTag(parent, MethodDeclaration.MODIFIERS2_PROPERTY, tagName);
						} else {
							throw new IllegalStateException(parent.toString());
						}
					
					}
				}
				super.endVisit(node);
			}

			private void addAnnotationForTag(ASTNode parent, ChildListPropertyDescriptor annotationsProperty, String tagName) {
				final ListRewrite annotations = rewrite.getListRewrite(parent, annotationsProperty);
				annotations.insertFirst(annotationFromTag(tagName), null);
			}

			private ASTNode annotationFromTag(String tagName) {
				final String[] tagParts = tagName.split("\\.");
				final String annotationType = "decaf." + toPascalCase(tagParts[1]);
				final ASTNode annotation = tagParts.length == 2
					? newMarkerAnnotation(annotationType) // no platform
					: newAnnotationWithPlatform(annotationType, tagParts[2]);
				return annotation;
			}

			private String toPascalCase(String string) {
				return string.substring(0, 1).toUpperCase() + string.substring(1);
			}

			private SingleMemberAnnotation newAnnotationWithPlatform(final String typeName, String platform) {
				final SingleMemberAnnotation annotation = ast.newSingleMemberAnnotation();
				annotation.setTypeName(ast.newName(typeName));
				annotation.setValue(ast.newName("decaf.Platform." + platform.toUpperCase()));
				return annotation;
			}

			private MarkerAnnotation newMarkerAnnotation(final String typeName) {
				final MarkerAnnotation ignoreAnnotation = ast.newMarkerAnnotation();
				ignoreAnnotation.setTypeName(ast.newName(typeName));
				return ignoreAnnotation;
			}

			private Iterable<TagElement> tags(Javadoc node) {
				return ((Iterable<TagElement>)node.tags());
			}
		});
		cu.applyTextEdit(rewrite.rewriteAST(), null);
		cu.save(null, true);
	}

	/**
	 * @see IActionDelegate#selectionChanged(IAction, ISelection)
	 */
	public void selectionChanged(IAction action, ISelection selection) {
		_selection = (IStructuredSelection) selection;
	}

}

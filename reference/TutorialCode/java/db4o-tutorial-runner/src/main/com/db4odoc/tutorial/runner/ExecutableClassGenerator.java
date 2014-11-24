package com.db4odoc.tutorial.runner;

import com.db4o.ObjectContainer;
import com.db4odoc.tutorial.faulttolerance.OperationResult;
import com.db4odoc.tutorial.utils.OneArgAction;
import org.eclipse.jdt.internal.compiler.tool.EclipseCompiler;

import javax.tools.JavaCompiler;
import javax.tools.JavaFileObject;
import javax.tools.StandardLocation;
import java.io.StringWriter;
import java.util.Arrays;
import java.util.Collections;
import java.util.UUID;

import static com.db4odoc.tutorial.utils.ExceptionUtils.reThrow;

class ExecutableClassGenerator {
    JavaCompiler compiler = new EclipseCompiler();

    ExecutableClassGenerator() {

    }

    public static ExecutableClassGenerator create() {
        return new ExecutableClassGenerator();
    }
    public OperationResult<OneArgAction<ObjectContainer>> generateWithBody(
            String methodBody){
        return generateWithBody(methodBody, Collections.<String>emptyList());
    }

    public OperationResult<OneArgAction<ObjectContainer>> generateWithBody(
            String methodBody, Iterable<String> imports) {
        final Object instance;
        try {
            final OperationResult<Class> result = createClass(methodBody,imports);
            if(result.wasSuccessful()){
                instance = result.getResultData().newInstance();
                return OperationResult.success((OneArgAction<ObjectContainer>) instance);
            } else{
                return result.failToAny();
            }
        } catch (Exception e) {
            throw reThrow(e);
        }

    }

    private OperationResult<Class> createClass(String methodBody, Iterable<String> imports) throws ClassNotFoundException {
        final String className = produceClassName();

        String out = buildClass(methodBody, className,imports);

        MemoryFileManager fileManager = new MemoryFileManager(compiler.getStandardFileManager(null, null, null));

        final JavaFileObject source = fileManager.makeSource(className, out);
        StringWriter writer = new StringWriter();
        final JavaCompiler.CompilationTask task = compiler.getTask(writer, fileManager, null, null, null, Arrays.asList(source));

        if (task.call()) {
            ClassLoader cl = fileManager.getClassLoader(StandardLocation.CLASS_OUTPUT);
            return OperationResult.success((Class)cl.loadClass(className));
        }
        return failWith(writer.toString());
    }

    private String buildClass(String methodBody, String className,Iterable<String> imports) {
        StringBuilder out = new StringBuilder();
        out.append("import java.util.*;\n");
        out.append("import com.db4o.*;\n");
        out.append("import com.db4o.query.*;\n");
        appendImports(out,imports);
        out.append("public class ");
        out.append(className);
        out.append(" implements com.db4odoc.tutorial.utils.OneArgAction<com.db4o.ObjectContainer>\n");
        out.append("{\n");
        out.append(makeMethodBody(methodBody));
        out.append("}\n");
        return out.toString();
    }

    private void appendImports(StringBuilder out, Iterable<String> imports) {
        for (String imp : imports){
            out.append("import ").append(imp).append(";\n");
        }
    }

    private String produceClassName() {
        final String id = UUID.randomUUID().toString().replace('-', '_');
        return "DynamicOneArgAction" + id;
    }

    private OperationResult<Class> failWith(String errors) {
        return OperationResult.fail(new CouldNotGenerateCodeException("Compilation error: \n"+errors));
    }
    private String makeMethodBody(String methodBody) {
        return "public void invoke(com.db4o.ObjectContainer container){\n"
                + methodBody
                + "\n}";
    }

}

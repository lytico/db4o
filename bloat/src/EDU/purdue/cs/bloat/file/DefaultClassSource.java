package EDU.purdue.cs.bloat.file;

public class DefaultClassSource implements ClassSource {

	public Class loadClass(String name) throws ClassNotFoundException {
		return Class.forName(name);
	}

}

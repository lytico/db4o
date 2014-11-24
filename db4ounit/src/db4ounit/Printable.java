package db4ounit;

import java.io.IOException;
import java.io.StringWriter;
import java.io.Writer;

public abstract class Printable {

	public String toString() {
		StringWriter writer = new StringWriter();
		try {
			print(writer);
		} catch (IOException e) {
		}
		return writer.toString();
	}
	
	public abstract void print(Writer writer) throws IOException;

}

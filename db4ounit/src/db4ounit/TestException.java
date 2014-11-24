package db4ounit;

import java.io.PrintWriter;

/**
 * @sharpen.ignore
 */
public class TestException extends RuntimeException {
	
	private static final long serialVersionUID = 1L;
	
	private final Throwable _reason;
	
	public TestException(String message, Throwable reason) {
		super(message);
		_reason = reason;
	}	

	public TestException(Throwable reason) {
		_reason = reason;
	}
	
	public final Throwable getReason() {
		return _reason;
	}
	
	public void printStackTrace(PrintWriter s) {
		if (null != _reason) {
			super.printStackTrace(s);
			s.write(" caused by ");
			_reason.printStackTrace(s);
		} else {
			super.printStackTrace(s);
		}
		
	}
	
	public String toString() {
		String s = super.toString();
		if (null != _reason) {
			s = s + " caused by " + _reason.toString();
		}
		
		return s;
	}
}

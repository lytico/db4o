package com.db4o.db4ounit.common.ta;

import java.lang.reflect.*;
import java.util.*;
import java.util.LinkedList;

import com.db4o.collections.*;

import db4ounit.*;

/**
 * @sharpen.ignore
 */
public class ApiImplementationCompleteness implements TestCase {

	private static final String NEW_LINE = System.getProperty("line.separator");

	public static class MethodWrapper {
		public MethodWrapper(Method m) {
			this.m = m;
		}

		public Method m;

		@Override
		public int hashCode() {
			final int prime = 31;
			int result = 1;
			result = prime * result + ((m == null) ? 0 : m.getName().hashCode());
			result = prime * result + Arrays.hashCode(m.getParameterTypes());
			return result;
		}

		@Override
		public boolean equals(Object obj) {
			if (this == obj)
				return true;
			if (obj == null)
				return false;
			if (getClass() != obj.getClass())
				return false;
			MethodWrapper other = (MethodWrapper) obj;
			if (m == null) {
				if (other.m != null)
					return false;
			} else if (!m.getName().equals(other.m.getName()) || !Arrays.equals(m.getParameterTypes(), other.m.getParameterTypes())) // TODO
																																		// support
																																		// contravariance
				return false;
			return true;
		}

		public String toString() {
			try {
				StringBuffer sb = new StringBuffer();
				sb.append(getTypeName(m.getDeclaringClass()) + ".");
				sb.append(m.getName() + "(");
				Class[] params = m.getParameterTypes();
				for (int j = 0; j < params.length; j++) {
					sb.append(getTypeName(params[j]));
					if (j < (params.length - 1))
						sb.append(",");
				}
				sb.append(")");
				return sb.toString();
			} catch (Exception e) {
				throw new RuntimeException(e);
			}
		}

		String getTypeName(Class type) {
			if (type.isArray()) {
				try {
					Class cl = type;
					int dimensions = 0;
					while (cl.isArray()) {
						dimensions++;
						cl = cl.getComponentType();
					}
					StringBuffer sb = new StringBuffer();
					sb.append(cl.getName());
					for (int i = 0; i < dimensions; i++) {
						sb.append("[]");
					}
					return sb.toString();
				} catch (Throwable e) { /* FALLTHRU */
				}
			}
			return type.getName();
		}

	}

	public void testActivatableLists() {
		
		testImplementationCompleteness(ActivatableLinkedList.class, LinkedList.class);
		testImplementationCompleteness(ActivatableArrayList.class, ArrayList.class);
		testImplementationCompleteness(ActivatableStack.class, Stack.class);
	}

	private void testImplementationCompleteness(Class<?> activatable, Class<?> original) {
		Set<MethodWrapper> iface = getMethods(true, new HashSet<MethodWrapper>(), original);
		List<MethodWrapper> impl = getMethods(false, new ArrayList<MethodWrapper>(), activatable);

		for (MethodWrapper m : impl) {
			iface.remove(m);
		}

		Assert.isTrue(iface.isEmpty(), getMissingMethods(iface));
	}

	private String getMissingMethods(Set<MethodWrapper> methods) {
		
		List<MethodWrapper> sortedMethods = new ArrayList<MethodWrapper>(methods);
		Collections.sort(sortedMethods, new Comparator<MethodWrapper>() {

			@Override
			public int compare(MethodWrapper o1, MethodWrapper o2) {
				return o1.toString().compareTo(o2.toString());
			}
		});
		
		String ret = "Missing methods:" + NEW_LINE;
		for (MethodWrapper m : sortedMethods) {
			ret += "    ";
			ret += m;
			ret += NEW_LINE;
		}
		
		return ret;
	}

	private <T extends Collection<MethodWrapper>> T getMethods(boolean recursive, T collection, Class<?>... classes) {
		for (Class<?> clazz : classes) {

			Class<?> candidate = clazz;
			while (candidate != null && candidate != Object.class) {
				Method[] methods = candidate.getMethods();
				for (Method method : methods) {
					if (method.getDeclaringClass() != candidate)
						continue;
					collection.add(new MethodWrapper(method));
				}
				if (!recursive)
					break;
				candidate = candidate.getSuperclass();
			}
		}
		return collection;
	}

}

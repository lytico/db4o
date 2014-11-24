package objectmanager.test;

import com.db4o.ObjectSet;
import com.db4o.Db4o;
import com.db4o.ObjectContainer;

import java.util.Date;
import java.io.File;

/**
 * Contributed from forum user.
 *
 * User: treeder
 * Date: Jan 18, 2007
 * Time: 1:55:42 PM
 */
public class InheritanceTest {
	public static class test1 {
		public String str;
	}

	public static class test2 extends test1 {
		public int i;
	}

	public static class test3 extends test2 {
		public Date date;
	}

	public static void main(String[] args) {
		File l_file = new File("InheritanceB2.yap");
		if (l_file.exists()) l_file.delete();
		ObjectContainer db2 = Db4o.openFile("InheritanceB2.yap");
		test1 l_t1 = new test1();
		l_t1.str = "InheritanceB1";
		db2.set(l_t1);
		test2 l_t2 = new test2();
		l_t2.str = "InheritanceB2";
		l_t2.i = 222;
		db2.set(l_t2);
		test3 l_t3 = new test3();
		l_t3.str = "InheritanceB3";
		l_t3.i = 333;
		l_t3.date = new Date();
		db2.set(l_t3);
		db2.close();
		db2 = Db4o.openFile("InheritanceB2.yap");
		ObjectSet<test1> l_os = db2.get(test1.class);
		System.out.println("test instances:");
		for (test1 l_t : l_os) {
			System.out.println("str=" + l_t.str);
		}
		ObjectSet<test2> l_os2 = db2.get(test2.class);
		System.out.println("\ntest2 instances:");
		for (test2 l_t : l_os2) {
			System.out.println("str=" + l_t.str);
			System.out.println("i=" + l_t.i);
		}

		ObjectSet<test3> l_os3 = db2.get(test3.class);
		System.out.println("\ntest3 instances:");
		for (test3 l_t : l_os3) {
			System.out.println("str=" + l_t.str);
			System.out.println("i=" + l_t.i);
			System.out.println("date=" + l_t.date);
		}
		db2.close();
	}

}
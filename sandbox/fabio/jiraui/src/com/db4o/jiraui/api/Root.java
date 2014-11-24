package com.db4o.jiraui.api;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

public class Root {
	
	public static final String CONTAINER_FILE = "issues.db4o";
	private EmbeddedObjectContainer db;

	public Root() {
		open();
	}

	private void open() {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.common().objectClass(Task.class).objectField("key").indexed(true);
		config.common().objectClass(Task.class).objectField("order").indexed(true);
		config.common().objectClass(Resource.class).objectField("id").indexed(true);
		config.common().objectClass(Project.class).objectField("id").indexed(true);
		db = Db4oEmbedded.openFile(config, CONTAINER_FILE);
	}

	public Task task(String key) {
		Query q = db.query();
		q.constrain(Task.class);
		q.descend("id").constrain(key);
		ObjectSet<Object> s = q.execute();
		return s.hasNext() ? (Task) s.next() : null;
	}

	public void store(Object obj) {
		db.store(obj);
	}

	public void commit() {
		db.commit();
	}

	public void close() {
		db.close();
	}

	public Resource resource(String assignee) {
		Query q = db.query();
		q.constrain(Resource.class);
		q.descend("id").constrain(assignee);
		ObjectSet<Object> s = q.execute();
		return (Resource) (s.hasNext() ? s.next() : null);
	}

	public Project project(String project) {
		Query q = db.query();
		q.constrain(Project.class);
		q.descend("id").constrain(project);
		ObjectSet<Object> s = q.execute();
		return (Project) (s.hasNext() ? s.next() : null);
	}

	public Task accept(Visitor<Task> visitor) {
		Query q = db.query();
		q.constrain(Task.class);
		q.descend("fineGrainedOrder").orderAscending();
		ObjectSet<Task> s = q.execute();
		for (Task task : s) {
			Task ret = visitor.visit(task);
			if (ret != null) {
				return ret;
			}
		}
		return null;
	}

	public Iteration iteration(int id) {
		Query q = db.query();
		q.constrain(Iteration.class);
		q.descend("id").constrain(id);
		ObjectSet<Iteration> s = q.execute();
		return s.hasNext() ? s.next() : null;
	}

	public void delete(Task snapshot) {
		db.delete(snapshot);
	}

	public void clear() {
		close();
		new File(CONTAINER_FILE).delete();
		open();
		
	}

}

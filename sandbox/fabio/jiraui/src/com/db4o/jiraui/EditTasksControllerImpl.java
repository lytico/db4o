package com.db4o.jiraui;

import java.io.*;
import java.net.*;
import java.rmi.RemoteException;
import java.util.*;
import java.util.List;

import org.eclipse.swt.*;
import org.eclipse.swt.widgets.*;


import com.atlassian.jira.rpc.soap.client.*;
import com.db4o.jiraui.api.*;
import com.db4o.jiraui.soap.*;
import com.db4o.jiraui.ui.*;

public final class EditTasksControllerImpl implements EditTasksController {
	private final Root root;
	private SOAPSession soapSession;
	private final Display display;
	public EditTasks view;

	EditTasksControllerImpl(Display display, Root root) {
		this.display = display;
		this.root = root;
	}

	@Override
	public Task acceptTaskVisitor(Visitor<Task> visitor) {
		return root.accept(visitor);
	}

	@Override
	public Collection<Task> moveTasks(Task taskBefore, Task taskAfter, Collection<Task> tasks, int order) {

		int jiraOrder = order != -1 ? order : taskAfter != null ? taskAfter.getOrder() : taskBefore.getOrder();

		for(Task task : tasks) {
			task.setOrder(jiraOrder);
			// storing is done after we figure out the fine grained order and
			// what tasks will be set with it.
		}
		
		
		List<Task> allTasks = null;
		
		while(true) {
			
			float orderBefore = taskBefore == null ? 0 : taskBefore.getFineGrainedOrder();
	
			float diff = taskAfter.getFineGrainedOrder() - orderBefore;
	
			float inc = diff / (tasks.size() + 1f);
			
//			System.out.println("inc: " + inc);
			
			if (inc < .001) {
				Collection<Task> newList = new LinkedHashSet<Task>();
				if (taskBefore != null) newList.add(taskBefore);
				newList.addAll(tasks);
				if (taskAfter != null) newList.add(taskAfter);
				
				tasks = newList;
	
				if (allTasks == null) {
					allTasks = allTasks();
				}
				
				int indexBefore = taskBefore == null ? 0 : indexOf(allTasks, taskBefore, 0);
				int indexAfter = taskAfter == null ? Integer.MAX_VALUE : indexOf(allTasks, taskAfter, indexBefore>=0?indexBefore:0);
	
				if (indexBefore > 0) taskBefore = allTasks.get(indexBefore-1);
				if (indexAfter < allTasks.size()-1) taskAfter = allTasks.get(indexAfter+1);
				
				continue;
			}
			
			
			float nextOrder = orderBefore + inc;
	
			for (Task task : tasks) {
//				if (nextOrder == task.getFineGrainedOrder())
//					continue;
//				Task snapshot = task.getSnapshot();
//				if (snapshot != null && snapshot.getFineGrainedOrder() == nextOrder) {
//					task.resetSnapshot();
//					root.delete(task.getSnapshot());
//				} else {
					task.makeDirty();
//					System.out.printf("----> %s - %.6f - %.3f\n", task.getKey(), nextOrder, Math.round(nextOrder*1000f)/1000f);
//					task.setFineGrainedOrder(n/1000f);
					task.setFineGrainedOrder(Math.round(nextOrder*1000f)/1000f);
//				}
				nextOrder += inc;
				root.store(task);
	
			}
	
			root.commit();
			break;
		}

		return tasks;
	}

	private List<Task> allTasks() {
		final List<Task> searchTasks = new ArrayList<Task>();
		acceptTaskVisitor(new Visitor<Task>() {
			@Override
			public Task visit(Task t) {
				searchTasks.add(t);
				return null;
			}
		});
		return searchTasks;
	}

	private int indexOf(List<Task> allTasks, Task task, int index) {
		for(int i=index;i<allTasks.size();i++) {
			if (task == allTasks.get(i)) {
				return i;
			}
		}
		return -1;
	}

	@Override
	public void checkIn() {

		try {
			final List<Task> dirty = new ArrayList<Task>();
			root.accept(new Visitor<Task>() {
				@Override
				public Task visit(Task t) {
					if (t.isDirty()) {
						dirty.add(t);
					}
					return null;
				}
			});
			
			if (dirty.isEmpty()) {
				MessageBox m = new MessageBox(display.getActiveShell(), SWT.ICON_ERROR | SWT.OK);
				m.setText("Send updates");
				m.setMessage("Nothing to upload");
				m.open();
				return;
			}
			
			MessageBox m = new MessageBox(display.getActiveShell(), SWT.ICON_QUESTION | SWT.YES | SWT.NO);
			m.setText("Send updates");
			m.setMessage("Confirm uploading " + dirty.size() +" issue update(s) to tracker.db4o.com?");
			if (m.open() == SWT.NO) {
				return;
			}
			
			for(Task t : dirty) {
				try {
					System.out.println("updating: "+ t.getKey() + " - " + t.getFineGrainedOrder());
					JiraConnection.updateIssue(jira(), auth(), t.getKey(), t);
					t.resetDirty();
					root.store(t);
					view.taskUpdated(t);
				} catch (RemoteException e) {
					throw new RuntimeException(e);
				}
			}
			root.commit();
		} catch (LoginCanceled e) {
		} catch (RuntimeException e) {
			e.printStackTrace();
			if (e.getCause() instanceof RemoteValidationException) {
				e.printStackTrace();
				MessageBox m = new MessageBox(display.getActiveShell(), SWT.ICON_ERROR | SWT.OK);
				m.setText("Jira Error");
				m.setMessage(e.toString());
				m.open();
			} else {
				throw e;
			}
		}
	}

	@Override
	public void clear() {
		root.clear();
	}

	@Override
	public void fetch() {
		try {
			JiraConnection.populateDb(root, issues());
			display.asyncExec(new Runnable() {
				@Override
				public void run() {
					view.repopulate();
				}
			});
		} catch (LoginCanceled e) {
		} catch (RemoteValidationException e) {
			e.printStackTrace();
			MessageBox m = new MessageBox(display.getActiveShell(), SWT.ICON_ERROR | SWT.OK);
			m.setText("Jira Error");
			m.setMessage(e.toString());
			m.open();
		} catch (Exception e) {
			throw new java.lang.RuntimeException(e);
		}

	}

	private RemoteIssue[] issues() throws RemoteException, com.atlassian.jira.rpc.soap.client.RemoteException {
		return fetchAndSave();
//		return loadFromCache();
	}

	private RemoteIssue[] fetchAndSave() throws RemoteException, com.atlassian.jira.rpc.soap.client.RemoteException {
		RemoteIssue[] ret = jira().getIssuesFromFilter(auth(), "10590");
		
//		try {
//			ObjectOutputStream out = new ObjectOutputStream(new BufferedOutputStream(new FileOutputStream("issues.dat")));
//			out.writeObject(ret);
//			out.flush();
//			out.close();
//		} catch (Exception e) {
//			e.printStackTrace();
//		}
		return ret;
	}

	private RemoteIssue[] loadFromCache() {

		try {
			ObjectInputStream in = new ObjectInputStream(new BufferedInputStream(new FileInputStream("issues.dat")));
			Object ret = in.readObject();
			in.close();
			return (RemoteIssue[]) ret;
		} catch (IOException e) {
			throw new RuntimeException(e);
		} catch (ClassNotFoundException e) {
			throw new RuntimeException(e);
		}
	}

	private String auth() throws RemoteException, LoginCanceled {
		String authToken = soapSession().getAuthenticationToken();
		return authToken;
	}

	private JiraSoapService jira() throws RemoteException, LoginCanceled {
		JiraSoapService jiraSoapService = soapSession().getJiraSoapService();
		return jiraSoapService;
	}

	public SOAPSession soapSession() throws RemoteException, LoginCanceled {
		if (soapSession == null) {

			String[] login = null;
			SOAPSession soapSession;

			do {
				UsernamePasswordDialog user = new UsernamePasswordDialog(display, login == null ? "" : login[0]);

				login = user.open();

				if (login == null) {
					throw new LoginCanceled();
				}

				try {
					soapSession = new SOAPSession(new URL(Edit.TRACKER_URL));
				} catch (MalformedURLException e) {
					throw new RuntimeException(e);
				}

				try {
					if (soapSession.connect(login[0], login[1])) {
						break;
					}
				} catch (RemoteAuthenticationException e) {
					MessageBox m = new MessageBox(display.getActiveShell(), SWT.ICON_ERROR | SWT.OK);
					m.setText("Login Error");
					m.setMessage("Invalid username and/or password");
					m.open();
				}

			} while (true);

			this.soapSession = soapSession;

		}
		return soapSession;
	}

	@Override
	public void estimate(Collection<Task> tasks, int estimate) {
		for(Task t : tasks) {
			t.makeDirty();
			t.setEstimate(estimate);
			root.store(t);
		}
		root.commit();
	}

	@Override
	public void addAssign(Collection<Task> tasks, Resource r) {
		for(Task t : tasks) {
			t.makeDirty();
			t.addResource(r);
			root.store(t.getResources());
			root.store(t);
		}
		root.commit();
	}

	@Override
	public void removeAssign(Collection<Task> tasks, Resource r) {
		for(Task t : tasks) {
			t.makeDirty();
			t.removeResource(r);
			root.store(t.getResources());
			root.store(t);
		}
		root.commit();
	}

	@Override
	public void dropTasks(Collection<Task> tasks) {
		for(Task t : tasks) {
			t.makeDirty();
			t.removeResources();
			t.setIteration(null);
			root.store(t.getResources());
			root.store(t);
		}
		root.commit();
	}

	@Override
	public void setIteration(Collection<Task> tasks, int week) {
		Iteration it = week == 0 ? null : JiraConnection.iteration(root, week);
		for(Task t : tasks) {
			t.makeDirty();
			t.setIteration(it);
			root.store(t);
		}
		root.commit();
	}

	@Override
	public void setLabel(Collection<Task> tasks, String newLabel) {
		String[] label = newLabel.split(" ");
		for(Task t : tasks) {
			t.makeDirty();
			t.setLabel(label);
			root.store(t);
		}
		root.commit();
	}
}

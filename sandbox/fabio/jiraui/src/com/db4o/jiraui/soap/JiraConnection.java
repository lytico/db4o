package com.db4o.jiraui.soap;

import java.util.*;


import com.atlassian.jira.rpc.soap.client.*;
import com.db4o.jiraui.api.*;

public class JiraConnection {

	private static final String[] EMPTY = new String[0];

	public static void updateIssue(JiraSoapService jiraSoapService, String authToken, String key, Task task) throws RemoteException, java.rmi.RemoteException {
		List<RemoteFieldValue> fields = new ArrayList<RemoteFieldValue>();

		Iterator<Resource> r = task.getResources().iterator();
		if (r.hasNext()) {
			fields.add(new RemoteFieldValue("assignee", new String[]{r.next().getId()}));
		} else {
			fields.add(new RemoteFieldValue("assignee", EMPTY));
		}
		List<String> peers = new ArrayList<String>();
		while (r.hasNext()) {
			peers.add(r.next().getId());
		}
		Iteration iteration = task.getIteration();
		fields.add(new RemoteFieldValue("customfield_10000", peers.toArray(new String[peers.size()])));
		fields.add(new RemoteFieldValue("customfield_10020", new String[]{String.format("%d", task.getOrder())}));
		fields.add(new RemoteFieldValue("customfield_10120", task.getLabel()));
		fields.add(new RemoteFieldValue("customfield_10030", iteration == null ? EMPTY : new String[]{String.format("%d", iteration.getId())}));
		fields.add(new RemoteFieldValue("customfield_10040", new String[]{task.getEstimate() == 0 ? "" : String.format("%d", task.getEstimate())}));
		fields.add(new RemoteFieldValue("customfield_10150", new String[]{String.format("%.10f", task.getFineGrainedOrder())}));
		
		jiraSoapService.updateIssue(authToken, key, fields.toArray(new RemoteFieldValue[fields.size()]));
	}

	public static void populateDb(Root root, RemoteIssue[] issues) throws Exception {

		int count = 0;

		float maxOrder = 0;
		
		List<Task> unordered = new ArrayList<Task>();

		for (RemoteIssue issue : issues) {

			Map<String, String[]> custom = parseCustomFields(issue);

			String key = issue.getKey();
			Task task = new Task(key);
			task.setSummary(issue.getSummary());
			tryAddingAssignee(root, task, issue.getAssignee());
			tryAddingAssignee(root, task, custom.get("customfield_10000"));
			task.setStatus(Status.values()[Integer.parseInt("" + issue.getStatus()) - 1]);
			task.setReporter(resource(root, "" + issue.getReporter()));
			task.setProject(project(root, "" + issue.getProject()));
			task.setPriority(Integer.parseInt("" + issue.getPriority()));
			task.setUpdated(issue.getUpdated().getTime());
			task.setCreated(issue.getCreated().getTime());
			String[] order = custom.get("customfield_10020");
			if (order != null && order.length > 0) {
				task.setOrder(Integer.parseInt(order[0]));
			}
			String[] label = custom.get("customfield_10120");
			if (label != null && label.length > 0) {
				task.setLabel(toLowerCase(label));
			}
			String[] estimate = custom.get("customfield_10040");
			if (estimate != null && estimate.length > 0) {
				try {
					task.setEstimate(Integer.parseInt(estimate[0]));
				} catch (NumberFormatException e) {
					System.out.println("Cannot read estimate from issue " + key + ": " + estimate[0]);
				}
			}
			String[] iteration = custom.get("customfield_10030");
			if (iteration != null && iteration.length > 0) {
				task.setIteration(iteration(root, Integer.parseInt(iteration[0])));
			}

			String[] fineOrderString = custom.get("customfield_10150");

			if (fineOrderString != null && fineOrderString.length > 0) {
				float fineOrder = Float.parseFloat(fineOrderString[0]);
				task.setFineGrainedOrder(fineOrder);
				maxOrder = Math.max(maxOrder, fineOrder);
			} else {
				unordered.add(task);
			}

			root.store(task);

			System.out.println("reading: " + (count++) + " - " + task.getKey() + " - " + task.getSummary());
		}
		
		for(Task task : unordered) {
			task.makeDirty();
			task.setFineGrainedOrder(++maxOrder);
			System.out.println("updating: " + String.format("%.1f", task.getFineGrainedOrder()) + " - " + task.getKey() + " - " + task.getSummary());
			root.store(task);
		}
		
		root.commit();
	}

	private static String[] toLowerCase(String[] label) {
		String[] ret = new String[label.length];
		for(int i=0;i<ret.length;i++) {
			ret[i] = label[i].toLowerCase();
		}
		return ret;
	}

	private static Map<String, String[]> parseCustomFields(RemoteIssue issue) {
		Map<String, String[]> custom = new HashMap<String, String[]>();

		RemoteCustomFieldValue[] customs = issue.getCustomFieldValues();
		for (RemoteCustomFieldValue c : customs) {
			custom.put(c.getCustomfieldId(), c.getValues());
		}
		return custom;
	}

	private static void tryAddingAssignee(Root root, Task task, String... assignees) {
		if (assignees == null || assignees.length == 0) {
			return;
		}
		for (String a : assignees) {
			if (a == null) continue;
			a = a.trim();
			if (a.isEmpty()) continue;
			System.out.println("----> " + a);
			task.addResource(resource(root, a));
		}
	}

	private static Resource resource(Root root, String resource) {
		Resource r = root.resource(resource);
		if (r == null) {
			r = new Resource(resource);
			fillResource(r);
			root.store(r);
		}
		return r;
	}

	private static String[][] knownNames = {
			{"fabioroger", "fabio"},	
			{"patrick roemer", "patrick"},	
			{"carl@db4o.com", "carl"},	
			{"adriano verona", "adriano"},	
			{"gamlerhart", "roman"},	
	};
	
	private static void fillResource(Resource r) {
		for(String[] pair : knownNames) {
			if (r.getId().equals(pair[0])) {
				r.setName(pair[1]);
				r.setFavorite(true);
				return;
			}
		}
		r.setName(r.getId());
	}

	public static Iteration iteration(Root root, int id) {
		Iteration p = root.iteration(id);
		if (p == null) {
			p = new Iteration(id);
			root.store(p);
		}
		return p;
	}

	private static Project project(Root root, String project) {
		Project p = root.project(project);
		if (p == null) {
			p = new Project(project);
			root.store(p);
		}
		return p;
	}

}

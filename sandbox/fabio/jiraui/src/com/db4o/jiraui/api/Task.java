package com.db4o.jiraui.api;

import java.util.*;

public class Task {

	private String key;
	private String summary;
	private String description;
	private Project project;
	private Set<Resource> resources;
	private Resource reporter;
	private Task parent;
	private int estimate;
	private Status status;
	private Iteration iteration;
	private List<Comment> comments;
	private Date updated;
	private Date created;
	private int priority;
	
	private float fineGrainedOrder;
	
	private int order;
	
	private boolean dirty = false;
	
	private String[] label;
	
	public Task(String key) {
		this.key = key;
	}
	public String getKey() {
		return key;
	}
	public void setKey(String key) {
		this.key = key;
	}
	public Project getProject() {
		return project;
	}
	public void setProject(Project project) {
		this.project = project;
	}
	public Set<Resource> getResources() {
		if (resources == null) {
			resources = new LinkedHashSet<Resource>();
		}
		return resources;
	}
	public void setResources(Set<Resource> resources) {
		this.resources = resources;
	}
	public Task getParent() {
		return parent;
	}
	public void setParent(Task parent) {
		this.parent = parent;
	}
	public int getEstimate() {
		return estimate;
	}
	public void setEstimate(int estimate) {
		this.estimate = estimate;
	}
	public Status getStatus() {
		return status;
	}
	public void setStatus(Status status) {
		this.status = status;
	}
	public Iteration getIteration() {
		return iteration;
	}
	public void setIteration(Iteration iteration) {
		this.iteration = iteration;
	}
	public List<Comment> getComments() {
		return comments;
	}
	public void setComments(List<Comment> comments) {
		this.comments = comments;
	}
	public void setDescription(String description) {
		this.description = description;
	}
	public String getDescription() {
		return description;
	}
	public void addResource(Resource resource) {
		getResources().add(resource);
	}
	public void removeResource(Resource resource) {
		getResources().remove(resource);
	}
	public void removeResources() {
		getResources().clear();
	}
	public void setReporter(Resource reporter) {
		this.reporter = reporter;
	}
	public Resource getReporter() {
		return reporter;
	}
	public void setUpdated(Date updated) {
		this.updated = updated;
	}
	public Date getUpdated() {
		return updated;
	}
	public void setCreated(Date created) {
		this.created = created;
	}
	public Date getCreated() {
		return created;
	}
	public void setPriority(int priority) {
		this.priority = priority;
	}
	public int getPriority() {
		return priority;
	}
	public void setSummary(String summary) {
		this.summary = summary;
	}
	public String getSummary() {
		return summary;
	}
	public void setFineGrainedOrder(float order) {
		this.fineGrainedOrder = order;
	}
	public float getFineGrainedOrder() {
		return fineGrainedOrder;
	}
	
	public void makeDirty() {
		dirty = true;
	}
	
	public void resetDirty() {
		dirty = false;
	}
	
	public boolean isDirty() {
		return dirty;
	}
	
	public void setOrder(int order) {
		this.order = order;
	}

	public int getOrder() {
		return order;
	}
	public void setLabel(String[] label) {
		this.label = label;
	}
	public String[] getLabel() {
		return label;
	}
	
}

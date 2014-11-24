package com.db4o.jiraui;

import java.util.*;

import com.db4o.jiraui.api.*;


public interface EditTasksController {

	Task acceptTaskVisitor(Visitor<Task> visitor);

	Collection<Task> moveTasks(Task taskBefore, Task taskAfter, Collection<Task> tasks, int order);

	void checkIn();

	void clear();

	void fetch();

	void estimate(Collection<Task> selectedItems, int estimate);

	void addAssign(Collection<Task> tasks, Resource r);

	void removeAssign(Collection<Task> tasks, Resource r);

	void dropTasks(Collection<Task> tasks);

	void setIteration(Collection<Task> selectedTasks, int week);

	void setLabel(Collection<Task> task, String newLabel);

}

package de.sangamon.freespace.core;

public interface FreespaceManager {
	Freespace acquire();
	void free(Freespace freed);
	void shutdown();
}

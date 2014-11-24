package com.db4o.io;

import com.db4o.foundation.*;

/**
 * Block size registry.
 * 
 * Accessible through the environment. 
 * 
 * @see Environments#my(Class)
 * @since 7.7
 */
public interface BlockSize {

	void register(Listener4<Integer> listener);

	void set(int newValue);

	int value();
}

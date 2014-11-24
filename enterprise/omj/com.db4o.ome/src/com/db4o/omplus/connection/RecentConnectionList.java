/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.connection;

import java.util.*;

public interface RecentConnectionList {

	<T extends ConnectionParams> List<T> getRecentConnections(Class<T> paramType);

	<T extends ConnectionParams> void addNewConnection(T params);

}
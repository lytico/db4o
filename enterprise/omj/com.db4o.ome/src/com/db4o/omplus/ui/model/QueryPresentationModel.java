package com.db4o.omplus.ui.model;

import com.db4o.omplus.*;

public class QueryPresentationModel {

	private ErrorMessageHandler err;
	
	public QueryPresentationModel(ErrorMessageHandler err) {
		this.err = err;
	}
	
	public ErrorMessageHandler err() {
		return err;
	}
	
}

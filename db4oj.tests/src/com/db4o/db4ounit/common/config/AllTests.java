/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.config;

import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
        return composeTests(
        		new Class[] {
        				ClassConfigOverridesGlobalConfigTestSuite.class,
			        	ConfigurationItemTestCase.class,
			        	ConfigurationOfObjectClassNotSupportedTestCase.class,
			        	Config4ImplTestCase.class,
			        	CustomStringEncodingTestCase.class,
			        	EmbeddedConfigurationItemIntegrationTestCase.class,
			        	EmbeddedConfigurationItemUnitTestCase.class,
			        	GlobalVsNonStaticConfigurationTestCase.class,
			        	LatinStringEncodingTestCase.class,
			        	ObjectContainerCustomNameTestCase.class,
			        	ObjectTranslatorTestCase.class,
			        	TransientConfigurationTestSuite.class,
			        	UnicodeStringEncodingTestCase.class,
			        	UTF8StringEncodingTestCase.class,
			        	VersionNumbersTestCase.class,
        		});        
    }

	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	@Override
	protected Class[] composeWith() {
		return new Class[] {				
			ConfigurationReuseTestSuite.class, // Uses Client/Server
		};
	}
	
}

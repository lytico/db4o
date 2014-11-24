package db4ounit.extensions.fixtures;

/**
 * Marker interface to denote that implementing test cases should be excluded
 * from running with any fixture but a networking C/S one.
 */
public interface OptOutAllButNetworkingCS extends OptOutSolo {
}

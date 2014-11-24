/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.consistency;

import java.util.*;

import com.db4o.foundation.*;

public class ConsistencyReport {
	
	private static final int MAX_REPORTED_ITEMS = 50;
	final List<SlotDetail> bogusSlots;
	final OverlapMap overlaps;
	final List<Pair<String,Integer>> invalidObjectIds;
	final List<Pair<String,Integer>> invalidFieldIndexEntries;
	
	ConsistencyReport(
			List<SlotDetail> bogusSlots, 
			OverlapMap overlaps, 
			List<Pair<String,Integer>> invalidClassIds, 
			List<Pair<String,Integer>> invalidFieldIndexEntries) {
		this.bogusSlots = bogusSlots;
		this.overlaps = overlaps;
		this.invalidObjectIds = invalidClassIds;
		this.invalidFieldIndexEntries = invalidFieldIndexEntries;
	}
	
	public boolean consistent() {
		return bogusSlots.size() == 0 && overlaps.overlaps().size() == 0 && overlaps.dupes().size() == 0 && invalidObjectIds.size() == 0 && invalidFieldIndexEntries.size() == 0;
	}
	
	public Set<Pair<SlotDetail, SlotDetail>> overlaps() {
		return overlaps.overlaps();
	}

	public Set<Pair<SlotDetail, SlotDetail>> dupes() {
		return overlaps.dupes();
	}

	@Override
	public String toString() {
		if(consistent()) {
			return "no inconsistencies detected";
		}
		StringBuffer message = new StringBuffer("INCONSISTENCIES DETECTED\n")
			.append(overlaps.overlaps().size() + " overlaps\n")
			.append(overlaps.dupes().size() + " dupes\n")
			.append(bogusSlots.size() + " bogus slots\n")
			.append(invalidObjectIds.size() + " invalid class ids\n")
			.append(invalidFieldIndexEntries.size() + " invalid field index entries\n");
		message.append("(slot lengths are non-blocked)\n");
		appendInconsistencyReport(message, "OVERLAPS", overlaps.overlaps());
		appendInconsistencyReport(message, "DUPES", overlaps.dupes());
		appendInconsistencyReport(message, "BOGUS SLOTS", bogusSlots);
		appendInconsistencyReport(message, "INVALID OBJECT IDS", invalidObjectIds);
		appendInconsistencyReport(message, "INVALID FIELD INDEX ENTRIES", invalidFieldIndexEntries);
		return message.toString();
	}
	
	private <T> void appendInconsistencyReport(StringBuffer str, String title, Collection<T> entries) {
		if(entries.size() != 0) {
			str.append(title + "\n");
			int count = 0;
			for (T entry : entries) {
				str.append(entry).append("\n");
				count++;
				if(count > MAX_REPORTED_ITEMS) {
					str.append("and more...\n");
					break;
				}
			}
		}
	}
}
/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.querying;

import com.db4o.activation.*;
import com.db4o.config.*;
import com.db4o.query.Query;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.AbstractDb4oTestCase;

import java.util.*;

/** @sharpen.if !SILVERLIGHT */
public class NonPrimitiveSortingTestCase extends AbstractDb4oTestCase{

    public static void main(String[] args) {
        new NonPrimitiveSortingTestCase().runSolo();
    }
    
    private static final ReleaseNote rn1 = new ReleaseNote("foo", "no comments");
    private static final ReleaseNote rn2 = new ReleaseNote("bar", "next after the best ever");
    private static final ReleaseNote rn3 = new ReleaseNote("foo.bar", "the best ever");
    
    private static final List<Holder> reference = Arrays.asList(
            new Holder(new Version(1, 0, 0, rn1 )),
            new Holder(new Version(3, 0, 0, rn2 )),
            new Holder(new Version(2, 0, 0, rn3 )),
            new Holder(new Version(1, 1, 0, rn1 )),
            new Holder(new Version(1, 0, 1, rn2 )),
            new Holder(new Version(1, 0, 0, rn2 )),
            new Holder(new Version(2, 0, 1, rn3 )),
            new Holder(new Version(2, 1, 0, rn1 )),
            new Holder(new Version(2, 0, 2, rn2 ))
    );
    
    @Override
    protected void configure(Configuration config) throws Exception {
    	config.add(new TransparentActivationSupport());
    }
    @Override
    protected void store() throws Exception {
        for (Holder holder : reference) {
            db().store(holder);
        }
    }

    public void testSorting(){
        Query query = db().query();
        query.constrain(Holder.class);
        query.descend("version").orderAscending();

        List<Holder> queryResult = query.execute();
        List<Holder> referenceSorted = referencSorting();

        Iterator<Holder> db4oIt = queryResult.iterator();
        Iterator<Holder> referenceIt = referenceSorted.iterator();
        
        Assert.areEqual(referenceSorted.size(), queryResult.size());
        while(db4oIt.hasNext() && referenceIt.hasNext()){
            Holder db4o = db4oIt.next();
            Holder reference = referenceIt.next();
            
            Version expected = reference.getVersion();
            Assert.areEqual(db4o.getVersion(), expected);
        }

    }

    private List<Holder> referencSorting() {
        ArrayList<Holder> referencesorted = new ArrayList<Holder>(reference);
        Collections.sort(referencesorted, new Comparator<Holder>() {
            public int compare(Holder o1, Holder o2) {
                return o1.getVersion().compareTo(o2.getVersion());
            }
        });
        return referencesorted;
    }

    private static class Holder {
        private final Version version;

        public Holder(Version version) {
            this.version = version;
        }

        public Version getVersion() {
            return version;
        }
    }
    
    private static class ReleaseNote implements Comparable<ReleaseNote> {
		private String remarks;
    	private String authorName;
    	
    	public ReleaseNote(String authorName, String remarks) {
			this.authorName = authorName;
			this.remarks = remarks;
		}

		public int compareTo(ReleaseNote other) {
			int cmp = authorName.compareTo(other.authorName);
			if (cmp == 0) {
				cmp = remarks.compareTo(other.remarks);
			}
				
			return cmp;
		}
		
		@Override
		public int hashCode() {
			return remarks.hashCode() ^ authorName.hashCode();
		}
		
		@Override
		public String toString() {
			return "{" + getClass().getSimpleName() + ": " + authorName + " / " + remarks + "}";
		}
    }
    
    private static class Version implements Comparable<Version>, Activatable  {
        private final int major;
        private final int minor;
        private final int bugFix;
        private ReleaseNote notes;
        
		private transient Activator _activator;

        public Version(int major, int minor, int bugFix, ReleaseNote notes) {
            this.major = major;
            this.minor = minor;
            this.bugFix = bugFix;
            this.notes = notes;
        }

        public int compareTo(Version o) {
        	activate(ActivationPurpose.READ);
        	
            int comparison = new Integer(major).compareTo(o.major);
            if(0==comparison){
                comparison = new Integer(minor).compareTo(o.minor);
            }
            if(0==comparison){
                comparison = new Integer(bugFix).compareTo(o.bugFix);
            }
            if (0 == comparison) {
            	comparison = notes.compareTo(o.notes);
            }
            
            return comparison;
        }

        @Override
        public boolean equals(Object o) {
        	activate(ActivationPurpose.READ);
        	
            if (this == o) return true;
            if (o == null || getClass() != o.getClass()) return false;

            Version other = (Version) o;

            if (bugFix != other.bugFix) return false;
            if (major != other.major) return false;
            if (minor != other.minor) return false;            
            if (notes.compareTo(other.notes) != 0) return false;

            return true;
        }

        @Override
        public int hashCode() {
        	activate(ActivationPurpose.READ);
        	
            int result = major;
            result = 31 * result + minor;
            result = 31 * result + bugFix;
            result = 31 * result + notes.hashCode();
            
            return result;
        }
        
        @Override
        public String toString() {
        	activate(ActivationPurpose.READ);
        	return Version.class.getSimpleName() + " [" + major + "." + minor + "." + bugFix + " - " +  notes + "]";
        }

		public void bind(Activator activator) {
			_activator = activator;
		}

		public void activate(ActivationPurpose purpose) {
			if (_activator != null) {
				_activator.activate(purpose);
			}
		}
    }
}
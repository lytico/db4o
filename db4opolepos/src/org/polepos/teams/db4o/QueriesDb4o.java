/* 
This file is part of the PolePosition database benchmark
http://www.polepos.org

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public
License along with this program; if not, write to the Free
Software Foundation, Inc., 59 Temple Place - Suite 330, Boston,
MA  02111-1307, USA. */


package org.polepos.teams.db4o;

import org.polepos.circuits.queries.*;
import org.polepos.runner.db4o.*;

import com.db4o.config.*;
import com.db4o.query.*;


public class QueriesDb4o extends Db4oDriver implements QueriesDriver{
    
	private static int maximumPayload;
    
	@Override
	public void configure(Configuration config) {
		indexField(config, IndianapolisList.class  , fieldNext());
		indexField(config, IndianapolisList.class  , fieldPayload());
	}

    public void write() {
        IndianapolisList list = IndianapolisList.generate(setup().getObjectCount());
        maximumPayload = list.getPayload();
        begin();
        store(list);
        commit();
    }
    
    public void queryAndBigResult() {
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            Query q = newIndianapolisListQuery();
            Constraint c1 = q.descend(fieldPayload()).constrain(new Integer(0)).greater();
            Constraint c2 = q.descend(fieldPayload()).constrain(new Integer(maximumPayload)).smaller();
            c1.and(c2);
            doQuery(q);
        }
    }
    
    public void queryNestedAnd() {
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            Query q = newIndianapolisListQuery();
            Constraint c1 = q.descend(fieldPayload()).constrain(new Integer(0)).greater();
            Constraint c2 = q.descend(fieldPayload()).constrain(new Integer(1)).greater();
            Constraint c3 = q.descend(fieldPayload()).constrain(new Integer(2)).greater();
            Constraint c4 = q.descend(fieldPayload()).constrain(new Integer(3)).greater();
            Constraint and1 = c1.and(c2);
            Constraint and2 = c3.and(c4);
            and1.and(and2);
            doQuery(q);
        }
    }

    public void queryTwoFieldAnd() {
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            Query q = newIndianapolisListQuery();
            Constraint c1 = q.descend(fieldPayload()).constrain(new Integer(maximumPayload - 1)).greater();
            Constraint c2 = q.descend(fieldNext()).constrain(null);
            c1.and(c2);
            doQuery(q);
        }
    }
    
    public void queryTwoFieldAndNot() {
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            Query q = newIndianapolisListQuery();
            Constraint c1 = q.descend(fieldPayload()).constrain(new Integer(maximumPayload / 2)).greater();
            Constraint c2 = q.descend(fieldNext()).constrain(null).not();
            c1.and(c2);
            doQuery(q);
        }
    }
    
    public void queryRange(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            Query q = newIndianapolisListQuery();
            Query qPayload = q.descend(fieldPayload());
            qPayload.constrain(new Integer(1)).greater();
            qPayload.constrain(new Integer(3)).smaller();
            doQuery(q);
        }
    }

    public void query5Links(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            Query q = db().query();
            Query qChild = q;
            for (int j = 0; j < 5; j++) {
                qChild = qChild.descend(fieldNext());
            }
            qChild.descend(fieldPayload()).constrain(new Integer(1));
            doQuery(q);
        }
    }
    
    public void queryPreferShortPath(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            Query q = newIndianapolisListQuery();
            q.descend(fieldNext()).descend(fieldNext()).descend(fieldPayload()).constrain(new Integer(maximumPayload - 4));
            q.descend(fieldNext()).descend(fieldPayload()).constrain(new Integer(maximumPayload - 2));
            doQuery(q);
        }
    }
    
    public void queryOr(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            Query q = newIndianapolisListQuery();
            Constraint cMax = q.descend(fieldPayload()).constrain(new Integer(maximumPayload));
            Constraint cMin = q.descend(fieldPayload()).constrain(new Integer(1));
            cMax.or(cMin);
            doQuery(q);
        }
    }
    
    public void queryOrRange(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            Query q = newIndianapolisListQuery();
            Constraint c1 = q.descend(fieldPayload()).constrain(new Integer(1)).greater();
            Constraint c2 = q.descend(fieldPayload()).constrain(new Integer(3)).smaller();
            Constraint c3 = q.descend(fieldPayload()).constrain(new Integer(maximumPayload - 2)).greater();
            Constraint c4 = q.descend(fieldPayload()).constrain(new Integer(maximumPayload)).smaller();
            Constraint cc1 = c1.and(c2);
            Constraint cc2 = c3.and(c4);
            cc1.or(cc2);
            doQuery(q);
        }
    }
    
    public void queryNotGreater(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            Query q = newIndianapolisListQuery();
            q.descend(fieldPayload()).constrain(new Integer(2)).greater().not();
            doQuery(q);
        }
    }
    
    public void queryNotRange(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            Query q = newIndianapolisListQuery();
            Constraint c1 = q.descend(fieldPayload()).constrain(new Integer(2)).greater();
            Constraint c2 = q.descend(fieldPayload()).constrain(new Integer(maximumPayload)).smaller();
            c1.and(c2).not();
            doQuery(q);
        }
    }
    
    public void queryOrTwoLevels(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            doQuery(twoLevelsOrQuery());
        }
    }
    
    public void queryBigRangeFound(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            doQuery(bigRangeQuery());
        }
    }
    
    public void queryTwoLevelBigRangeFound(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            doQuery(twoLevelBigRangeQuery());
        }
    }
    
    public void queryByChildIdentity(){
        
        Query q = newIndianapolisListQuery();
        q.descend(fieldPayload()).constrain(new Integer(2));
        IndianapolisList il = (IndianapolisList) q.execute().next();
        
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            q = newIndianapolisListQuery();
            q.descend(fieldNext()).constrain(il).identity();
            doQuery(q);
        }
    }
    
    public void getSingleRandomObject(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
        	IndianapolisList il = getOne(newIndianapolisListQuery());
        	db().purge(il);
        }
    }
    
    public void getOneFromBigRangeQuery(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            getOne(bigRangeQuery());
        }
    }
    
    public void getOneFromTwoLevelBigRangeQuery(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
            getOne(twoLevelBigRangeQuery());
        }
    }
    
    public void getOneFromOrTwoLevelsQuery(){
        int count = setup().getSelectCount();
        for (int i = 1; i <= count; i++) {
        	getOne(twoLevelsOrQuery());
        }
    }
    
    private Query twoLevelsOrQuery(){
        Query q = newIndianapolisListQuery();
        Constraint c1 = q.descend(fieldPayload()).constrain(new Integer(2)).smaller();
        Constraint c2 = q.descend(fieldNext()).descend(fieldPayload()).constrain(new Integer(maximumPayload - 2)).greater();
        c1.or(c2);
        return q;
    }
    
    private Query twoLevelBigRangeQuery(){
        Query q = newIndianapolisListQuery();
        Constraint c1 = q.descend(fieldPayload()).constrain(new Integer(1)).greater();
        Constraint c2 = q.descend(fieldNext()).descend(fieldPayload()).constrain(new Integer(maximumPayload)).smaller();
        c1.or(c2);
        return q;
    }
    
    private Query bigRangeQuery(){
        Query q = newIndianapolisListQuery();
        q.descend(fieldPayload()).constrain(new Integer(1)).greater();
        q.descend(fieldPayload()).constrain(new Integer(maximumPayload)).smaller();
        return q;
    }

    
    private IndianapolisList getOne(Query q){
    	return (IndianapolisList) q.execute().next();
    }
    
    private Query newIndianapolisListQuery() {
        Query q = db().query();
        q.constrain(IndianapolisList.class);
        return q;
    }
    
    private String fieldNext(){
        return IndianapolisList.FIELD_NEXT;
    }
    
    private String fieldPayload(){
        return IndianapolisList.FIELD_PAYLOAD;
    }


}

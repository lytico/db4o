/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */
package com.db4o.inside.ix;

import com.db4o.*;
import com.db4o.foundation.*;

/**
 * @exclude
 */
public class QxProcessor {
    
    private Tree _paths;
    
    private QxPath _best;
    
    private int _limit;

    void addPath(final QxPath newPath){
        
        if(_paths == null){
            _paths = newPath;
            return;
        }
        
        if(! Debug.useNIxPaths){
            _paths = Tree.add(_paths, newPath);
            return;
        }
        
        final QxPath[] same = new QxPath[] {null};
        
        _paths.traverse(new Visitor4() {
            public void visit(Object a_object) {
                QxPath path = (QxPath)a_object;
                if(path._parent == newPath._parent){
                    if(path.onSameFieldAs(newPath)){
                        same[0] = path;
                    }
                }
            }
        });
        
        if(same[0] != null){
            _paths = _paths.removeNode(same[0]);
            newPath.mergeForSameField(same[0]);
        }
        
        _paths = Tree.add(_paths, newPath);
    }
    
    private void buildPaths(QCandidates candidates){
        Iterator4 i = candidates.iterateConstraints();
        while(i.hasNext()){
            QCon qCon = (QCon)i.next();
            qCon.setCandidates(candidates);
            if(! qCon.hasOrJoins()){
                new QxPath(this, null, qCon, 0).buildPaths();
            }
        }
    }

    public boolean run(QCandidates candidates, int limit){
        _limit = limit;
        buildPaths(candidates);
        if(_paths == null){
            return false;
        }
        return chooseBestPath();
    }
    
    private boolean chooseBestPath(){
        while(_paths != null){
            QxPath path = (QxPath)_paths.first();
            _paths = _paths.removeFirst();
            if(path.isTopLevelComplete()){
                _best = path;
                return true;
            }
            path.load();
        }
        return false;
    }
    
    public Tree toQCandidates(QCandidates candidates){
        return _best.toQCandidates(candidates);
    }
    
    boolean exceedsLimit(int count, int depth){
        int limit = _limit;
        for (int i = 0; i < depth; i++) {
            limit = limit / 10;
        }
        return count > limit;
    }

}

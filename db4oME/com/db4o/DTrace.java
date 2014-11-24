/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;


/**
 * @exclude 
 */
public class DTrace {
    
    public static final boolean enabled = false;
    
    private static void breakPoint(){
        int placeBreakPointHere = 1;
    }
    
    private static final Object init(){
        if(enabled){
            
            // breakOnEvent(15);

            // addRange(4874);
            
            // addRangeWithEnd(3835808, 3836267);
            
            // addRangeWithLength(10666,1);
            
            // addRangeWithLength(1013,1);
            
            // addRangeWithLength(3212,1);
            
            // addRangeWithLength(0,Integer.MAX_VALUE);

            
            trackEventsWithoutRange();
            
            // addRangeWithLength(3181,1);
            
            addRangeWithLength(3243,1);
            
            

            
            
            ADD_TO_CLASS_INDEX = new DTrace(true, true, "add to class index tree", true);
            BIND = new DTrace(true, true, "bind", true);
            CANDIDATE_READ = new DTrace(true, true, "candidate read", true);
            CLOSE = new DTrace(true, true, "close", true);
            COLLECT_CHILDREN = new DTrace(true, true, "collect children", true);
            COMMIT = new DTrace(false, false, "commit", true);
            CONTINUESET = new DTrace(true, true, "continueset", true);
            CREATE_CANDIDATE = new DTrace(true, true, "create candidate", true);
            DELETE = new DTrace(true, true, "delete", true);
            DONOTINCLUDE = new DTrace(true, true, "donotinclude", true);
            EVALUATE_SELF = new DTrace(true, true, "evaluate self", true);
            FREE = new DTrace(true, true, "free", true);
            FILE_FREE = new DTrace(true, true, "fileFree", true);
            FREE_RAM = new DTrace(true, true, "freeRAM", true);
            FREE_ON_COMMIT = new DTrace(true, true, "trans freeOnCommit", true);
            FREE_ON_ROLLBACK = new DTrace(true, true, "trans freeOnRollback", true);
            GET_SLOT = new DTrace(true, true, "getSlot", true);
            GET_FREESPACE = new DTrace(true, true, "getFreespace", true);
            GET_FREESPACE_RAM = new DTrace(true, true, "getFreespaceRam", true);
            GET_YAPOBJECT = new DTrace(true, true, "get yapObject", true);
            ID_TREE_ADD = new DTrace(true, true, "id tree add", true);
            ID_TREE_REMOVE = new DTrace(true, true, "id tree remove", true);
            IO_COPY = new DTrace(true, true, "io copy", true);
            JUST_SET = new DTrace(true, true, "just set", true);
            NEW_INSTANCE = new DTrace(true, true, "newInstance", true);
            QUERY_PROCESS = new DTrace(true, true, "query process", true);
            READ_ARRAY_WRAPPER = new DTrace(true, true, "read array wrapper", true);
            READ_BYTES = new DTrace(true, true, "readBytes", true); 
            READ_ID = new DTrace(true, true, "read ID", true);
            READ_SLOT = new DTrace(true, true, "read slot", true);
            REFERENCE_REMOVED = new DTrace(true, true, "reference removed", true);
            REGULAR_SEEK = new DTrace(true, true, "regular seek", true);
            REMOVE_FROM_CLASS_INDEX = new DTrace(true, true, "trans removeFromClassIndexTree", true);
            TRANS_COMMIT = new DTrace(false, false, "trans commit", false);
            TRANS_DELETE = new DTrace(true, true, "trans delete", true);
            TRANS_DONT_DELETE = new DTrace(true, true, "trans dontDelete", true);
            TRANS_FLUSH = new DTrace(true, true, "trans flush", true);
            
            YAPCLASS_BY_ID = new DTrace(true, true, "yapclass by id", true);
            YAPCLASS_INIT = new DTrace(true, true, "yapclass init", true);
            WRITE_BYTES = new DTrace(true, true, "writeBytes", true); 
            WRITE_POINTER = new DTrace(true, true, "write pointer", true);
            WRITE_UPDATE_DELETE_MEMBERS = new DTrace(true, true, "trans writeUpdateDeleteMembers", true);
            WRITE_XBYTES = new DTrace(true, true, "writeXBytes", true); 
            
            // turnAllOffExceptFor(new DTrace[] {GET_SLOT, FREE_ON_COMMIT, FREE, WRITE_BYTES});
            
            // turnAllOffExceptFor(new DTrace[] {WRITE_POINTER});
            
         
        }
        return null;
    }
    
    private static void trackEventsWithoutRange() {
        _trackEventsWithoutRange = true;
    }

    private DTrace(boolean enabled_, boolean break_, String tag_, boolean log_){
        if(enabled){
            _enabled = enabled_;
            _break = break_;
            _tag = tag_;
            _log = log_;
            if(all == null){
                all = new DTrace[100];
            }
            all[current++] = this;
        }
    }
    
    private boolean _enabled;
    private boolean _break;
    private boolean _log;
    private String _tag;
    
    private static long[] _rangeStart;
    private static long [] _rangeEnd;
    private static int _rangeCount;
    
    public static long _eventNr;
    private static long[] _breakEventNrs;
    private static int _breakEventCount;
    
    private static boolean _trackEventsWithoutRange;

    
    public static DTrace ADD_TO_CLASS_INDEX;
    public static DTrace BIND;
    public static DTrace CANDIDATE_READ;
    public static DTrace CLOSE;
    public static DTrace COLLECT_CHILDREN;
    public static DTrace COMMIT;
    public static DTrace CONTINUESET;
    public static DTrace CREATE_CANDIDATE;
    public static DTrace DELETE;
    public static DTrace DONOTINCLUDE;
    public static DTrace EVALUATE_SELF;
    public static DTrace FILE_FREE;
    public static DTrace FREE;
    public static DTrace FREE_RAM;
    public static DTrace FREE_ON_COMMIT;
    public static DTrace FREE_ON_ROLLBACK;
    public static DTrace GET_SLOT;
    public static DTrace GET_FREESPACE;
    public static DTrace GET_FREESPACE_RAM;
    public static DTrace GET_YAPOBJECT;
    public static DTrace ID_TREE_ADD;
    public static DTrace ID_TREE_REMOVE;
    public static DTrace IO_COPY;
    public static DTrace JUST_SET;
    public static DTrace NEW_INSTANCE;
    public static DTrace QUERY_PROCESS;
    public static DTrace READ_ARRAY_WRAPPER;
    public static DTrace READ_BYTES;
    public static DTrace READ_ID;
    public static DTrace READ_SLOT;
    public static DTrace REFERENCE_REMOVED;
    public static DTrace REGULAR_SEEK;
    public static DTrace REMOVE_FROM_CLASS_INDEX;
    public static DTrace TRANS_COMMIT;
    public static DTrace TRANS_DONT_DELETE;
    public static DTrace TRANS_DELETE;
    public static DTrace TRANS_FLUSH;
    public static DTrace YAPCLASS_BY_ID;
    public static DTrace YAPCLASS_INIT;
    public static DTrace WRITE_BYTES;
    public static DTrace WRITE_POINTER;
    public static DTrace WRITE_XBYTES;
    public static DTrace WRITE_UPDATE_DELETE_MEMBERS;
    
    private static final Object forInit = init();
    
    private static DTrace all[];
    private static int current;
    
    public void log(){
        if(enabled){
            log(-1);
        }
    }
    
    public void log(long p){
        if(enabled){
            logLength(p, 1);
        }
    }
    
    public void logInfo(String info){
        if(enabled){
            logEnd(-1,0, info );
        }
    }
    
    public void log(long p, String info){
        if(enabled){
            logEnd(p, 0, info);
        }
        
    }
    
    public void logLength(long start, long length){
        if(enabled){
            logEnd(start, start + length - 1);
        }
    }
    
    public void logEnd(long start, long end){
        if(enabled){
            logEnd(start, end, null);
        }
    }
    
    public void logEnd(long start, long end, String info){
        if(enabled){
            if(! _enabled){
                return;
            }
            boolean inRange = false;
            
            if(_rangeCount == 0){
                inRange = true;
            }
            
            for (int i = 0; i < _rangeCount; i++) {
                
                // Case 1 start in range
                if(start >= _rangeStart[i] && start <= _rangeEnd[i]){
                    inRange = true;
                    break;
                }
                
                if(end != 0){
                    
                    // Case 2 end in range
                    if (end >= _rangeStart[i] && end <= _rangeEnd[i]){
                        inRange = true;
                        break;
                    }
                    
                    // Case 3 start before range, end after range
                    if(start <= _rangeStart[i]  && end >= _rangeEnd[i]){
                        inRange = true;
                        break;
                    }
                }
            }
            if(inRange || ( _trackEventsWithoutRange &&  (start == -1) )){
                if(_log){
                    _eventNr ++;
                    StringBuffer sb = new StringBuffer(":");
                    sb.append(formatInt(_eventNr, 6));
                    sb.append(":");
                    if(start != 0){
                        sb.append(formatInt(start));
                        sb.append(":");
                    }
                    if(end != 0  && start != end){
                        sb.append(formatInt(end));
                        sb.append(":");
                        sb.append(formatInt(end - start + 1));
                    }else{
                        sb.append(formatInt(0));
                    }
                    sb.append(":");
                    if(info != null){
                        sb.append(" " + info + " ");
                        sb.append(":");
                    }
                    sb.append(" ");
                    sb.append(_tag);
                    System.out.println(sb);
                }
                if(_break){
                    if(_breakEventCount > 0){
                        for (int i = 0; i < _breakEventCount; i++) {
                            if(_breakEventNrs[i] == _eventNr){
                                breakPoint();
                                break;
                            }
                        }
                    }else{
                        breakPoint();
                    }
                }
            }
        }
    }
    
    public static void addRange(long pos){
        if(enabled){
            addRangeWithEnd(pos, pos);
        }
    }
    
    public static void addRangeWithLength(long start, long length){
        if(enabled){
            addRangeWithEnd(start, start + length - (long)1);
        }
    }
    
    public static void addRangeWithEnd(long start, long end){
        if(enabled){
            if(_rangeStart == null){
                _rangeStart = new long[100];
                _rangeEnd = new long[100];
            }
            _rangeStart[_rangeCount] = start;
            _rangeEnd[_rangeCount] = end;
            _rangeCount++;
        }
    }
    
    private static void breakOnEvent(long eventNr){
        if(enabled){
            if(_breakEventNrs == null){
                _breakEventNrs = new long[100];
            }
            _breakEventNrs[_breakEventCount] = eventNr;
            _breakEventCount ++;
        }
    }
    
    
    private String formatInt(long i, int len){
        if(enabled){
            String str = "              ";
            if( i != 0){
                str += i + " ";
            }
            return str.substring(str.length() - len);
        }
        return null;
    }
    
    private String formatInt(long i){
        if(enabled){
            return formatInt(i, 10);
        }
        return null;
    }
    
    private static void turnAllOffExceptFor(DTrace[] these){
        if(enabled){
            for (int i = 0; i < all.length; i++) {
                if(all[i] == null){
                    break;
                }
                boolean turnOff = true;
                for (int j = 0; j < these.length; j++) {
                    if(all[i] == these[j]){
                        turnOff = false;
                        break;
                    }
                }
                if(turnOff){
                    all[i]._break = false;
                    all[i]._enabled = false;
                    all[i]._log = false;
                }
            }
        }
    }
    
}

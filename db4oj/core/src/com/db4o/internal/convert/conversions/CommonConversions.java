/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.convert.conversions;

import com.db4o.internal.convert.*;

/**
 * @exclude
 */
public class CommonConversions {
    
    public static void register(Converter converter){
        converter.register(ClassIndexesToBTrees_5_5.VERSION, new ClassIndexesToBTrees_5_5());
        converter.register(FieldIndexesToBTrees_5_7.VERSION, new FieldIndexesToBTrees_5_7());
        converter.register(ClassAspects_7_4.VERSION, new ClassAspects_7_4());
        converter.register(ReindexNetDateTime_7_8.VERSION, new ReindexNetDateTime_7_8());
        converter.register(DropEnumClassIndexes_7_10.VERSION, new DropEnumClassIndexes_7_10());
        converter.register(DropGuidClassAndFieldIndexes_7_12.VERSION, new DropGuidClassAndFieldIndexes_7_12());
        converter.register(DropDateTimeOffsetClassIndexes_7_12.VERSION, new DropDateTimeOffsetClassIndexes_7_12());
        converter.register(VersionNumberToCommitTimestamp_8_0.VERSION, new VersionNumberToCommitTimestamp_8_0());
    }   
}

package com.db4odoc.performance;


import com.db4o.config.annotations.Indexed;
import org.joda.time.DateTime;

import java.util.Date;

public class Item {
    @Indexed
    private final String indexedString;
    @Indexed
    private final int indexNumber;
    @Indexed
    private final Date indexDate;

    public Item(int number) {
        this.indexedString = dataString(number);
        this.indexNumber = number;
        DateTime dt = new DateTime(number);
        this.indexDate = dt.toDate();
    }

    public String getIndexedString() {
        return indexedString;
    }

    public static String dataString(int number){
        return "data for "+number;
    }

    public boolean complexMethod(){
        return indexedString.split("for")[0].contains("5");
    }

    public int getIndexNumber() {
        return indexNumber;
    }

    public Date getIndexDate() {
        return indexDate;
    }
}

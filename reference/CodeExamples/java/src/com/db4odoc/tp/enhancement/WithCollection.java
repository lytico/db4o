package com.db4odoc.tp.enhancement;


import java.util.ArrayList;
import java.util.List;
@TransparentPersisted
public class WithCollection {
    private List<String> testField = new ArrayList<String>();

    public boolean add(String s) {
        return testField.add(s);
    }


}

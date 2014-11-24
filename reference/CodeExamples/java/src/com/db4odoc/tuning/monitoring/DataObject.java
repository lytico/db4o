package com.db4odoc.tuning.monitoring;

import java.util.Random;


class DataObject {
    private String label;

    DataObject(Random rnd) {
        this.label = newString(rnd);
    }

    private static String newString(Random rnd) {
        StringBuilder buffer = new StringBuilder();
        for (int i = 0; i < rnd.nextInt(4096); i++) {
            int charNr = 65 + rnd.nextInt(26);
            buffer.append((char) charNr);
        }
        return buffer.toString();
    }
}

package com.db4odoc.configuration.freespace;

import com.db4o.config.FreespaceFiller;
import com.db4o.io.BlockAwareBinWindow;

import java.io.IOException;

// #example: The freespace filler
class MyFreeSpaceFiller implements FreespaceFiller {
    @Override
    public void fill(BlockAwareBinWindow block) throws IOException {
        byte[] emptyBytes = new byte[block.length()];
        block.write(0,emptyBytes);
    }
}
// #end example

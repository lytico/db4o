package com.db4odoc.practises.deletion;


abstract class Deletable {
    private boolean deleted = false;

    public void delete() {
        this.deleted = true;
    }


    public boolean isDeleted() {
        return deleted;
    }
}

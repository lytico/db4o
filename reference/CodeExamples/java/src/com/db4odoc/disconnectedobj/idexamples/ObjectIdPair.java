package com.db4odoc.disconnectedobj.idexamples;


public class ObjectIdPair<TId,TObject> {
    private final TId id;
    private final TObject object;

    private ObjectIdPair(TId id, TObject object) {
        this.id = id;
        this.object = object;
    }

    public static <TId,TObject>  ObjectIdPair<TId,TObject> create(TId id, TObject object){
        return new ObjectIdPair<TId,TObject>(id, object);
    }

    public TId getId() {
        return id;
    }

    public TObject getObject() {
        return object;
    }
}

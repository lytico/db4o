package com.db4o.rmi;

import java.io.*;
import java.util.*;
import java.util.Map.Entry;


@SuppressWarnings({ "unchecked", "rawtypes" })
public class Serializers {

	private static final Map<Class<?>, Serializer<?>> serializers = new HashMap<Class<?>, Serializer<?>>();

	public static <T> Serializer<T> addSerializer(Serializer<T> serializer, Class<?>... classes) {
		for (Class<?> clazz : classes) {
			serializers.put(clazz, serializer);
		}
		return serializer;
	}

	public static <T> Serializer<T> serializerFor(Class<T> t) {
		return (Serializer<T>) serializers.get(t);
	}


	public final static Serializer<UUID> uuid = addSerializer(new Serializer<UUID>() {
		
		public UUID deserialize(DataInput in) throws IOException {
			return new UUID(in.readLong(), in.readLong());
		}

		public void serialize(DataOutput out, UUID id) throws IOException {
			out.writeLong(id.getMostSignificantBits());
			out.writeLong(id.getLeastSignificantBits());
		}

	}, UUID.class);

	public final static Serializer<Boolean> bool = addSerializer(new Serializer<Boolean>() {
		
		public Boolean deserialize(DataInput in) throws IOException {
			return in.readBoolean();
		}

		public void serialize(DataOutput out, Boolean v) throws IOException {
			out.writeBoolean(v);
		}

	}, Boolean.class, boolean.class);

	
	public final static Serializer<Integer> integer = addSerializer(new Serializer<Integer>() {

		public void serialize(DataOutput out, Integer item) throws IOException {
			out.writeInt(item);
		}

		public Integer deserialize(DataInput in) throws IOException {
			return in.readInt();
		}
	}, Integer.class, int.class);

	
	public final static Serializer<Long> sixtyfourbit = addSerializer(new Serializer<Long>() {

		public void serialize(DataOutput out, Long item) throws IOException {
			out.writeLong(item);
		}

		public Long deserialize(DataInput in) throws IOException {
			return in.readLong();
		}
	}, Long.class, long.class);

	
	public final static Serializer<Byte> eightbit = addSerializer(new Serializer<Byte>() {

		public void serialize(DataOutput out, Byte item) throws IOException {
			out.writeByte(item);
		}

		public Byte deserialize(DataInput in) throws IOException {
			return in.readByte();
		}
	}, Byte.class, byte.class);

	
	public final static Serializer<String> string = addSerializer(new Serializer<String>() {

		public void serialize(DataOutput out, String item) throws IOException {
			out.writeUTF(item);
		}

		public String deserialize(DataInput in) throws IOException {
			return in.readUTF();
		}
	}, String.class);
	
	public final static Serializer<String[]> stringArray = addSerializer(new Serializer<String[]>() {

		public void serialize(DataOutput out, String[] item) throws IOException {
			out.writeInt(item.length);
			for (int i = 0; i < item.length; i++) {
				out.writeUTF(item[i]);	
			}
		}

		public String[] deserialize(DataInput in) throws IOException {
			int length = in.readInt();
			String[] result = new String[length];
			for (int i = 0; i < result.length; i++) {
				result[i] = in.readUTF();	
			}
			return result;
		}
	}, String[].class);
	
    public final static Serializer<long[]> longArray = addSerializer(new Serializer<long[]>() {

        public void serialize(DataOutput out, long[] arr) throws IOException {
            out.writeInt(arr.length);
            for (int i = 0; i < arr.length; i++) {
                out.writeLong(arr[i]);  
            }
        }

        public long[] deserialize(DataInput in) throws IOException {
            int length = in.readInt();
            long[] result = new long[length];
            for (int i = 0; i < result.length; i++) {
                result[i] = in.readLong();  
            }
            return result;
        }
    }, long[].class);

    
    public final static Serializer<byte[]> byteArray = addSerializer(new Serializer<byte[]>() {

        public void serialize(DataOutput out, byte[] arr) throws IOException {
            out.writeInt(arr.length);
            out.write(arr);
        }

        public byte[] deserialize(DataInput in) throws IOException {
            byte[] buffer = new byte[in.readInt()];
            in.readFully(buffer);
            return buffer;
        }
    }, byte[].class);

    
    public final static Serializer<HashSet> hashset = addSerializer(new Serializer<HashSet>() {

        public void serialize(DataOutput out, HashSet item) throws IOException {
			out.writeInt(item.size());
			for (Object object : item) {
				out.writeUTF(object.getClass().getName());
				Serializer<Object> s = (Serializer<Object>) serializerFor(object.getClass());
				s.serialize(out, object);
			}
		}

		public HashSet deserialize(DataInput in) throws IOException {
			HashSet<Object> ret = new HashSet<Object>();
			int len = in.readInt();
			for(int i=0;i<len;i++) {
				Serializer<Object> s = (Serializer<Object>) serializerFor(classFor(in.readUTF()));
				ret.add(s.deserialize(in));
			}
			return ret;
		}
	}, HashSet.class, Set.class);

	public final static Serializer<ArrayList> arraylist = addSerializer(new Serializer<ArrayList>() {

		public void serialize(DataOutput out, ArrayList item) throws IOException {
			out.writeInt(item.size());
			for (Object object : item) {
				out.writeUTF(object.getClass().getName());
				Serializer<Object> s = (Serializer<Object>) serializerFor(object.getClass());
				s.serialize(out, object);
			}
		}

		public ArrayList deserialize(DataInput in) throws IOException {
			ArrayList<Object> ret = new ArrayList<Object>();
			int len = in.readInt();
			for(int i=0;i<len;i++) {
				Serializer<Object> s = (Serializer<Object>) serializerFor(classFor(in.readUTF()));
				ret.add(s.deserialize(in));
			}
			return ret;
		}
	}, ArrayList.class, List.class);

	public final static Serializer<HashMap> hashmap = addSerializer(new Serializer<HashMap>() {

		public void serialize(DataOutput out, HashMap item) throws IOException {
			out.writeInt(item.size());
			Set<Entry> e = item.entrySet();
			for (Entry entry : e) {
				
				Class<? extends Object> keyClass = entry.getKey().getClass();
				Class<? extends Object> valueClass = entry.getValue().getClass();

				out.writeUTF(keyClass.getName());
				out.writeUTF(valueClass.getName());
				
				Serializer keySerializer = serializerFor(keyClass);
				Serializer valueSerializer = serializerFor(valueClass);
				
				keySerializer.serialize(out, entry.getKey());
				valueSerializer.serialize(out, entry.getValue());
				
			}
		}

        public HashMap deserialize(DataInput in) throws IOException {
			HashMap ret = new HashMap();
			
			int len = in.readInt();
			for(int i=0;i<len;i++) {
				
				Class<? extends Object> keyClass = classFor(in.readUTF());
				Class<? extends Object> valueClass = classFor(in.readUTF());
				
				Serializer keySerializer = serializerFor(keyClass);
				Serializer valueSerializer = serializerFor(valueClass);
				
				ret.put(keySerializer.deserialize(in), valueSerializer.deserialize(in));
			}
			
			return ret;
		}
	}, HashMap.class, Map.class);

	protected static Class<? extends Object> classFor(String className) throws IOException {
		try {
			return Class.forName(className);
		} catch (ClassNotFoundException e) {
			throw new IOException(e.toString());
		}
	}

	
}

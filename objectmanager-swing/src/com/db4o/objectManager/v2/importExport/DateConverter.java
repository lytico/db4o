package com.db4o.objectManager.v2.importExport;

import com.db4o.objectManager.v2.util.DateFormatter;
import com.thoughtworks.xstream.converters.ConversionException;
import com.thoughtworks.xstream.converters.Converter;
import com.thoughtworks.xstream.converters.MarshallingContext;
import com.thoughtworks.xstream.converters.UnmarshallingContext;
import com.thoughtworks.xstream.io.HierarchicalStreamReader;
import com.thoughtworks.xstream.io.HierarchicalStreamWriter;

import java.text.ParseException;
import java.util.Date;

/**
 * User: treeder
 * Date: Mar 19, 2007
 * Time: 1:26:41 AM
 */
public class DateConverter implements Converter {
	private DateFormatter formatter;

	public DateConverter() {
		super();
		formatter = new DateFormatter();
	}

	public boolean canConvert(Class clazz) {
		return Date.class.isAssignableFrom(clazz);
	}

	public void marshal(Object value, HierarchicalStreamWriter writer,
						MarshallingContext context) {
		Date d = (Date) value;
		writer.setValue(formatter.edit(d));
	}

	public Object unmarshal(HierarchicalStreamReader reader,
							UnmarshallingContext context) {
		Date d;
		try {
			d = formatter.parse(reader.getValue());
			return d;
		} catch(ParseException e) {
			throw new ConversionException(e.getMessage(), e);
		}
	}

}

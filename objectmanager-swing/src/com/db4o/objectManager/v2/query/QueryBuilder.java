package com.db4o.objectManager.v2.query;


/**
 * User: treeder
 * Date: Sep 27, 2006
 * Time: 1:48:41 PM
 */
public class QueryBuilder {
    public static String addClass(String currentText, String className) {
        return "FROM '" + className + "' ";
    }

    public static String addField(String currentText, String className, String fieldName) {
        StringBuilder sb = new StringBuilder(currentText);
        // verify the correct class
        if (!currentText.contains(className)) {
            sb = new StringBuilder(addClass(currentText, className));
        }

        // then just append field to where
        if (!currentText.contains("WHERE")) { // todo: case insensitive
            sb.append(" WHERE");
        } else {
            sb.append(" AND");
        }
        sb.append(" ").append(fieldName).append(" = ?");
        return sb.toString();
    }
}

package com.db4odoc.pitfalls.typehandling;

import java.util.Date;


class DatesHolder {
    private Date regularDate;
    private java.sql.Date sqlField;

    DatesHolder() {
        this.regularDate = new Date();
        this.sqlField = new java.sql.Date(regularDate.getTime());
    }

    public Date getRegularDate() {
        return regularDate;
    }

    public java.sql.Date getSqlField() {
        return sqlField;
    }
}

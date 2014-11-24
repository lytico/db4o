/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.eclipse
package preferences

object AndOrEnum extends Enumeration {

  abstract class AndOr(name: String) extends Val(name) {
    def apply(a: Boolean, b: Boolean): Boolean
  }
  
  val And = new AndOr("And") {
    override def apply(a: Boolean, b: Boolean) = a && b
  }
  
  val Or = new AndOr("Or") {
    override def apply(a: Boolean, b: Boolean) = a || b
  }
  
  override def valueOf(name: String) = super.valueOf(name).map(_.asInstanceOf[AndOr])
}

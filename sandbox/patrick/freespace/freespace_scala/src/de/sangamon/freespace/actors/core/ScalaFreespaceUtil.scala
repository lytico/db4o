package de.sangamon.freespace.actors.core;

object ScalaFreespaceUtil {
	val NUM_CLIENTS = Array(1, 2, 4, 8, 16, 32)
  
  def argVal(args: Array[String], idx: Int, defVal: Int) =
    if(args != null && args.length > 0)
      Integer.parseInt(args(idx))
    else
      defVal

}

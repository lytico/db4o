package de.sangamon.freespace.actors.sync;


import de.sangamon.freespace.core._
import de.sangamon.freespace.actors.core._

import scala.actors._
import scala.actors.Actor._

class SyncActorFreespaceManagerWrapper extends FreespaceManager {

  val delegate = new SyncActorFreespaceManager()
  delegate.start
  
  def acquire(): Freespace = {
    (delegate !? AcquireRequest) match {
      case AcquireReceipt(freespace) => freespace
    }
  }

  def free(freed: Freespace) = {
    delegate ! FreeRequest(freed)
  }

  def shutdown(): Unit = {
    delegate ! QuitRequest
  }

}

package de.sangamon.freespace.actors.core;

import de.sangamon.freespace.core._

import scala.actors._
import scala.actors.Actor._


class ActorFreespaceManagerWrapper(delegate: Actor) extends FreespaceManager {

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

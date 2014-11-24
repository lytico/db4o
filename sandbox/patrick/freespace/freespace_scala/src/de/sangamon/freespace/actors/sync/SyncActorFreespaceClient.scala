package de.sangamon.freespace.actors.sync

import de.sangamon.freespace.core._
import de.sangamon.freespace.actors.core._

import scala.actors._
import scala.actors.Actor._


case object FinishNotification


class SyncActorFreespaceClient(id: Int, manager: SyncActorFreespaceManager, numAcquisitions: Int, numRuns: Int, control: Actor) extends Actor {

  private val lock = new Object()
  
  override def act(): unit = {
    val acquired = new Array[Freespace](numAcquisitions)
    var runIdx = 0
    for (runIdx <- 0 until numRuns) {
      log(runIdx, "acq")
      for (acqIdx <- 0 until numAcquisitions) {
        acquired(acqIdx) = manager !? AcquireRequest match{
          case AcquireReceipt(freespace) => freespace
        }
      }
      log(runIdx, "fre")
      FreespaceUtil.sleep(FreespaceUtil.CLIENT_WORK_TICKS);
      wait(FreespaceUtil.CLIENT_IDLE_TIME);
      for(acq <- acquired) {
        manager ! FreeRequest(acq)
      }
      FreespaceUtil.sleep(FreespaceUtil.CLIENT_WORK_TICKS);
      wait(FreespaceUtil.CLIENT_IDLE_TIME);
    }
    log(runIdx, "stp")
    control ! FinishNotification
  }
  
  private def wait(time: Int) = {
    FreespaceUtil.wait(lock, FreespaceUtil.CLIENT_IDLE_TIME);
  }

  private def log(runIdx: Int, msg: String) {
//    println(id + " " + msg  + " (" + runIdx + ")")
  }

}

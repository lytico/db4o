package de.sangamon.freespace.actors.react

import de.sangamon.freespace.core._
import de.sangamon.freespace.actors.core._

import scala.actors._
import scala.actors.Actor._


case object FinishNotification

case class ClientState(curRun: Int, curIdx: Int)

class ReactActorFreespaceClient(id: Int, manager: ReactActorFreespaceManager, numAcquisitions: Int, numRuns: Int, control: Actor) extends Actor {

  private val lock = new Object()

  override def act(): unit = {
    val acquired = new Array[Freespace](numAcquisitions)
    var state = ClientState(0, 0)
    log(state.curRun, "acq")
    manager ! AcquireRequest
    loop {
      react {
        case AcquireReceipt(freespace) =>
        	acquired(state.curIdx) = freespace
          if(state.curIdx == numAcquisitions - 1) {
            log(state.curRun, "fre")
            FreespaceUtil.sleep(FreespaceUtil.CLIENT_WORK_TICKS);
            idleAndThen((anyx: Any) => {
                for(acq <- acquired)
                  manager ! FreeRequest(acq)
                FreespaceUtil.sleep(FreespaceUtil.CLIENT_WORK_TICKS)
                idleAndThen((anyy: Any) =>{
                    if(state.curRun == numRuns - 1) {
                      control ! FinishNotification
                      log(state.curRun, "stp")
                      exit()
                    }
                    else {
                      state = ClientState(state.curRun + 1, 0)
                      log(state.curRun, "acq")
                      manager ! AcquireRequest
                    }
                })
            })
          }
          else {
            state = ClientState(state.curRun, state.curIdx + 1)
            manager ! AcquireRequest
          }
      }
    }
  }
  
  def idleAndThen(f: Function[Any, Unit]): Nothing = {
    reactWithin(FreespaceUtil.CLIENT_IDLE_TIME) {
      case TIMEOUT => {
        f()
      }
    }
  }
  
  private def log(runIdx: Int, msg: String) {
//    println(id + " " + msg + " (" + runIdx + ")")
  }

}

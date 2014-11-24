package de.sangamon.freespace.actors.sync

import de.sangamon.freespace.core._
import de.sangamon.freespace.actors.core._

import scala.actors.Actor
import scala.actors.Actor._


class SyncActorFreespaceControl(manager: SyncActorFreespaceManager, clientCount: int) extends Actor {

  override def act(): unit = {
    var closeCount = 0
    while(true) {
      receive {
        case FinishNotification => {
          closeCount += 1
          if(closeCount == clientCount) {
            manager ! QuitRequest
            exit()
          }
        }
      }
    }
  }
  
}


object SyncActorFreespaceMain {
  def main(args: Array[String]) = {
    val numClients = ScalaFreespaceUtil.NUM_CLIENTS(ScalaFreespaceUtil.argVal(args, 0, ScalaFreespaceUtil.NUM_CLIENTS.length - 1))
    println(numClients)
    val manager = new SyncActorFreespaceManager()
    manager.start()
    val control = new SyncActorFreespaceControl(manager, numClients)
    control.start()
    for(clientIdx <- 0 until numClients) {
      val client = new SyncActorFreespaceClient(clientIdx, manager, FreespaceUtil.NUM_RUNS, FreespaceUtil.NUM_ACQUISITIONS, control)
      client.start()
    }  
  }
}

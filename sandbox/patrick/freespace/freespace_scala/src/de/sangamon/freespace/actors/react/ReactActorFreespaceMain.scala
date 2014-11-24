package de.sangamon.freespace.actors.react


import de.sangamon.freespace.core._
import de.sangamon.freespace.actors.core._

import scala.actors.Actor
import scala.actors.Actor._


class ReactActorFreespaceControl(manager: ReactActorFreespaceManager, clientCount: int) extends Actor {

  override def act(): unit = {
    var closeCount = 0
    loop {
      react {
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


object ReactActorFreespaceMain {
  def main(args: Array[String]) = {
    val numClients = ScalaFreespaceUtil.NUM_CLIENTS(ScalaFreespaceUtil.argVal(args, 0, ScalaFreespaceUtil.NUM_CLIENTS.length - 1))
    println(numClients)
    val manager = new ReactActorFreespaceManager()
    manager.start()
    val control = new ReactActorFreespaceControl(manager, numClients)
    control.start()
    for(clientIdx <- 0 until numClients) {
      val client = new ReactActorFreespaceClient(clientIdx, manager, FreespaceUtil.NUM_RUNS, FreespaceUtil.NUM_ACQUISITIONS, control)
      client.start()
    }  
  }

}
package de.sangamon.freespace.actors.sync

import de.sangamon.freespace.managers.SimpleFreespaceManager
import de.sangamon.freespace.core._
import de.sangamon.freespace.actors.core._

import scala.actors.Actor
import scala.actors.Actor._

class SyncActorFreespaceManager extends Actor {
	private val space = new SimpleFreespaceManager()
  
  override def act(): unit = {
    while(true) {
      receive {
        case AcquireRequest => reply { AcquireReceipt(space.acquire()) }
        case FreeRequest(freed) => space.free(freed)
        case QuitRequest => space.shutdown(); exit()
      }
    }
  }
}

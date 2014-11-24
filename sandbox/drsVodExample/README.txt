
**************************************************************
                      drsVodExample
**************************************************************

All Java classes in the drs.vod.example package contain main()
methods that can be run.

This sample requires VOD 8.0 Patch 3 available from here:
http://www.versant.com/developer/downloads/

When you install it, make sure you also include JVI, selectable
below Java, since JVI includes veddriver libraries, required 
for this sample.

To include VOD libraries for this sample, it's easiest if you
create a 'VERSANT' user library. Configure the Eclipse build
path, add library, User Library, new library 'VERSANT', and
add jars below C:/Versant/lib and C:/Versant/SDK/lib.    

Before you can get started you need to prepare the VOD Database
for events and you have to start the EventDriver and the 
EventProcessor applications.

To do this: 

- Adjust the script /scripts/setEnvironment to the paths on 
your machine

- Create the sample database using /scripts/createDatabase

- Create the event schema using /scripts/createEventSchema

- Start the event driver using /scripts/startEventDriver
It should come up in a separate console stay up and print:
'Event Daemon : Starting the event daemon for dRSVodExample'

- Start the event processor using /scripts/startEventProcessor
It should come up in a separate console stay up and print:
'VOD EventProcessor for dRS is listening for events.'

- The persistent classes for this example need to be enhanced.
To do this, you can use the build.xml Ant script in this 
project. To set this up, create a machine.properties containing
a single line where dir.versant points to the /lib folder of
your VOD installation. For example:
dir.versant.lib=C:/Versant/8/lib
Once this machine.properties file is created, you can run the
enhance target in the build.xml.

Once you have done all of the above you can play with all the
classes in the drs.vod.example package to store, replicate and
print the content of a VOD and a db4o database.

A recommended sequence to run the Java programs:
(1) Store2NewBooksToVod
(2) PrintDatabaseContent 
(3) ReplicateAllBooksOrdersCustomersFromVodToDb4o
(4) PrintDatabaseContent
(5) StoreNewOrderInDb4o
(6) PrintDatabaseContent
(7) ReplicateAllChangesInBothDirections
(8) PrintDatabaseContent
...


**************************************************************
                       * Important *
**************************************************************
If you want to use dRS replication in productive use, both the
EventDriver and the EventProcessor always need to run against
the VOD database to track changes.
**************************************************************


In a production environment it is recommended to adjust the
profile.be configuration file of the database to automate
starting the event driver and the event processor.

To set this up (after creating the database and the event schema):
- Stop the database with stopdb <databaseName> -f

- Copy the setEnvironment and the startEventProcessor scripts and
the config.ved file ( config.ved.win  or config.ved.linux)
to the database folder  (e.g.:  C:\Versant\db\dRSVodExample )

- Edit EXAMPLE_HOME in the setEnvironment script to point to
the full path of your project
(e.g.:  SET EXAMPLE_HOME=C:\Workspace\drsVodExample )

- Add two lines like the following to the profile.be configuration
of your database file:
--------------------------------------------------------------
event_daemon C:\Versant\8\bin\veddriver.exe  C:\Versant\db\drsVodExample\config.ved.win

startup_script startEventProcessor.bat
--------------------------------------------------------------

- Start the database with startdb  


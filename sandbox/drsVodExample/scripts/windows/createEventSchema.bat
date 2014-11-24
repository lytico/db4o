call %~dp0%setEnvironment.bat
%JAVA% -cp %LOCAL_CLASSPATH% com.db4o.drs.versant.eventprocessor.CreateEventSchema %DATABASE_NAME%
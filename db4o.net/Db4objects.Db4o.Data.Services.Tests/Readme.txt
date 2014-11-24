The following command is required to run Db4objects.Db4o.Data.Services.Tests.

netsh http add urlacl url=http://+:666/integration/ user=MACHINE\user

The following error will happen had you failed to run the above command:

"HTTP could not register URL http://+:666/integration/. Your process does not have access rights to this namespace (see http://go.microsoft.com/fwlink/?LinkId=70353 for details). ---> System.Net.HttpListenerException: Access is denied"
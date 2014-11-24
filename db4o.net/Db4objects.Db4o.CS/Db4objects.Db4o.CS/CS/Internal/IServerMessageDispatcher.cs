/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal
{
	/// <exclude></exclude>
	public interface IServerMessageDispatcher : IClientConnection, IMessageDispatcher
		, ICommittedCallbackDispatcher
	{
		void QueryResultFinalized(int queryResultID);

		Socket4Adapter Socket();

		int DispatcherID();

		LazyClientObjectSetStub QueryResultForID(int queryResultID);

		void SwitchToMainFile();

		void SwitchToFile(MSwitchToFile file);

		void UseTransaction(MUseTransaction transaction);

		void MapQueryResultToID(LazyClientObjectSetStub stub, int queryResultId);

		ObjectServerImpl Server();

		void Login();

		bool Close();

		bool Close(ShutdownMode mode);

		void CloseConnection();

		void CaresAboutCommitted(bool care);

		bool CaresAboutCommitted();

		bool Write(Msg msg);

		CallbackObjectInfoCollections CommittedInfo();

		Db4objects.Db4o.CS.Internal.ClassInfoHelper ClassInfoHelper();

		bool ProcessMessage(Msg message);

		/// <exception cref="System.Exception"></exception>
		void Join();

		void SetDispatcherName(string name);

		Db4objects.Db4o.Internal.Transaction Transaction();
	}
}

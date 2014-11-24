//#define RUNNING_OUTSIDE_SERVER
/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */

using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;
using Db4objects.Db4o.Query;
using Sharpen.Lang;

//TODO: Move TransportObjectContainer to CS project
namespace Db4objects.Db4o.Tests.CLI1.CrossPlatform
{
	class JavaServerCrossplatformTestCase : CrossplatformTestCaseBase
	{
#if !CF && !SILVERLIGHT
		public void TestQueryOnEmptyDatabaseDoesntThrows()
		{
			AssertQuery(
				new Person[0],
				FindAllJoes,
				"Robert');DROP TABLE Students;");
		}

		public void TestCrossplatform()
		{
			ReconnectAndRun(AssertEvents);

			//ReconnectAndRun(AssertSort());

			AddPersons(Persons);

			AssertQuery(
				Array.FindAll(Persons, FindAllJoes),
				FindAllJoes,
				JOE_NAME);

			ReconnectAndRun(AssertDelete);

			ReconnectAndRun(AssertQueryFromJavaClient);

			ReconnectAndRun(
				delegate
				{
					AssertInsertFromJavaClient("Michael Sullivan", 2002, DateTime.Now);
				});
		}

		public void TestJaggedArray()
		{
			Movies movies = new Movies();

			movies.Add("Toy Story", "Animation");
			movies.Add("The Terminal", "Drama");
			movies.Add("Catch Me If You Can", "Crime");

			_client.Store(movies);
			_client.Commit();

			ReconnectAndRun(
				delegate
				{
					string result = RunJavaQuery("query-movie", String.Empty);
					Assert.AreEqual(movies.ToString(), result);
				});
		}

		public void _TestQueryByIdentity()
		{
			//ByExample foo = new ByExample("foo", new ByExample("foo.child"));
			ByExample foo = new ByExample("foo");
			ReconnectAndRun(
				delegate
				{
					_client.Store(foo);
					_client.Commit();
					return;
					IObjectSet result = _client.QueryByExample(new ByExample("foo", foo.Child));
					Assert.AreEqual(1, result.Count);
					ByExample actual = (ByExample)result[0];
					Assert.AreEqual(foo, actual);
				});
		}

		private void AssertQuery(Person[] expected, Predicate<Person> predicate, string name)
		{
			AssertSodaEvaluation(expected, name);
			AssertSodaQuery(expected, name);
			AssertNativeQuery(expected, predicate);
			AssertUnoptimizedNativeQuery(expected);
			AssertQBEQuery(expected, new Person(name, 0, default(DateTime)));
		}

		private void AssertSodaEvaluation(Person[] expected, string name)
		{
			ReconnectAndRun(
				delegate
				{
					IQuery query = _client.Query();
					query.Constrain(typeof (Person));
					query.Constrain(new PersonEvaluator(name));
                    
					Iterator4Assert.SameContent(expected, query.Execute().GetEnumerator());
				});
		}

		private void AssertNativeQuery(Person[] expected, Predicate<Person> predicate)
		{
			ReconnectAndRun(
				delegate
				{
					Iterator4Assert.SameContent(expected, _client.Query(predicate).GetEnumerator());
					Assert.IsTrue(_queryStatus.Optimized);
				});
		}

		private void AssertUnoptimizedNativeQuery(Person[] expected)
		{
			ReconnectAndRun(
				delegate
				{
					TestPlatform.EmitWarning("Unoptimized NQ still not working.");

					//Iterator4Assert.SameContent(
					//    expected,
					//    _client.Query(new UnoptimizideJoeFinder()).GetEnumerator());
					//Assert.IsFalse(_queryStatus.Optimized);
				});
		}

		private void AssertQBEQuery(Person[] expected, Person template)
		{
			ReconnectAndRun(delegate
			               	{
			               		Iterator4Assert.SameContent(expected, _client.QueryByExample(template).GetEnumerator());
			               	});
		}

		private void AssertSodaQuery(Person[] expected, string name)
		{
			ReconnectAndRun(
				delegate
				{
					ICollection actual = SodaQueryByName(name);
					Assert.AreEqual(expected.Length, actual.Count);
					Iterator4Assert.SameContent(expected, actual.GetEnumerator());
				});
		}

		private void AssertDelete()
		{
			AssertDelete(JOE_NAME, Array.FindAll(Persons, FindAllJoes).Length);

			DeleteAll();

			AssertNotFound(WOODY_NAME);

			AddPersons(Persons);
		}

		private void AssertDelete(string name, int count)
		{
			AssertStoredCount(name, count);
			Delete(SodaQueryByName(name));
			AssertNotFound(name);
		}

		private void AssertEvents()
		{
			DeleteAll();

			int createdCount = 0;
			RegisterEvents(delegate { createdCount++; } );

			AddPersons(Persons);
			Assert.AreEqual(Persons.Length, createdCount);
		}

		private void RegisterEvents(EventHandler<ObjectInfoEventArgs> createdHandler)
		{
			IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(_client);

			eventRegistry.Created += createdHandler;
		}

		private void AssertSort()
		{
			Person[] expectedOrder = (Person[])Persons.Clone();
			Array.Sort(expectedOrder, Person.SortByYear);

			Iterator4Assert.AreEqual(
				expectedOrder,
				_client.Query(Person.SortByYear).GetEnumerator());
		}

		private void AssertStoredCount(string name, int count)
		{
			ReconnectAndRun(
				delegate
				{
					ICollection personList = SodaQueryByName(name);
					Assert.IsNotNull(personList);

					Assert.AreEqual(count, personList.Count);
				});
		}

		private void AssertNotFound(string name)
		{
			AssertStoredCount(name, 0);
		}

		private void Delete(ICollection toBeDeleted)
		{
			foreach (Object item in toBeDeleted)
			{
				_client.Delete(item);
			}

			_client.Commit();
		}

		private void DeleteAll()
		{
			IQuery query = _client.Query();
			query.Constrain(typeof (Person));
			Delete(query.Execute());
		}

		private void AddPersons(IEnumerable<Person> toBeAdded)
		{
			foreach (Person person in toBeAdded)
			{
				_client.Store(person);
			}

			_client.Commit();
		}

		protected override IConfiguration Config()
		{
			IConfiguration config = Db4oFactory.NewConfiguration();

			ConfigureDiagnostics(config);
			ConfigureAliases(config);

			return config;
		}

		protected override string GetClientAliases()
		{
			return
				@"config.addAlias(new com.db4o.config.TypeAlias(""com.db4o.crossplatform.test.server.StartServer$Person"", Person.class.getName()));
                  config.addAlias(new com.db4o.config.TypeAlias(""com.db4o.crossplatform.test.server.StartServer$Movies"", Movies.class.getName()));";
		}

		protected override void StartServer()
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				JavaServices.CompileJavaCode(JavaServerCode.MainClassFile, JavaServerCode.SourceCode);
				string output = JavaServices.java(JavaServerCode.MainClassName, Port.ToString(), USER_NAME, USER_PWD, Debugger.IsAttached.ToString());
				if (output.Length > 0)
				{
					Assert.Fail("\r\n\r\n****** Java Server returned an error ********\r\n\r\n" + output);
				}
			});
		}

		private static void ConfigureDiagnostics(IConfiguration config)
		{
			config.Diagnostic().AddListener(_queryStatus);
		}

		private static void ConfigureAliases(IConfiguration config)
		{
			config.Add(new JavaSupport());

			AddAlias(config, "com.db4o.crossplatform.test.server.StartServer$Person", typeof(Person));
			AddAlias(config, "com.db4o.crossplatform.test.server.StartServer$Movies", typeof(Movies));
			AddAlias(config, "com.db4o.crossplatform.test.server.StartServer$UnoptimizideJoeFinder", typeof(UnoptimizideJoeFinder));
			AddAlias(config, "com.db4o.crossplatform.test.server.StartServer$StopServer", typeof(StopServer));
			AddAlias(config, "com.db4o.crossplatform.test.server.StartServer$PersonEvaluator", typeof(PersonEvaluator));

			config.AddAlias((new TypeAlias("com.db4o.crossplatform.test.server.StartServer$SortByYear", "Db4objects.Db4o.Tests.CLI1.CrossPlatform.Person+SortByYearImpl, Db4objects.Db4o.Tests")));
		}

		private static void AddAlias(IConfiguration config, string storedType, Type runtimeType)
		{
			config.AddAlias(new TypeAlias(storedType, TypeReference.FromType(runtimeType).GetUnversionedName()));
		}

		private static readonly TrackQueryOptimization _queryStatus = new TrackQueryOptimization();

		private readonly JavaSnippet JavaServerCode = new JavaSnippet("com.db4o.crossplatform.test.server.StartServer",
																	  @"package com.db4o.crossplatform.test.server;

import java.util.Date;
import java.io.*;
import com.db4o.query.*; 
import com.db4o.*;
import com.db4o.foundation.io.File4;
import com.db4o.foundation.io.Path4;
import com.db4o.messaging.MessageContext;
import com.db4o.messaging.MessageRecipient;

public class StartServer implements MessageRecipient  {
	private boolean stop = false;

	public static void main(String[] args) throws IOException {
		new StartServer().runServer(args);
	}

	public void runServer(String[] args) throws IOException {
		if (args.length < 3) {
			System.err.println(""Db4o (java) server usage: StartServer host_port user password debugging"");
		}
		synchronized (this) {
			ObjectServer db4oServer	= null;

			String databaseFile = Path4.combine(Path4.getTempPath(),""CrossPlatformJavaServer_"" + this.hashCode() + "".odb""); 
			try {
				int waitTime = 50;
				int iterationsToWait = ((args[3] == ""True"") ? toMiliseconds(600) : toMiliseconds(120)) / waitTime;
				File4.delete(databaseFile);

				db4oServer = com.db4o.cs.Db4oClientServer.openServer(databaseFile, Integer.parseInt(args[0]));
				db4oServer.grantAccess(args[1], args[2]);
				
				db4oServer.ext().configure().clientServer().setMessageRecipient(this);

				Thread.currentThread().setName(this.getClass().getName());

				int count = 0;
				while(!stop && count < iterationsToWait) {
					this.wait(waitTime);
					count++;
				}
			} catch (Exception e) {
				System.out.println(e);
				e.printStackTrace();
			}

			if (db4oServer != null) db4oServer.close();
		}
	}

	private int toMiliseconds(int seconds) {
		return 1000 * seconds;
	}

	public void processMessage(MessageContext con, Object message) {
		if (message instanceof StopServer) {
			close();
		}
	}
	
	public void close() {
		synchronized (this) {
			stop = true;
			this.notifyAll();
		}
	}

	public static class StopServer {
	}

	/*private static class ByExample {
		public String Name;
		public ByExample Child;

		public ByExample(String name) {
			Name = name;
		}

		public ByExample(String name, ByExample child)
		{
			this(name);
			Child = child;
		}
	}*/

	private static class Movies {
		private String[][] _notes;
		private int _counter;	
	}

	public static class Person {
		public String _name;
		public int _year;
		public Date _localReleaseDate;
    }

	public static class SortByYear implements QueryComparator {
		public int compare(Object first, Object second) {
			Person lhs = (Person) first;
			Person rhs = (Person) second;

			return lhs._year - rhs._year;
		}
	}

	public static class UnoptimizideJoeFinder extends Predicate {
		public boolean match(Object candidate) {
			Person tbt = (Person) candidate;
			return tbt._name.startsWith(""Joe"");
		}
	}

	public static class PersonEvaluator implements Evaluation	{
		private String _name;

		public PersonEvaluator(String name)	{
			_name = name;
		}

		public void evaluate(Candidate candidate) {
			Person realCandidate = (Person) candidate.getObject();
			candidate.include(realCandidate._name.equals(_name));
		}
	}
}");
#endif
	}
}

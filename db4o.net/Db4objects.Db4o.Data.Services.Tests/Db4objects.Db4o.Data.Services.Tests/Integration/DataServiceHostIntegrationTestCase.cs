using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Client;
using System.Data.Services.Common;
using System.Linq;
using System.Linq.Expressions;
using Db4objects.Db4o.Linq;
using Db4oUnit;
using Moq;
using Moq.Language.Flow;

namespace Db4objects.Db4o.Data.Services.Tests.Integration
{
	class DataServiceHostIntegrationTestCase : ITestLifeCycle
	{
		private static readonly Uri ServiceUri = new Uri("http://127.0.0.1:666/integration");

		private MockFactory _mockery = new MockFactory(MockBehavior.Strict)
		                               {
		                               	DefaultValue = DefaultValue.Mock
		                               };

		private readonly Mock<IObjectContainer> _sessionMock;
		private readonly Mock<IResourceFinder> _resourceFinderMock;

		public DataServiceHostIntegrationTestCase()
		{
			_sessionMock = Mock<IObjectContainer>();
			_resourceFinderMock = Mock<IResourceFinder>();
		}

		private Mock<T> Mock<T>() where T : class
		{
			return _mockery.Create<T>();
		}

		public void TestAddObject()
		{
			var contact = new Contact { Email = "a@b.c", Name = "abc" };

			Setup(session => session.Store(It.Is<Contact>(actual => actual.Equals(contact))))
				.AtMostOnce();

			Setup(session => session.Commit())
				.AtMostOnce();

			Playback(()=>
			{
				var context = new DataServiceContext(ServiceUri);
				context.AddObject("Contacts", contact);
				context.SaveChanges();
			});
		}

		public void TestUpdateObject()
		{
			var contact = new Contact { Email = "a@b.c", Name = "new name" };

			_resourceFinderMock.Setup(
					resourceFinder => resourceFinder.GetResource(It.IsAny<IQueryable<Contact>>(), typeof(Contact).FullName))
				.Returns(contact)
				.AtMostOnce();

			Setup(session => session.Store(It.Is<Contact>(actual => actual == contact)))
				.AtMostOnce();

			Setup(session => session.Commit())
				.AtMostOnce();

			Playback(() =>
			{
				var context = new DataServiceContext(ServiceUri);
				context.AttachTo("Contacts", contact);
				context.UpdateObject(contact);
				context.SaveChanges();
			});
		}

		public void TestDeleteObject()
		{
			var contact = new Contact { Email = "a@b.c", Name = "abc" };

			_resourceFinderMock.Setup(
					resourceFinder => resourceFinder.GetResource(It.IsAny<IQueryable<Contact>>(), null))
				.Returns(contact)
				.AtMostOnce();

			Setup(session => session.Delete(It.Is<Contact>(actual => actual == contact)))
				.AtMostOnce();

			Setup(session => session.Commit())
				.AtMostOnce();

			Playback(() =>
			{
				var context = new DataServiceContext(ServiceUri);
				context.AttachTo("Contacts", contact);
				context.DeleteObject(contact);
				context.SaveChanges();
			});
		}

		public void TestAddObjectGraph()
		{
			var message = new Message
			            {
			            	Id = Guid.NewGuid(),
			            	Body = "Hi!",
			            	To = new Contact
			            	     {
			            	     	Email = "a@b.c",
									Name = "abc"
			            	     }
			            };

			Setup(session => session.Store(It.Is<Contact>(actual => actual.Equals(message.To))))
				.AtMostOnce();

			Setup(session => session.Commit())
				.AtMost(2);

			_resourceFinderMock.Setup(
					resourceFinder => resourceFinder.GetResource(It.IsAny<IQueryable<Contact>>(), null))
				.Returns(message.To)
				.AtMostOnce();

			Setup(session => session.Store(It.Is<Message>(actual => actual.Equals(message))))
				.AtMostOnce();
			
			var context = new DataServiceContext(ServiceUri);
			Playback(()=>
			{
				context.AddObject("Contacts", message.To);
				context.AddObject("Messages", message);
				context.SetLink(message, "To", message.To);
				context.SaveChanges();
			});
		}

		public void TestReferenceUpdate()
		{
			var message = new Message
			{
				Id = Guid.NewGuid(),
				Body = "Hi!",
				To = new Contact
				{
					Email = "a@b.c"
				}
			};

			var newContact = new Contact
			{
				Email = "d@e.f",
			};

			_resourceFinderMock.Setup(
					resourceFinder => resourceFinder.GetResource(It.IsAny<IQueryable<Message>>(), null))
				.Returns(message)
				.AtMostOnce();

			_resourceFinderMock.Setup(
					resourceFinder => resourceFinder.GetResource(It.IsAny<IQueryable<Contact>>(), null))
				.Returns(newContact)
				.AtMostOnce();

			Setup(session => session.Store(It.Is<Message>(actual => actual == message && actual.To == newContact)))
				.AtMostOnce();

			Setup(session => session.Commit())
				.AtMostOnce();

			var context = new DataServiceContext(ServiceUri);
			Playback(() =>
			{
				context.AttachTo("Contacts", newContact);
				context.AttachTo("Messages", message);
				context.SetLink(message, "To", newContact);
				context.SaveChanges();
			});
		}

		public void TestCollectionAdd()
		{
			var message = new Message
			{
				Id = Guid.NewGuid(),
				Body = "Hi!",
				CC = new List<Contact>()
			};

			var cc = new Contact
			         {
			         	Email = "d@e.f",
			         	Name = "def"
			         };
			
			_resourceFinderMock.Setup(
					resourceFinder => resourceFinder.GetResource(It.IsAny<IQueryable<Message>>(), null))
				.Returns(message)
				.AtMostOnce();

			_resourceFinderMock.Setup(
					resourceFinder => resourceFinder.GetResource(It.IsAny<IQueryable<Contact>>(), null))
				.Returns(cc)
				.AtMostOnce();

			Setup(session => session.Store(It.Is<Message>(actual => actual == message && actual.CC.First() == cc)))
				.AtMostOnce();

			Setup(session => session.Commit())
				.AtMostOnce();

			var context = new DataServiceContext(ServiceUri);
			Playback(() =>
			{
				context.AttachTo("Contacts", cc);
				context.AttachTo("Messages", message);
				context.AddLink(message, "CC", cc);
				context.SaveChanges();
			});
		}

		public void TestCollectionDelete()
		{
			var cc = new Contact
			{
				Email = "d@e.f",
				Name = "def"
			};

			var message = new Message
			{
				Id = Guid.NewGuid(),
				CC = new List<Contact> { cc }
			};

			_resourceFinderMock.Setup(
					resourceFinder => resourceFinder.GetResource(It.IsAny<IQueryable<Message>>(), null))
				.Returns(message)
				.AtMostOnce();

			_resourceFinderMock.Setup(
					resourceFinder => resourceFinder.GetResource(It.IsAny<IQueryable<Contact>>(), null))
				.Returns(cc)
				.AtMostOnce();

			Setup(session => session.Store(It.Is<Message>(actual => actual == message && actual.CC.Count == 0)))
				.AtMostOnce();

			Setup(session => session.Commit())
				.AtMostOnce();

			var context = new DataServiceContext(ServiceUri);
			Playback(() =>
			{
				context.AttachTo("Contacts", cc);
				context.AttachTo("Messages", message);

				context.DeleteLink(message, "CC", cc);

				context.SaveChanges();
			});
		}

		private ISetup<IObjectContainer> Setup(Expression<Action<IObjectContainer>> expression)
		{
			return _sessionMock.Setup(expression);
		}

		private void Playback(Action action)
		{
			IntegrationDataContext.Session = _sessionMock.Object;
			IntegrationDataContext.ResourceFinder = _resourceFinderMock.Object;
			action();
			_mockery.VerifyAll();
		}

		public void SetUp()
		{
			_dataHost = new DataServiceHost(typeof(IntegrationDataService), new Uri[] { ServiceUri });
			_dataHost.Open();
		}

		public void TearDown()
		{
			_dataHost.Close();
		}

		private DataServiceHost _dataHost;
	}

	[System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]
	public class IntegrationDataService : DataService<IntegrationDataContext>
	{
		public static void InitializeService(IDataServiceConfiguration config)
		{
			config.SetEntitySetAccessRule("*", EntitySetRights.All);
			config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
			config.UseVerboseErrors = true;
		}
	}

	[DataServiceKey("Id")]
	public class Message
	{
		public Guid Id { get; set; }
		public Contact To { get; set; }
		public string Body { get; set; }
		public List<Contact> CC { get; set; }

		public override bool Equals(object obj)
		{
			Message other = obj as Message;
			if (null == other)
				return false;
			return Id == other.Id
				&& object.Equals(To, other.To)
				&& Body == other.Body;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}

	[DataServiceKey("Email")]
	public class Contact
	{
		public string Email { get; set; }

		public string Name { get; set; }

		public override bool Equals(object obj)
		{
			Contact other = obj as Contact;
			if (null == other)
				return false;
			return Email == other.Email
				 && Name == other.Name;
		}

		public override int GetHashCode()
		{
			return Email.GetHashCode();
		}
	}

	public interface IResourceFinder
	{
		object GetResource(IQueryable queryable, string resourceTypeName);
	}

	public class IntegrationDataContext : Db4oDataContext
	{
		public static IObjectContainer Session;

		public static IResourceFinder ResourceFinder;

		protected override IObjectContainer OpenSession()
		{
			return Session;
		}

		public override object GetResource(IQueryable query, string fullTypeName)
		{
			return ResourceFinder.GetResource(query, fullTypeName);
		}

		public IQueryable<Contact> Contacts
		{
			get { return QueryableFor<Contact>(); }
		}

		public IQueryable<Message> Messages
		{
			get { return QueryableFor<Message>(); }
		}

		private IDb4oLinqQueryable<T> QueryableFor<T>()
		{
			return Container.Cast<T>().AsQueryable();
		}
	}

	
}

/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Types;

namespace Db4objects.Db4o.Internal
{
	/// <exclude>TODO: Split into separate enums with defined range and values.</exclude>
	public sealed partial class Const4
	{
		public const byte Yapfileversion = 4;

		public const byte Yapbegin = (byte)'{';

		public const byte Yapfile = (byte)'Y';

		public const byte Yapid = (byte)'#';

		public const byte Yappointer = (byte)'>';

		public const byte Yapclasscollection = (byte)'A';

		public const byte Yapclass = (byte)'C';

		public const byte Yapfield = (byte)'F';

		public const byte Yapobject = (byte)'O';

		public const byte Yaparray = (byte)'N';

		public const byte Yaparrayn = (byte)'Z';

		public const byte Yapindex = (byte)'X';

		public const byte Yapstring = (byte)'S';

		public const byte Yaplong = (byte)'l';

		public const byte Yapinteger = (byte)'i';

		public const byte Yapboolean = (byte)'=';

		public const byte Yapdouble = (byte)'d';

		public const byte Yapbyte = (byte)'b';

		public const byte Yapshort = (byte)'s';

		public const byte Yapchar = (byte)'c';

		public const byte Yapfloat = (byte)'f';

		public const byte Yapend = (byte)'}';

		public const byte Yapnull = (byte)'0';

		public const byte Btree = (byte)'T';

		public const byte BtreeNode = (byte)'B';

		public const byte Header = (byte)'H';

		public const byte IntegerArray = (byte)'I';

		public const byte BtreeList = (byte)'L';

		public const int IdentifierLength = (Deploy.debug && Deploy.identifiers) ? 1 : 0;

		public const int BracketsBytes = (Deploy.debug && Deploy.brackets) ? 1 : 0;

		public const int BracketsLength = BracketsBytes * 2;

		public const int LeadingLength = IdentifierLength + BracketsBytes;

		public const int AddedLength = IdentifierLength + BracketsLength;

		public const int ShortBytes = 2;

		public const int IntegerBytes = (Deploy.debug && Deploy.debugLong) ? 11 : 4;

		public const int LongBytes = (Deploy.debug && Deploy.debugLong) ? 20 : 8;

		public const int CharBytes = 2;

		public const int Unspecified = int.MinValue + 100;

		public const int IntLength = IntegerBytes + AddedLength;

		public const int IdLength = IntLength;

		public const int LongLength = LongBytes + AddedLength;

		public const int IndirectionLength = IntLength + IdLength;

		public const int WriteLoop = (IntegerBytes - 1) * 8;

		public const int ObjectLength = AddedLength;

		public const int PointerLength = (IntLength * 2) + AddedLength;

		public const int MessageLength = IntLength * 2 + 1;

		public const byte SystemTrans = (byte)'s';

		public const byte UserTrans = (byte)'u';

		public const byte Xbyte = (byte)'X';

		public const int IgnoreId = -99999;

		public const int Primitive = -2000000000;

		public const int TypeArray = 3;

		public const int TypeNarray = 4;

		public const int None = 0;

		public const int State = 1;

		public const int Activation = 2;

		public const int Transient = -1;

		public const int AddMembersToIdTreeOnly = 0;

		public const int AddToIdTree = 1;

		public const int LockTimeInterval = 1000;

		public const int ServerSocketTimeout = Debug4.longTimeOuts ? 1000000 : 600000;

		public const int ClientSocketTimeout = ServerSocketTimeout;

		public const int MaximumBlockSize = 70000000;

		public const int MaximumArrayEntries = 7000000;

		public const int MaximumArrayEntriesPrimitive = MaximumArrayEntries * 100;

		public static readonly Type ClassCompare = typeof(ICompare);

		public static readonly Type ClassDb4otype = typeof(IDb4oType);

		public static readonly Type ClassDb4otypeimpl = typeof(IDb4oTypeImpl);

		public static readonly Type ClassInternal = typeof(IInternal4);

		public static readonly Type ClassUnversioned = typeof(IUnversioned);

		public static readonly Type ClassObject = new object().GetType();

		public static readonly Type ClassObjectcontainer = typeof(IObjectContainer);

		public static readonly Type ClassStaticfield = new StaticField().GetType();

		public static readonly Type ClassStaticclass = new StaticClass().GetType();

		public static readonly Type ClassTransientclass = typeof(ITransientClass);

		public static readonly string EmbeddedClientUser = "embedded client";

		public const int Clean = 0;

		public const int Active = 1;

		public const int Processing = 2;

		public const int CachedDirty = 3;

		public const int Continue = 4;

		public const int StaticFieldsStored = 5;

		public const int CheckedChanges = 6;

		public const int Dead = 7;

		public const int Reading = 8;

		public const int Activating = 9;

		public const int Old = -1;

		public const int New = 1;

		public static readonly UnicodeStringIO stringIO = new UnicodeStringIO();

		public static readonly Type[] EssentialClasses = new Type[] { ClassStaticfield, ClassStaticclass
			 };

		public static readonly string VirtualFieldPrefix = "v4o";

		public const int InvalidObjectId = 1;

		public const int DefaultMaxStackDepth = 20;
		// make sure we don't fall over the -1 cliff
		// TODO: Is this the right place for the knowledge, that an indirection
		//       within a slot is an address and a length?
		// debug constants
		// TODO: This one is a terrible low-frequency blunder in YapArray.writeClass!!!
		// If YapClass-ID == 99999 (not very likely) then we will get IGNORE_ID. Change
		// to -Integer.MAX_VALUE or protect 99999 in YapFile.getPointerSlot() 
		// This is a hard coded 2 Gig-Limit for YapClass-IDs.
		// TODO: get rid of magic numbers like this one
		// array type information
		// message levels
		// Use if > NONE: normal messages
		// if > STATE: state messages
		// if > ACTIVATION: activation messages
		// Timings
		// 10 minutes until clients are disconnected, (5 minutes until they get pinged) 
		// TODO: Consider to make configurable
		// 70 MB   
		// 7 Million 
		// 70 MB for byte arrays
		// bits in PersistentBase.i_state
		// and reuse in other classes 
		// system classes that need to get loaded first
		// StaticClass should load Staticfield
		// TODO: remove unnecessary
	}
}

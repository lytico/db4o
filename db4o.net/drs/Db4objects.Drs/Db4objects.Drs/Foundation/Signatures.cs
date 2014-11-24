/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections.Generic;
using Db4objects.Drs.Foundation;

namespace Db4objects.Drs.Foundation
{
	public class Signatures
	{
		private readonly IDictionary<Signature, long> _loidBySignature = new Dictionary<Signature
			, long>();

		private readonly IDictionary<long, Signature> _signatureByLoid = new Dictionary<long
			, Signature>();

		public virtual void Add(Signature signature, long signatureLoid)
		{
			_loidBySignature[signature] = signatureLoid;
			_signatureByLoid[signatureLoid] = signature;
		}

		public virtual long LoidForSignature(Signature signature)
		{
			return _loidBySignature[signature];
		}

		public virtual Signature SignatureForLoid(long signatureLoid)
		{
			return _signatureByLoid[signatureLoid];
		}
	}
}

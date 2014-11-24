/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace Db4objects.Db4o.Tests.Util
{
	internal class Certificates
	{
		private const int ENCODING = 0x00010001; // X509_ASN_ENCODING | PKCS_7_ASN_ENCODING
		private const int PROV_RSA_FULL = 1;
		private const int AT_KEYEXCHANGE = 1;
		private const int CERT_X500_NAME_STR = 3;
		private const string sz_CERT_STORE_PROV_MEMORY = "Memory";
		private const int CERT_STORE_CREATE_NEW_FLAG = 0x2000;
		private const int CERT_STORE_ADD_NEW = 1;
		private const int CERT_KEY_PROV_INFO_PROP_ID = 2;

		public static byte[] CreateSelfSignCertificate(string distinguishedName, DateTime startTime, DateTime endTime)
		{
			byte[] pfxData = CreateSelfSignCertificate(distinguishedName, startTime, endTime, (SecureString)null);
			return pfxData;
		}

		public static byte[] CreateSelfSignCertificate(string distinguishedName, DateTime startTime, DateTime endTime, string insecurePassword)
		{
			SecureString password = null;
			try
			{
				if (!string.IsNullOrEmpty(insecurePassword))
				{
					password = new SecureString();
					foreach (char ch in insecurePassword)
					{
						password.AppendChar(ch);
					}

					password.MakeReadOnly();
				}

				return CreateSelfSignCertificate(distinguishedName, startTime, endTime, password);
			}
			finally
			{
				if (password != null)
				{
					password.Dispose();
				}
			}
		}

		private static byte[] CreateSelfSignCertificate(string distinguishedName, DateTime startTime, DateTime endTime, SecureString password)
		{
			if (distinguishedName == null)
			{
				distinguishedName = "";
			}

			string containerName = Guid.NewGuid().ToString();

			IntPtr providerContext = IntPtr.Zero;
			IntPtr cryptKey = IntPtr.Zero;
			IntPtr certContext = IntPtr.Zero;
			IntPtr certStore = IntPtr.Zero;
			IntPtr storeCertContext = IntPtr.Zero;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				providerContext = AcquireProviderContext(containerName, PROV_RSA_FULL, NativeMethods.CRYPT_NEWKEYSET + NativeMethods.CRYPT_MACHINE_KEYSET);
				cryptKey = GenerateKey(providerContext, AT_KEYEXCHANGE, NativeMethods.CRYPT_EXPORTABLE);

				CryptKeyProviderInformation kpi = NewProviderInformationFor(containerName, PROV_RSA_FULL, AT_KEYEXCHANGE);

				certContext = CreateSelfSignedCertificate(providerContext, distinguishedName, startTime, endTime, ref kpi);

				certStore = NativeMethods.CertOpenStore(sz_CERT_STORE_PROV_MEMORY, 0, IntPtr.Zero, CERT_STORE_CREATE_NEW_FLAG, IntPtr.Zero);
				Check(certStore != IntPtr.Zero);

				Check(NativeMethods.CertAddCertificateContextToStore(certStore, certContext, CERT_STORE_ADD_NEW, out storeCertContext));

				NativeMethods.CertSetCertificateContextProperty(storeCertContext, CERT_KEY_PROV_INFO_PROP_ID, 0, ref kpi);
				return ExportCertificate(certStore, password);
			}
			finally
			{
				if (certContext != IntPtr.Zero)
				{
					NativeMethods.CertFreeCertificateContext(certContext);
				}

				if (storeCertContext != IntPtr.Zero)
				{
					NativeMethods.CertFreeCertificateContext(storeCertContext);
				}

				if (certStore != IntPtr.Zero)
				{
					NativeMethods.CertCloseStore(certStore, 0);
				}

				if (cryptKey != IntPtr.Zero)
				{
					NativeMethods.CryptDestroyKey(cryptKey);
				}

				if (providerContext != IntPtr.Zero)
				{
					NativeMethods.CryptReleaseContext(providerContext, 0);
					NativeMethods.CryptAcquireContextW(out providerContext, containerName, null, PROV_RSA_FULL, NativeMethods.CRYPT_DELETEKEYSET);
				}
			}
		}

		private static byte[] ExportCertificate(IntPtr certStore, SecureString password)
		{
			GCHandle dataHandle = new GCHandle();
			IntPtr passwordPtr = IntPtr.Zero;
			if (password != null)
			{
				passwordPtr = Marshal.SecureStringToCoTaskMemUnicode(password);
			}

			try
			{
				byte[] pfxData;
				CryptoApiBlob pfxBlob = new CryptoApiBlob();
				Check(NativeMethods.PFXExportCertStoreEx(
			      		certStore,
			      		ref pfxBlob,
			      		passwordPtr,
			      		IntPtr.Zero,
			      		7)); // EXPORT_PRIVATE_KEYS | REPORT_NO_PRIVATE_KEY | REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY

				pfxData = new byte[pfxBlob.DataLength];
				dataHandle = GCHandle.Alloc(pfxData, GCHandleType.Pinned);
				pfxBlob.Data = dataHandle.AddrOfPinnedObject();
				Check(NativeMethods.PFXExportCertStoreEx(
			      		certStore,
			      		ref pfxBlob,
			      		passwordPtr,
			      		IntPtr.Zero,
			      		7)); // EXPORT_PRIVATE_KEYS | REPORT_NO_PRIVATE_KEY | REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY
				
				return pfxData;
			}
			finally
			{
				if (passwordPtr != IntPtr.Zero)
				{
					Marshal.ZeroFreeCoTaskMemUnicode(passwordPtr);
				}

				if (dataHandle.IsAllocated)
				{
					dataHandle.Free();
				}
			}
		}

		private static IntPtr CreateSelfSignedCertificate(IntPtr providerContext, string distinguishedName, DateTime startTime, DateTime endTime, ref CryptKeyProviderInformation kpi)
		{
			SystemTime startSystemTime = ToSystemTime(startTime);
			SystemTime endSystemTime = ToSystemTime(endTime);

			byte[] nameData = GetNameData(distinguishedName);
			GCHandle dataHandle = GCHandle.Alloc(nameData, GCHandleType.Pinned);
			try
			{
				CryptoApiBlob nameBlob = new CryptoApiBlob(nameData.Length, dataHandle.AddrOfPinnedObject());
				IntPtr certContext = NativeMethods.CertCreateSelfSignCertificate(
					providerContext,
					ref nameBlob,
					0,
					ref kpi,
					IntPtr.Zero, // default = SHA1RSA
					ref startSystemTime,
					ref endSystemTime,
					IntPtr.Zero);
				Check(certContext != IntPtr.Zero);
				dataHandle.Free();

				return certContext;
			}
			finally
			{
				if (dataHandle.IsAllocated)
				{
					dataHandle.Free();
				}
			}
		}

		private static CryptKeyProviderInformation NewProviderInformationFor(string containerName, int providerType, int keyexchange)
		{
			CryptKeyProviderInformation kpi = new CryptKeyProviderInformation();
			kpi.ContainerName = containerName;
			kpi.ProviderType = providerType;
			kpi.KeySpec = keyexchange;
			kpi.Flags = NativeMethods.CRYPT_MACHINE_KEYSET;

			return kpi;
		}

		private static byte[] GetNameData(string distinguishedName)
		{
			IntPtr errorStringPtr;
			int nameDataLength = 0;

			GCHandle dataHandle = new GCHandle();

			// errorStringPtr gets a pointer into the middle of the distinguishedName string,
			// so distinguishedName needs to be pinned until after we've copied the value
			// of errorStringPtr.

			try
			{
				dataHandle = GCHandle.Alloc(distinguishedName, GCHandleType.Pinned);
				if (!NativeMethods.CertStrToNameW(
				     	ENCODING,
				     	dataHandle.AddrOfPinnedObject(),
				     	CERT_X500_NAME_STR,
				     	IntPtr.Zero,
				     	null,
				     	ref nameDataLength,
				     	out errorStringPtr))
				{
					string error = Marshal.PtrToStringUni(errorStringPtr);
					throw new ArgumentException(error);
				}

				byte[] nameData = new byte[nameDataLength];
				if (!NativeMethods.CertStrToNameW(
				     	ENCODING,
				     	dataHandle.AddrOfPinnedObject(),
				     	CERT_X500_NAME_STR,
				     	IntPtr.Zero,
				     	nameData,
				     	ref nameDataLength,
				     	out errorStringPtr))
				{
					string error = Marshal.PtrToStringUni(errorStringPtr);
					throw new ArgumentException(error);
				}

				return nameData;
			}
			finally
			{
				if (dataHandle.IsAllocated)
				{
					dataHandle.Free();
				}
			}
		}

		private static IntPtr GenerateKey(IntPtr providerContext, int algorithmId, int flags)
		{
			IntPtr cryptKey;
			Check(NativeMethods.CryptGenKey(providerContext, algorithmId, flags, out cryptKey));

			return cryptKey;
		}

		private static IntPtr AcquireProviderContext(string containerName, int providerType, int flags)
		{
			IntPtr providerContext;
			Check(NativeMethods.CryptAcquireContextW(out providerContext, containerName, null, providerType, flags));
			return providerContext;
		}

		private static SystemTime ToSystemTime(DateTime dateTime)
		{
			long fileTime = dateTime.ToFileTime();
			SystemTime systemTime;
			Check(NativeMethods.FileTimeToSystemTime(ref fileTime, out systemTime));
			return systemTime;
		}

		private static void Check(bool nativeCallSucceeded)
		{
			if (!nativeCallSucceeded)
			{
				int error = Marshal.GetHRForLastWin32Error();
				Marshal.ThrowExceptionForHR(error);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct SystemTime
		{
			public short Year;
			public short Month;
			public short DayOfWeek;
			public short Day;
			public short Hour;
			public short Minute;
			public short Second;
			public short Milliseconds;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct CryptoApiBlob
		{
			public int DataLength;
			public IntPtr Data;

			public CryptoApiBlob(int dataLength, IntPtr data)
			{
				DataLength = dataLength;
				Data = data;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct CryptKeyProviderInformation
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string ContainerName;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string ProviderName;
			public int ProviderType;
			public int Flags;
			public int ProviderParameterCount;
			public IntPtr ProviderParameters; // PCRYPT_KEY_PROV_PARAM
			public int KeySpec;
		}

		private static class NativeMethods
		{
			internal const int CRYPT_NEWKEYSET = 8;
			internal const int CRYPT_EXPORTABLE = 1;
			internal const int CRYPT_MACHINE_KEYSET = 0x00000020;
			internal const int CRYPT_DELETEKEYSET = 0x10;

			[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool FileTimeToSystemTime([In] ref long fileTime, out SystemTime systemTime);

			[DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CryptAcquireContextW(
				out IntPtr providerContext, 
				[MarshalAs(UnmanagedType.LPWStr)] string container,
				[MarshalAs(UnmanagedType.LPWStr)] string provider,
				int providerType,
				int flags);

			[DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CryptReleaseContext(
				IntPtr providerContext,
				int flags);

			[DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CryptGenKey(
				IntPtr providerContext,
				int algorithmId,
				int flags,
				out IntPtr cryptKeyHandle);

			[DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CryptDestroyKey(
				IntPtr cryptKeyHandle);

			[DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CertStrToNameW(
				int certificateEncodingType,
				IntPtr x500,
				int strType,
				IntPtr reserved,
				[MarshalAs(UnmanagedType.LPArray)] [Out] byte[] encoded,
				ref int encodedLength,
				out IntPtr errorString);

			[DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
			public static extern IntPtr CertCreateSelfSignCertificate(
				IntPtr providerHandle,
				[In] ref CryptoApiBlob subjectIssuerBlob,
				int flags,
				[In] ref CryptKeyProviderInformation keyProviderInformation,
				IntPtr signatureAlgorithm,
				[In] ref SystemTime startTime,
				[In] ref SystemTime endTime,
				IntPtr extensions);

			[DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CertFreeCertificateContext(
				IntPtr certificateContext);

			[DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
			public static extern IntPtr CertOpenStore(
				[MarshalAs(UnmanagedType.LPStr)] string storeProvider,
				int messageAndCertificateEncodingType,
				IntPtr cryptProvHandle,
				int flags,
				IntPtr parameters);

			[DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CertCloseStore(
				IntPtr certificateStoreHandle,
				int flags);

			[DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CertAddCertificateContextToStore(
				IntPtr certificateStoreHandle,
				IntPtr certificateContext,
				int addDisposition,
				out IntPtr storeContextPtr);

			[DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CertSetCertificateContextProperty(
				IntPtr certificateContext,
				int propertyId,
				int flags,
				[In] ref CryptKeyProviderInformation data);

			[DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool PFXExportCertStoreEx(
				IntPtr certificateStoreHandle,
				ref CryptoApiBlob pfxBlob,
				IntPtr password,
				IntPtr reserved,
				int flags);
		}
	}
}

#endif
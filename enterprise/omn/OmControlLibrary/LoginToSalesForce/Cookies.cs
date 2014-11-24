using System;
using System.IO;
using OME.Crypto;

namespace OMControlLibrary.LoginToSalesForce
{
	public class CustomCookies
	{
		private readonly CryptoDES objCryptoDES = new CryptoDES();

		public CustomCookies()
		{
			objCryptoDES.Initialize();
		}

		public void SetCookies(string content)
		{
#if DEBUG

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise"))
            {

                //Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise");

            }
#endif
			string filepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise" + Path.DirectorySeparatorChar + "encyr.info";
			FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
			string encryptSTR = objCryptoDES.DESSelfEncrypt(content);
			byte[] contents = StrToByteArray(encryptSTR);
			fs.Write(contents, 0, contents.Length);
			fs.Close();
		}

		private byte[] StrToByteArray(string str)
		{
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			return encoding.GetBytes(str);
		}

		public string GetCookies()
		{
#if DEBUG

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise"))
            {

                //Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise");

            }
#endif
			string filepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise" + Path.DirectorySeparatorChar + "encyr.info";

			if (File.Exists(filepath))
			{
				byte[] contents;
				using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
				{
					contents = new byte[fs.Length];
					fs.Read(contents, 0, (int)fs.Length);
				}

				if (contents.Length > 0)
				{
					string info = ByteArrayToStr(contents);
					return objCryptoDES.DESSelfDecrypt(info);
				}
			}
			return null;
		}

		private string ByteArrayToStr(byte[] array)
		{
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			return encoding.GetString(array);
		}

	}
}

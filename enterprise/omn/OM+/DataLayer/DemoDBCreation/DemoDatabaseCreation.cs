using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OManager.BusinessLayer.FavFolder;
using OManager.BusinessLayer.UIHelper;
using OManager.DataLayer.Connection;
using Db4objects.Db4o;
using System.Collections;
using DemoDbCreation;
using OManager.BusinessLayer.Login;


namespace OManager.DataLayer.DemoDBCreation
{
    class DemoDatabaseCreation
    {
        public void CreateDemoDb(string filepath)
        {

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
           
            #region creation
            IObjectContainer objContainer = Db4oEmbedded.OpenFile(filepath);

            for (int i = 0; i < 7; i++)
            {
                try
                {
                 
                    Pilot p1 = new Pilot("John " + i, i + 2);
                    Car c1 = new Car("Ferrari " + i);
                    c1.Count = i;
                    c1.List = new ArrayList();
                    for (int l = 0; l < 15; l++)
                    {
                        c1.List.Add(l + 77);
                    }

                    c1.Pilot = p1;
                    Children ch1 = new Children();
                    ch1.Child_name = "Baby " + i;
                    ch1.Child_no = 1;
                    ch1.DateOfBirth = DateTime.Now;
                    c1.Pilot.Children = new Children();
                    c1.Pilot.Children = ch1;
                    c1.Pilot.Dbl = i / 3;
                    c1.Pilot.ByteProp = (Byte)(1.1 * i * 255.0);
                    c1.Pilot.SbyteProp = (SByte)(1.23 * i * 55.0);
                    c1.Pilot.SingleProp = 2;
                    c1.Htbl = new Hashtable();

                    for (int m = 0; m < 10; m++)
                    {
                        c1.Htbl.Add(m + 7, "value " + m + 9);
                    }


                    c1.CharArr = new char[2];
                    c1.CharArr[0] = '$';
                    c1.CharArr[1] = '*';

                    c1.DblArr = new float[6];
                    for (int n = 0; n < 6; n++)
                    {
                        c1.DblArr[n] = n + 33;
                    }
                    if (i % 2 == 0)
                        c1.Pilot.HasWon = false;
                    else
                        c1.Pilot.HasWon = true;

                    c1.IntArrl = new int[] { 1 + i, 2 + i, 3 + i };
                    c1.Int16 = 12;
                    c1.String = "String Ferrari" + i;
                    c1.Character = '!';
                    c1.DecimalProp = 0.7M;

                    objContainer.Store(c1);
                    objContainer.Commit();
                }

                catch { }
            }
            for (int i = 0; i < 7; i++)
            {
                try
                {
                    Pilot p1 = new Pilot("Michael " + i, i + 2);
                    Car c1 = new Car("BMW " + i);
                    c1.Count = i;
                    c1.List = new ArrayList();
                    for (int k = 0; k < 3; k++)
                    {
                        c1.List.Add(k + 55);
                    }

                    c1.Pilot = p1;
                    Children ch1 = new Children();
                    ch1.Child_name = "Child " + i;
                    ch1.Child_no = 1;
                    c1.Pilot.Children = new Children();
                    c1.Pilot.Children = ch1;
                    c1.Pilot.Dbl = i / 3;
                    c1.Pilot.ByteProp = (Byte)(1.5 * i * 255.0);
                    c1.Pilot.SbyteProp = (SByte)(1.4 * i * 55.0);
                    c1.Pilot.SingleProp = 2;
                    c1.Htbl = new Hashtable();
                    for (int l = 0; l < 10; l++)
                    {
                        c1.Htbl.Add("key no" + l + 34, "Value " + l + 12);
                    }

                    c1.CharArr = new char[3];
                    c1.CharArr[0] = '!';
                    c1.CharArr[1] = '&';
                    c1.CharArr[2] = '^';

                    c1.DblArr = new float[8];
                    for (int m = 0; m < 8; m++)
                    {
                        c1.DblArr[m] = m + 21;
                    }
                    if (i % 2 == 0)
                        c1.Pilot.HasWon = true;
                    else
                        c1.Pilot.HasWon = false;

                    c1.IntArrl = new int[] { 4 + i, 5 + i, 7 + i };
                    c1.Int16 = 12;
                    c1.String = "String BMW" + i;
                    c1.Character = '(';
                    c1.DecimalProp = 0.4M;

                    objContainer.Store(c1);
                    objContainer.Commit();
                }
                catch { }
            }

            objContainer.Close();
            
            #endregion
			RefreshConnectionData(filepath);
        }

    	private void RefreshConnectionData(string filepath)
    	{
    		ConnParams conParams = new ConnParams(filepath, false);
            long id = OMEInteraction.ChkIfRecentConnIsInDb(conParams);
            if (id > 0)
            {
                ConnectionDetails connectionDetails = OMEInteraction.GetConnectionDetailsObject(id);
                if (connectionDetails != null)
                {
                    OMEInteraction.RemoveFavFolder();
                    OMEInteraction.RemoveSearchString();
                    OMEInteraction.RemoveRecentQueries();
                }
            }
    	}
    }
}

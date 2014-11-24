using System.Windows.Forms;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Microsoft.Reporting.WinForms;

namespace Db4oDoc.Code.Reporting
{
    public partial class ReportForm : Form
    {
        private readonly IObjectContainer container;
        public ReportForm()
        {
            InitializeComponent();
            container = Db4oEmbedded.OpenFile("database.db4o");
            StoreData(container);
            ShowReport(container);
        }

        private void ShowReport(IObjectContainer container)
        {
            // #example: Run a report with db4o
            var dataToShow = from Person p in container
                             where p.FirstName.Contains("o")
                             select p;

            var reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local
            };

            // Put the data into the datasource which you are using
            // in your report. Here it's named 'MainData'
            reportViewer.LocalReport.DataSources.Add(
                new ReportDataSource("MainData", dataToShow));
            reportViewer.Dock = DockStyle.Fill;

            // The report can be an embedded resource
            reportViewer.LocalReport.ReportEmbeddedResource = "Db4oDoc.Code.Reporting.ExampleReport.rdlc";
            // or can be a file
            // reportViewer.LocalReport.ReportPath = "path/to/your/report"; 

            // After that you can use the report viewer in your app
            this.Controls.Add(reportViewer);
            reportViewer.RefreshReport();
            // #end example
        }


        private void StoreData(IObjectContainer container)
        {
            container.Store(new Person("Roman","Stoffel")
                                {
                                    Address = new Address()
                                                  {
                                                      City = "Zurich",
                                                      Street = "FunStreet"
                                                  }
                                });    
            container.Store(new Person("Joe","Cool")
                                {
                                    Address = new Address()
                                                  {
                                                      City = "FunTown",
                                                      Street = "Fun Plaza"
                                                  }
                                });
            container.Store(new Person("Joanna", "Awesome")
            {
                Address = new Address()
                {
                    City = "More Fun Town",
                    Street = "Plaza"
                }
            });          
        }


    }
}

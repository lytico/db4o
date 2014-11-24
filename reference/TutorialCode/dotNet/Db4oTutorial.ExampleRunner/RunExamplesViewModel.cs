using System;
using System.ComponentModel;
using Db4oTutorial.ExampleRunner.Utils;

namespace Db4oTutorial.ExampleRunner
{
    public class RunExamplesViewModel : INotifyPropertyChanged
    {
        private string code;
        public string Code  
        {
            get { return code; }
            set { 
                code = value;
                PropertyChanged.Fire(this, () => Code);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
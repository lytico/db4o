
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oDoc
{
    class ComplexNumberHolder   
    {
        Complex number = new Complex(3, 5);

        public ComplexNumberHolder(int real,int imaginary)
        {
            this.number = new Complex(real, imaginary);
        }
        public ComplexNumberHolder(Complex number)
        {
            this.number = number;
        }

        public Complex Number
        {
            get { return number; }
            set { number = value; }
        }
    }
    class RefHolder
    {
        public object reference;
    }
    public class Boot
    {
        [STAThread]
        public static void Main(string[] args)
        {
        }
    }
}
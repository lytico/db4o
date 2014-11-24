using System;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Collections
{
    class CodeBlockRunner<T> : ICodeBlock
    {
        private readonly Action<T> method;
        private T value;

        internal CodeBlockRunner(Action<T> method, T value)
        {
            this.method = method;
            this.value = value;
        }

        internal CodeBlockRunner(Action<T> method) : this(method, default(T))
        {
        }

        public void Run()
        {
            method(value);
        }

        public ICodeBlock WithValue(T value)
        {
            this.value = value;
            return this;
        }
    }
}
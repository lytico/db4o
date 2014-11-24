using System;

namespace Db4oTutorial.ExampleRunner.Utils
{
    public static class OperationResult
    {
        public static OperationResult<T> Success<T>(T result)
        {
            return new OperationResult<T>(result);
        }

        public static OperationResult<T> Failure<T>(Exception exception)
        {
            return new OperationResult<T>(exception);
        }
    }

    public class OperationResult<T>
    {
        private readonly T result;
        private readonly Exception exeption;

        internal OperationResult(T result)
        {
            this.result = result;
        }

        internal OperationResult(Exception execption)
        {
            this.exeption= execption;
        }

        public T Result { get
        {
            if(null!=exeption)
            {
                throw new InvalidOperationException("Cannot get value of a failure",exeption);
            }
            return result;
        } }
        public Exception Exception { get { return exeption; } }

        public bool IsFailure
        {
            get { return null != exeption; }
        }
    }
}
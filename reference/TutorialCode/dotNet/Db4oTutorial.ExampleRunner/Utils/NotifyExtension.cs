using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Db4oTutorial.ExampleRunner.Utils
{
    public static class NotifyExtensions
    {

        public static void Fire<TSender, TPropertyValue>(this PropertyChangedEventHandler handler,
                                                         TSender sender,
                                                         Expression<Func<TSender, TPropertyValue>> property)
        {
            if (null == handler)
            {
                return;
            }
            FireImpl(handler, sender, property);
        }

        public static void Fire<TPropertyValue>(this PropertyChangedEventHandler handler,
                                                         object sender,
                                                         Expression<Func<TPropertyValue>> property)
        {
            if (null == handler)
            {
                return;
            }
            FireImpl(handler, sender, property);
        }

        private static void FireImpl<TSender>(PropertyChangedEventHandler handler, TSender sender, Expression property)
        {
            if (null == sender)
            {
                throw new ArgumentNullException("sender");
            }
            var lamda = ThrowOnNull(property, property as LambdaExpression);
            var call = ThrowOnNull(property, lamda.Body as MemberExpression);
            CheckIsProperty(property, call);
            var nameToFire = call.Member.Name;
            handler(sender, new PropertyChangedEventArgs(nameToFire));
        }

        private static void CheckIsProperty(Expression property, MemberExpression call)
        {
            if (MemberTypes.Property != call.Member.MemberType)
            {
                ThrowNotValidLambda(property);
            }
        }

        private static T ThrowOnNull<T>(Expression property, T lamda) where T : class
        {
            if (null == lamda)
            {
                ThrowNotValidLambda(property);
            }
            return lamda;
        }

        private static void ThrowNotValidLambda(Expression property)
        {
            throw new ArgumentException("Require a property-access, but is :" + property);
        }
    }
}
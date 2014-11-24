using System;
using System.Collections.Generic;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4oTool.MSBuild.Tests
{
    public class CommandlineTestCase : AbstractDb4oTestCase
    {
        public void testByName()
        {
            Type intItemType = typeof(IntItem);
            Assert.IsTrue(IsInstrumented(intItemType));

            Type nonTAItemType = typeof(NonTAItem);
            Assert.IsFalse(IsInstrumented(nonTAItemType));
        }

        public void testByName2()
        {
            Type taIntItemType = Type.GetType("Db4oTool.MSBuild.Tests.Project.TAIntItem, Db4oTool.MSBuild.Tests.Project", true);

            Assert.IsTrue(IsInstrumented(taIntItemType));

            Type stringItemType = Type.GetType("Db4oTool.MSBuild.Tests.Project.StringItem, Db4oTool.MSBuild.Tests.Project", true);
            Assert.IsFalse(IsInstrumented(stringItemType));
        }

        private bool IsInstrumented(Type type)
        {
            Type t = type.GetInterface("Db4objects.Db4o.TA.IActivatable");
            return (t != null);
        }
    }
}

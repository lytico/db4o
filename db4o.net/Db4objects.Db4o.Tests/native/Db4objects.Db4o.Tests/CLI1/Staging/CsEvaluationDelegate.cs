/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;

/**
 *  COR-1432
 */
namespace Db4objects.Db4o.Tests.CLI1
{
    public class CsEvaluationDelegate : AbstractDb4oTestCase
    {
        internal CsEvaluationDelegate child;
        internal String name;

        override protected void Store()
        {
            name = "one";
            Store(this);
            CsEvaluationDelegate se1 = new CsEvaluationDelegate();
            se1.child = new CsEvaluationDelegate();
            se1.child.name = "three";
            se1.name = "two";
            Store(se1);
            se1 = new CsEvaluationDelegate();
            se1.child = new CsEvaluationDelegate();
            se1.child.name = "five";
            se1.name = "four";
            Store(se1);
        }

        public void TestStaticMethodDelegate()
        {
            RunEvaluationDelegateTest(new EvaluationDelegate(Evaluate));
        }

        public void TestInstanceMethodDelegate()
        {
            RunEvaluationDelegateTest(new EvaluationDelegate(new NameCondition("three").Evaluate));
        }

        void RunEvaluationDelegateTest(EvaluationDelegate evaluation)
        {
            IQuery q1 = NewQuery();
            IQuery cq1 = q1;
            q1.Constrain(GetType());
            cq1 = cq1.Descend("child");
            cq1.Constrain(evaluation);
            IObjectSet os = q1.Execute();
            Assert.AreEqual(1, os.Count);
            CsEvaluationDelegate se = (CsEvaluationDelegate)os.Next();
            Assert.AreEqual("two", se.name);
        }

        public static void Evaluate(ICandidate candidate)
        {
            CsEvaluationDelegate obj = ((CsEvaluationDelegate)candidate.GetObject());
            candidate.Include(obj.name.Equals("three"));
        }

        class NameCondition
        {
            string _name;

            public NameCondition(string name)
            {
                _name = name;
            }

            public void Evaluate(ICandidate candidate)
            {
                CsEvaluationDelegate obj = ((CsEvaluationDelegate)candidate.GetObject());
                candidate.Include(obj.name.Equals(_name));
            }
        }
    }
}

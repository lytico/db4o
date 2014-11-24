/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.NativeQueries.Expr.Cmp
{
	public sealed class ArithmeticOperator
	{
		public const int AddId = 0;

		public const int SubtractId = 1;

		public const int MultiplyId = 2;

		public const int DivideId = 3;

		public const int ModuloId = 4;

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator 
			Add = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator(AddId, "+");

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator 
			Subtract = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator(SubtractId
			, "-");

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator 
			Multiply = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator(MultiplyId
			, "*");

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator 
			Divide = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator(DivideId, 
			"/");

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator 
			Modulo = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator(ModuloId, 
			"%");

		private string _op;

		private int _id;

		private ArithmeticOperator(int id, string op)
		{
			_id = id;
			_op = op;
		}

		public int Id()
		{
			return _id;
		}

		public override string ToString()
		{
			return _op;
		}
	}
}

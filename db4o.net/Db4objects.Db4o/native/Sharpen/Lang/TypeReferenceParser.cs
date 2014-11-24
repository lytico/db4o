/* Copyright (C) 2005   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Sharpen.Lang
{
	internal class TypeReferenceParser
	{
		private readonly TypeReferenceLexer _lexer;
		private readonly Stack<Token> _stack = new Stack<Token>();
		
		public TypeReferenceParser(string input)
		{
			_lexer = new TypeReferenceLexer(input);
		}

		public TypeReference Parse()
		{
			SimpleTypeReference str = ParseSimpleTypeReference();
			TypeReference returnValue = ParseQualifiedTypeReference(str);
			Token token = NextToken();
			if (null != token)
			{
				switch (token.Kind)
				{
					case TokenKind.Comma:
						str.SetAssemblyName(ParseAssemblyName());
						break;
					default:
						UnexpectedToken(TokenKind.Comma, token);
						break;
				}
			}
			return returnValue;
		}
		
		private TypeReference ParseQualifiedTypeReference(TypeReference elementType)
		{
			TypeReference returnValue = elementType;
			
			Token token;
			while (null != (token = NextToken()))
			{
				switch (token.Kind)
				{
					case TokenKind.LBrack:
						returnValue = ParseArrayTypeReference(returnValue);
						break;
					case TokenKind.PointerQualifier:
						returnValue = new PointerTypeReference(returnValue);
						break;
					default:
						Push(token);
						return returnValue;
				}
			}
			
			return returnValue;
		}

		private TypeReference ParseArrayTypeReference(TypeReference str)
		{
			int rank = 1;
			Token token = NextToken();
			while (null != token && token.Kind == TokenKind.Comma)
			{
				++rank;
				token = NextToken();
			}
			AssertTokenKind(TokenKind.RBrack, token);

			return new ArrayTypeReference(str, rank);
		}

		private SimpleTypeReference ParseSimpleTypeReference()
		{
			Token id = Expect(TokenKind.Id);

			Token t = NextToken();
			if (null == t) return new SimpleTypeReference(id.Value);

			while (TokenKind.NestedQualifier == t.Kind)
			{
				Token nestedId = Expect(TokenKind.Id);
				id.Value += "+" + nestedId.Value;

				t = NextToken();
				if (null == t) return new SimpleTypeReference(id.Value);
			}
			
			if (t.Kind == TokenKind.GenericQualifier)
			{
				return ParseGenericTypeReference(id);
			}
			
			Push(t);
			return new SimpleTypeReference(id.Value);
		}

		private SimpleTypeReference ParseGenericTypeReference(Token id)
		{
			return InternalParseGenericTypeReference(id, 0);
		}

		private SimpleTypeReference InternalParseGenericTypeReference(Token id, int count)
		{
			Token argcToken = Expect(TokenKind.Number);
			id.Value += "`" + argcToken.Value;

			int argc = int.Parse(argcToken.Value);

			Token t = NextToken();
			while (TokenKind.NestedQualifier == t.Kind)
			{
				Token nestedId = Expect(TokenKind.Id);
				id.Value += "+" + nestedId.Value;

				t = NextToken();
			}

			if (IsInnerGenericTypeReference(t))
			{
				return InternalParseGenericTypeReference(id, argc + count);
			}

			TypeReference[] args = new TypeReference[0];
			if (!IsOpenGenericTypeDefinition(t))
			{
				args = new TypeReference[argc + count];
				AssertTokenKind(TokenKind.LBrack, t);
				for (int i = 0; i < args.Length; ++i)
				{
					if (i > 0) Expect(TokenKind.Comma);
					Expect(TokenKind.LBrack);
					args[i] = Parse();
					Expect(TokenKind.RBrack);
				}
				Expect(TokenKind.RBrack);
			}
			else
			{
				Push(t);
			}

			return new GenericTypeReference(id.Value, args);
		}

		private static bool IsOpenGenericTypeDefinition(Token t)
		{
			return t.Kind != TokenKind.LBrack;
		}

		private static bool IsInnerGenericTypeReference(Token t)
		{
			return TokenKind.GenericQualifier == t.Kind;
		}

		public AssemblyName ParseAssemblyName()
		{
			Token simpleName = _lexer.SimpleName();

			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = simpleName.Value;

			if (!CommaIdEquals()) return assemblyName;

			Token version = Expect(TokenKind.VersionNumber);
			assemblyName.Version = new Version(version.Value);

			if (!CommaIdEquals()) return assemblyName;
			
			Token culture = Expect(TokenKind.Id);
			if ("neutral" == culture.Value)
			{
				assemblyName.CultureInfo = CultureInfo.InvariantCulture;
			}
			else
			{
#if SILVERLIGHT
                assemblyName.CultureInfo = CultureInfo.InvariantCulture;
#else
				assemblyName.CultureInfo = CultureInfo.CreateSpecificCulture(culture.Value);
#endif
			}

			if (!CommaIdEquals()) return assemblyName;
			
			Token token = NextToken();
			if ("null" != token.Value)
			{
				assemblyName.SetPublicKeyToken(ParsePublicKeyToken(token.Value));
			}

			return assemblyName;
		}

		static byte[] ParsePublicKeyToken(string token)
		{
			int len = token.Length / 2;
			byte[] bytes = new byte[len];
			for (int i = 0; i < len; ++i)
			{
				bytes[i] = byte.Parse(token.Substring(i * 2, 2), NumberStyles.HexNumber);
			}
			return bytes;
		}

		private bool CommaIdEquals()
		{
			Token token = NextToken();
			if (null == token) return false;
			if (token.Kind != TokenKind.Comma)
			{
				Push(token);
				return false;
			}
			
			AssertTokenKind(TokenKind.Comma, token);
			Expect(TokenKind.Id);
			Expect(TokenKind.Equals);
			return true;
		}

		Token Expect(TokenKind expected)
		{
			Token actual = NextToken();
			AssertTokenKind(expected, actual);
			return actual;
		}

		private static void AssertTokenKind(TokenKind expected, Token actual)
		{
			if (null == actual || actual.Kind != expected)
			{
				UnexpectedToken(expected, actual);
			}
		}

		private static void UnexpectedToken(TokenKind expectedKind, Token actual)
		{
			throw new ArgumentException(string.Format("Unexpected Token: '{0}' (Expected kind: '{1}')", actual, expectedKind));
		}
		
		private void Push(Token token)
		{
			_stack.Push(token);
		}
	
		private Token NextToken()
		{
			return _stack.Count > 0
				? _stack.Pop()
				: _lexer.NextToken();
		}
	}
}
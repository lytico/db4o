/* Copyright (C) 2005   Versant Inc.   http://www.db4o.com */
using System;
using System.Text;

namespace Sharpen.Lang
{
	internal enum TokenKind
	{
		Comma,
		Equals,
		Id,
		Number,
		VersionNumber,
		GenericQualifier,
		NestedQualifier,
		LBrack,
		RBrack,
		PointerQualifier,
		SimpleName
	}

	internal class Token
	{
		public TokenKind Kind;
		public string Value;

		public Token(TokenKind kind, string value)
		{
			Kind = kind;
			Value = value;
		}

		public override string ToString()
		{
			return string.Format("Token(Kind: {0}, Value: '{1}')", Kind, Value);
		}
	}

	internal class TypeReferenceLexer
	{
		string _input;
		int _index;
		StringBuilder _buffer; // TODO: get rid of StringBuilder and use string.Substring

		public TypeReferenceLexer(string input)
		{
			if (null == input) throw new ArgumentNullException("input");
			_input = input;
			_index = 0;
			_buffer = new StringBuilder(input.Length);
		}

		bool AtEOF
		{
			get { return _index == _input.Length; }
		}

		public Token NextToken()
		{
			if (AtEOF) return null;

			char ch = Peek();
			switch (ch)
			{
				case '*':
					return ConsumeSingleCharToken(ch, TokenKind.PointerQualifier);
				case '+':
					return ConsumeSingleCharToken(ch, TokenKind.NestedQualifier);
				case '[':
					return ConsumeSingleCharToken(ch, TokenKind.LBrack);
				case ']':
					return ConsumeSingleCharToken(ch, TokenKind.RBrack);
				case '=':
					return ConsumeSingleCharToken(ch, TokenKind.Equals);
				case ',':
					return ConsumeSingleCharToken(ch, TokenKind.Comma);
				case '`':
					return ConsumeSingleCharToken(ch, TokenKind.GenericQualifier);
				case ' ':
					Consume();
					return NextToken();

				default:
					if (IsIdStart(ch)) return Id();
					if (char.IsDigit(ch)) return NumberOrVersion();
					break;
			}
			throw new Exception(string.Format("Unexpected char '{0}'", ch));
		}


		private static bool IsIdStart(char ch)
		{
			switch (ch)
			{
				case '_':
				case '<': // c# compiler generated classes
					return true;
			}
			return char.IsLetter(ch);
		}

		private Token Id()
		{
			do
			{
				char ch = Peek();
				if (!char.IsLetterOrDigit(ch)
					&& '.' != ch
					&& '-' != ch
					&& '_' != ch
					&& '<' != ch
					&& '>' != ch
					&& ':' != ch)
				{
					break;
				}
				ConsumeAndBuffer(ch);
			}
			while (!AtEOF);
			return TokenFromBuffer(TokenKind.Id);
		}

		// Assembly name parsing based on:
		// http://blogs.msdn.com/b/junfeng/archive/2004/09/14/229254.aspx
		public Token SimpleName()
		{
			char ch = Peek();
			while(!AtEOF && char.IsWhiteSpace(ch))
			{
				Consume();
				ch = Peek();
			}

			//TODO: Correcly handle commas
			while(!AtEOF)
			{
				ch = Peek();
				if (!char.IsLetterOrDigit(ch)
					&& (ch == ',' || ch == '/' || ch == '\\' || ch == ':' || ch == '*' || ch == '?' || ch == '"' || ch == '<' || ch == '>' || ch == '|' 
					              || ch == '[' || ch == ']'))
				{
					break;
				}
				ConsumeAndBuffer(ch);

			}
			return TokenFromBuffer(TokenKind.SimpleName);
		}

		private Token NumberOrVersion()
		{
			TokenKind kind = TokenKind.Number;

			do
			{
				char ch = Peek();
				if ('.' == ch)
				{
					kind = TokenKind.VersionNumber;
				}
				else 
				{
					if (!IsHexDigit(ch)) 
					{
						break;
					}
				}
				ConsumeAndBuffer(ch);
			}
			while (!AtEOF);
			return TokenFromBuffer(kind);
		}

		private static bool IsHexDigit(char ch)
		{
			return char.IsDigit(ch) || IsHexLetter(ch);
		}

		private static bool IsHexLetter(char ch)
		{
			return (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f');
		}

		private void ConsumeAndBuffer(char ch)
		{
			Consume();
			_buffer.Append(ch);
		}

		private Token TokenFromBuffer(TokenKind kind)
		{
			Token token = new Token(kind, _buffer.ToString());
			_buffer.Length = 0;
			return token;
		}

		private Token ConsumeSingleCharToken(char ch, TokenKind kind)
		{
			Consume();
			return new Token(kind, new string(ch, 1));
		}

		void Consume()
		{
			++_index;
		}

		char Peek()
		{
			return _input[_index];
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNLang.SyntaxAnalyzer.Token;

namespace DNLang {
    class Lexer {
        private const char OPEN_BRACKET = '[';
        private const char CLOSE_BRACKET = ']';
        private const char SEMICOLON = ';';
        private const char ASSIGN = '=';
        private const char COMMA = ',';
        private const char COLON = ':';
        private const char PLUS = '+';
        private const char DOUBLE_QUOTE = '"';

        private char[] inputBuffer;
        private int charIndex;

        public Lexer(char[] inputBuffer) {
            charIndex = 0;
            this.inputBuffer = inputBuffer;
        }

        public List<Token> Tokenize() {
            var tokens = new List<Token>();
            var buffer = "";

            while (!IsEndOfBuffer()) {
                if (IsWhitespace(Peek())) {
                    SkipWhitespaces();
                    continue;
                }

                if (Peek() == DOUBLE_QUOTE) {
                    buffer = ScanString();

                    tokens.Add(new Token(TokenKind.String, buffer));
                    continue;
                } else if (IsIdentifier(Peek())) {
                    buffer = ScanIdentifier();

                    tokens.Add(new Token(TokenKind.Identifier, buffer));
                    continue;
                } else if (IsDecimalDigit(Peek())) {
                    buffer = ScanDecimalDigits();

                    tokens.Add(new Token(TokenKind.Number, buffer));
                    continue;
                } else if (Peek() == OPEN_BRACKET) {
                    tokens.Add(new Token(TokenKind.OpenBracket, Peek().ToString()));
                } else if (Peek() == CLOSE_BRACKET) {
                    tokens.Add(new Token(TokenKind.CloseBracket, Peek().ToString()));
                } else if (Peek() == SEMICOLON) {
                    tokens.Add(new Token(TokenKind.Semicolon, Peek().ToString()));
                } else if (Peek() == COLON) {
                    tokens.Add(new Token(TokenKind.Colon, Peek().ToString()));
                } else if (Peek() == ASSIGN) {
                    tokens.Add(new Token(TokenKind.Assign, Peek().ToString()));
                } else if (Peek() == COMMA) {
                    tokens.Add(new Token(TokenKind.Comma, Peek().ToString()));
                } else if (Peek() == PLUS) {
                    tokens.Add(new Token(TokenKind.Plus, Peek().ToString()));
                } else {
                    throw new Exception($"Unexpected char: {Peek()}");
                }

                Next();
            }

            return tokens;
        }

        public bool Next() {
            if (IsEndOfBuffer()) return false;
            charIndex++;
            return true;
        }

        public char Peek(int offset = 0) {
            if (IsEndOfBuffer()) throw new Exception("Unexprected end of stream.");

            return inputBuffer[charIndex + offset];
        }

        public bool IsEndOfBuffer(int offset = 0) {
            return charIndex + offset >= inputBuffer.Length;
        }

        public void SkipWhitespaces() {
            while (!IsEndOfBuffer() && IsWhitespace(Peek())) Next();
        }

        public string ScanString() {
            string stringBuffer = "";

            if (Peek() == DOUBLE_QUOTE) Next();
            while (Peek() != DOUBLE_QUOTE) {
                stringBuffer += Peek();
                Next();
            }
            Next();

            return stringBuffer;
        }

        public string ScanIdentifier() {
            string identifierBuffer = "";

            while (!IsEndOfBuffer() && IsIdentifier(Peek())) {
                identifierBuffer += Peek();
                Next();
            }

            return identifierBuffer;
        }

        public string ScanDecimalDigits() {
            string numberBuffer = "";
            while (!IsEndOfBuffer() && IsDecimalDigit(Peek())) {
                numberBuffer += Peek();
                Next();
            }

            return numberBuffer;
        }

        public static bool IsWhitespace(char value) {
            return value == '\n' || value == ' ' || value == '\r' || value == '\t';
        }

        public static bool IsIdentifier(char value) {
            return (value >= 'a' && value <= 'z') || (value >= 'A' && value <= 'Z');
        }

        public static bool IsDecimalDigit(char value) {
            return value >= '0' && value <= '9';
        }
    }
}

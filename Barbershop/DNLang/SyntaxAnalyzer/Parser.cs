using System;
using System.Collections.Generic;
using DNLang.SyntaxAnalyzer.Model;
using DNLang.SyntaxAnalyzer.Token;

namespace DNLang {
    enum DNStates {
        Start,
        ObjectStart, ObjectEnd,
        OpenList, CloseList,
        PropertyStart, PropertyEnd
    }

    sealed class Parser {
        private List<Token> tokenList;
        private int tokenIndex = 0;

        public Parser(List<Token> tokenList) {
            this.tokenList = tokenList;
        }

        public void RaiseUnexpectedError(Token factToken) {
            throw new Exception($"Unexpected token '{factToken.kind}:{factToken.value}'");
        }

        public DataNotationModel Parse() {
            var currentState = DNStates.Start;
            var dnModel = new DataNotationModel();
            var currentList = new DataNotationCollection();
            var currentObject = new DataNotationObject();
            var currentProperty = new DataNotationProperty();

            while (!IsEndOfBuffer()) {
                if (Peek().kind == TokenKind.End) break;

                if (currentState == DNStates.Start) {
                    if (Expect(TokenKind.Identifier)) {
                        currentState = DNStates.ObjectStart;
                        continue;
                    } else break;
                }

                if (currentState == DNStates.ObjectStart) {
                    if (Expect(TokenKind.Identifier) && Expect(TokenKind.Colon, 1)) {
                        currentObject = new DataNotationObject();
                        currentObject.name = Peek().value;
                        currentState = DNStates.OpenList;
                        Skip(2); continue;
                    } else RaiseUnexpectedError(Peek());
                }

                if (currentState == DNStates.OpenList) {
                    if (Expect(TokenKind.OpenBracket)) {
                        currentList = new DataNotationCollection();
                        currentState = DNStates.PropertyStart;
                        Skip(); continue;
                    } else RaiseUnexpectedError(Peek());
                }

                if (currentState == DNStates.PropertyStart) {
                    if (Expect(TokenKind.Identifier) && Expect(TokenKind.Assign, 1)) {
                        currentProperty = new DataNotationProperty();
                        currentProperty.key = Peek().value;
                        Skip(2);
                        if (Expect(TokenKind.Number) || Expect(TokenKind.String)) {
                            currentProperty.value = Peek().value;
                            currentState = DNStates.PropertyEnd;
                            Skip(); continue;
                        } else RaiseUnexpectedError(Peek());
                    } else if (Expect(TokenKind.CloseBracket)) {
                        currentState = DNStates.CloseList;
                    } else RaiseUnexpectedError(Peek());
                }

                if (currentState == DNStates.PropertyEnd) {
                    currentList.AddProperty(currentProperty);
                    currentState = Expect(TokenKind.Comma) ? DNStates.PropertyStart : DNStates.CloseList;
                    if (Expect(TokenKind.Comma)) Skip();
                    continue;
                }

                if (currentState == DNStates.CloseList) {
                    if (Expect(TokenKind.CloseBracket)) {
                        currentObject.AddCollection(currentList);
                        Skip();
                        if (Expect(TokenKind.Semicolon)) {
                            dnModel.objectList.Add(currentObject);
                            currentState = DNStates.Start;
                            Skip(); continue;
                        }

                        if (Expect(TokenKind.Comma)) {
                            currentState = DNStates.OpenList;
                            Skip(); continue;
                        }

                        RaiseUnexpectedError(Peek());
                    } else RaiseUnexpectedError(Peek());
                }

                RaiseUnexpectedError(Peek());
            }

            return dnModel;
        }

        public bool IsEndOfBuffer(int offset = 0) {
            return tokenIndex + offset >= tokenList.Count;
        }

        public bool Expect(TokenKind tokenKind, int offset = 0) {
            return (Peek(offset).kind == tokenKind);
        }

        public bool Skip(int offset = 1) {
            if (IsEndOfBuffer(offset)) return false;

            tokenIndex += offset;
            return true;
        }

        public Token Peek(int offset = 0) {
            if (IsEndOfBuffer(offset)) return new Token(TokenKind.End, null);

            return tokenList[tokenIndex + offset];
        }
    }
}

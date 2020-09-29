using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNLang.SyntaxAnalyzer.Token {
    internal sealed class Token {
        public TokenKind kind;
        public string value;

        public Token(TokenKind kind, string value) {
            this.kind = kind;
            this.value = value;
        }
    }
}

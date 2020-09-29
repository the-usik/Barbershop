namespace DNLang.SyntaxAnalyzer.Token {
    internal enum TokenKind {
        Number, String, Identifier,
        Plus, Assign,
        OpenBracket, CloseBracket,
        Comma, Semicolon, Colon, End
    }
}

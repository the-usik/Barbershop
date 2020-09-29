using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNLang.SyntaxAnalyzer.Model;

namespace DNLang {
    class Deserializer {
        public static DataNotationModel ParseModel(string inputBuffer) {
            var lexer = new Lexer(inputBuffer.ToCharArray());
            var parser = new Parser(lexer.Tokenize());

            return parser.Parse();
        }
    }
}

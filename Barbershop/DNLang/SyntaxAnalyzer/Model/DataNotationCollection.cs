using System.Collections.Generic;

namespace DNLang.SyntaxAnalyzer.Model {
    class DataNotationCollection {
        public List<DataNotationProperty> children { get; set; }

        public DataNotationCollection() {
            children = new List<DataNotationProperty>();
        }

        public bool HasProperties(params string[] args) {
            bool result = false;
            foreach (var arg in args) {
                result = (GetProperty(arg) != null);
            }

            return result;
        }

        public void AddProperty(DataNotationProperty property) => children.Add(property);

        public DataNotationProperty GetProperty(string name) => children.Find(property => property.key == name);
    }
}

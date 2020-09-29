namespace DNLang.SyntaxAnalyzer.Model {
    class DataNotationProperty : Serializable {
        public string key { get; set; }
        public object value { get; set; }

        public int GetIntValue() => (int)value;

        public string GetStringValue() => value.ToString();

        public string Serialize() {
            return $"{key} = {value.ToString()}";
        }
    }
}

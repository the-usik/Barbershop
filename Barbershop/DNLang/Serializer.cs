using System.Collections.Generic;

namespace DNLang {
    class Serializer {
        private string buffer = "";

        public void AddObject(string name, string collections) {
            buffer += $"{name}:\n\t{collections};\n";
        }

        public void Clear() => buffer = "";

        public string GetBuffer() => buffer;

        public static string CreateProperties(Dictionary<string, object> pairs) {
            string propertiesBuffer = "";

            foreach (var data in pairs) {
                string dataValue = "\"\"";

                if (data.Value is string) dataValue = $"\"{data.Value}\"";
                if (data.Value is int) dataValue = data.Value.ToString();

                propertiesBuffer += $"{data.Key} = {dataValue}, ";
            }

            return propertiesBuffer.Substring(0, propertiesBuffer.Length - 2);
        }

        public static string CreateCollection(List<string> properties) {
            string collectionBuffer = "";

            foreach (string property in properties) {
                collectionBuffer += $"[ {property} ],\n\t";
            }

            return collectionBuffer.Substring(0, collectionBuffer.Length - 3);
        }
    }
}

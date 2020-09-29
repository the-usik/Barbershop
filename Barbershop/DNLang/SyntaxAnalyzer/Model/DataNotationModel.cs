using System.Collections.Generic;

namespace DNLang.SyntaxAnalyzer.Model {
    class DataNotationModel {
        public List<DataNotationObject> objectList;

        public DataNotationModel() {
            objectList = new List<DataNotationObject>();
        }

        public void AddObject(DataNotationObject dnObject) {
            objectList.Add(dnObject);
        }

        public DataNotationObject GetNotationObject(string name) {
            return objectList.Find(obj => obj.name == name);
        }
    }
}

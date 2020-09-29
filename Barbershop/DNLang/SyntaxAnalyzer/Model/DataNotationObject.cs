using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNLang.SyntaxAnalyzer.Model {
    class DataNotationObject {
        public string name { get; set; }
        public List<DataNotationCollection> collections { get; set; }

        public DataNotationObject() {
            collections = new List<DataNotationCollection>();
        }

        public void AddCollection(DataNotationCollection dataNotationCollection) {
            collections.Add(dataNotationCollection);
        }

        public void ForEach(Action<DataNotationCollection> collection) {
            foreach (var dnCollection in collections)
                collection(dnCollection);
        }
    }
}

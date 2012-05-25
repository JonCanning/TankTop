using System.Collections.Generic;
using TankTop.Extensions;

namespace TankTop.Dto
{
    public class Document
    {
        public Document(string docId)
        {
            DocId = docId;
        }

        public string DocId { get; set; }
        public IDictionary<string, string> Fields { get; set; }
        public IDictionary<string, string> Categories { get; set; }
        public IDictionary<int, float> Variables { get; set; }
    }

    public class Document<T> : Document where T : class
    {
        public Document(string docId, T obj) : base(docId)
        {
            Fields = obj.ToDictionary();
        }
    } 
}
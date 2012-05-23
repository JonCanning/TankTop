using System.Collections.Generic;

namespace TankTop.Dto
{
    public class Document
    {
        public string DocId { get; set; }
        public IDictionary<string, string> Fields { get; set; }
        public IDictionary<string, string> Categories { get; set; }
        public IDictionary<int, float> Variables { get; set; }
    }
}
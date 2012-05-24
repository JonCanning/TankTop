using System.Collections.Generic;

namespace TankTop.Dto
{
    public class ResultDocument
    {
        public string DocId { get; set; }
        public IDictionary<string, string> Fields { get; set; }
        public IDictionary<string, string> Categories { get; set; }
        public IList<float> Variables { get; set; }
        public float Query_Relevance_Score { get; set; }
        public IDictionary<string, string> Snippets { get; set; }
        public double TimeStamp { get; set; }
    }
}
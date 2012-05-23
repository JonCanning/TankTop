using System.Collections.Generic;

namespace TankTop.Dto
{
    public class ResultDocument : Document
    {
        public float Query_Relevance_Score { get; set; }
        public IDictionary<string, string> Snippets { get; set; }
        public double TimeStamp { get; set; }
    }
}
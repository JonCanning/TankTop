using System.Collections.Generic;

namespace TankTop.IntegrationTests.Documentation
{
    public class StockItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IDictionary<string, string> Categories { get; set; }
    }


}

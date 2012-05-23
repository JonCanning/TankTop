namespace TankTop.Dto
{
    public class Index
    {
        public ITankTopClient TankTopClient { get; set; }
        public string Name { get; set; }
        public bool Started { get; set; }
        public string Code { get; set; }
        public string Creation_Time { get; set; }
        public int Size { get; set; }
        public bool Public_Search { get; set; }
        public string Status { get; set; }
    }
}
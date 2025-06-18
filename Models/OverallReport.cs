namespace SqlServerApi.Models
{
    public class OverallReport
    {
        public string customer { get; set; }
        public string cusDc_No { get; set; }
        public string date { get; set; }
        public string shade { get; set; }
        public string process { get; set; }
        public string batch_No { get; set; }
        public int age { get; set; }
        public decimal qty { get; set; }
        public string stage { get; set; }
    }
}

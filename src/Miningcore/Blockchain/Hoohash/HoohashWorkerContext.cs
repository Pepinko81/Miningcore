public class HoohashWorkerContext : WorkerContextBase
{
    public string WorkerAddress { get; set; }
    public double Difficulty { get; set; }
    public string ExtraNonce { get; set; }
    public Dictionary<string, string> WorkerParameters { get; set; }
    public DateTime LastActivity { get; set; }
    public bool IsAuthorized { get; set; }
}

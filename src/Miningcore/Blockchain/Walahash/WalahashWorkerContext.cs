public class WalahashWorkerContext : WorkerContextBase
{
    public string WorkerAddress { get; set; }
    public double Difficulty { get; set; }
    public string ExtraNonce { get; set; }
    public Dictionary<string, string> WorkerParameters { get; set; }
}

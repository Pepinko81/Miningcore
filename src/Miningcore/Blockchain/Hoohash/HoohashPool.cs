public class HoohashPool : PoolBase
{
    public HoohashPool(IComponentContext ctx) : base(ctx)
    {
    }

    protected override async Task SetupJobManager()
    {
        manager = ctx.Resolve<HoohashJobManager>();
        manager.Configure(poolConfig, clusterConfig);
    }

    protected override WorkerContextBase CreateWorkerContext()
    {
        return new HoohashWorkerContext();
    }
}

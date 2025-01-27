public class RavencashPool : PoolBase
{
    public RavencashPool(IComponentContext ctx) : base(ctx)
    {
    }

    protected override async Task SetupJobManager()
    {
        manager = ctx.Resolve<RavencashJobManager>();
        manager.Configure(poolConfig, clusterConfig);
    }

    protected override WorkerContextBase CreateWorkerContext()
    {
        return new RavencashWorkerContext();
    }
}

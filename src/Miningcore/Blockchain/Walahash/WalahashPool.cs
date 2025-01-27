using System.Reactive;
using Autofac;
using Miningcore.Mining;
using Miningcore.Stratum;

public class WalahashPool : PoolBase
{
    public WalahashPool(IComponentContext ctx) : base(ctx)
    {
    }

    protected override async Task SetupJobManager()
    {
        manager = ctx.Resolve<WalahashJobManager>();
        manager.Configure(poolConfig, clusterConfig);
    }

    protected override WorkerContextBase CreateWorkerContext()
    {
        return new WalahashWorkerContext();
    }
}

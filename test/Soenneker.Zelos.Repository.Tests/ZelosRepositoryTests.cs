using Soenneker.Zelos.Repository.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Zelos.Repository.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class ZelosRepositoryTests : HostedUnitTest
{
    public ZelosRepositoryTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}

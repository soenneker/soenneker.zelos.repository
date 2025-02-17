using Soenneker.Zelos.Repository.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Zelos.Repository.Tests;

[Collection("Collection")]
public class ZelosRepositoryTests : FixturedUnitTest
{
    public ZelosRepositoryTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public void Default()
    {

    }
}

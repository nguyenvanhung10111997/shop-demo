using MediatR;
using NSubstitute;

namespace example.product.api.tests;

public class TestFixtureBase
{
    public IMediator Mediator { get; set; }

    public TestFixtureBase()
    {
        Mediator = Substitute.For<IMediator>();
    }
}
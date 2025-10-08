using MediatR;
using NSubstitute;

namespace example.order.api.tests;

public class TestFixtureBase
{
    public IMediator Mediator { get; set; }

    public TestFixtureBase()
    {
        Mediator = Substitute.For<IMediator>();
    }
}
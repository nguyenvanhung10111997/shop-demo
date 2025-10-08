using example.order.service.Features.ProductCategories.QueryHandlers;
using example.product.api.Controllers;
using example.product.domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace example.product.api.tests;

[TestFixture]
public class ProductCategoriesControllerTests : TestFixtureBase
{
    private ProductCategoriesController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _controller = new ProductCategoriesController(Mediator);
    }

    [Test]
    public async Task GetAllOrderStatus_Success()
    {
        var orderStatuses = new List<ProductCategory>
        {
            new ProductCategory
            {
                CategoryName = "Electronics",
                Description = "Electronic gadgets and devices",
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSetup"
            },
            new ProductCategory
            {
                CategoryName = "Books",
                Description = "Various kinds of books",
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSetup"
            }
        };

        Mediator.Send(Arg.Any<GetAllProductCategoryQuery>(), Arg.Any<CancellationToken>()).Returns(orderStatuses);

        var response = await _controller.Get();

        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();

        var dataResponse = (response as OkObjectResult)?.Value as IEnumerable<ProductCategory>;
        dataResponse.Should().NotBeNull();
        dataResponse.Count().Should().Be(2);
    }
}
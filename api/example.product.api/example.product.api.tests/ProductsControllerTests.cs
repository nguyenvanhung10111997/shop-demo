using example.order.infrastructure.Models;
using example.product.api.Controllers;
using example.product.domain.Entities;
using example.service.Features;
using FluentAssertions;
using MassTransit.Transports;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace example.product.api.tests;

[TestFixture]
public class ProductsControllerTests : TestFixtureBase
{
    private ProductsController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _controller = new ProductsController(Mediator);
    }


    [Test]
    public async Task Search_Success()
    {
        var orderSearchData = new PagingResult<Product>
        {
            TotalRecords = 2,
            Records = new List<Product>
            {
               new Product
                {
                    ProductCode = "P001",
                    ProductName = "Laptop",
                    CategoryId = 1,
                    Price = 1200.00m,
                    Rating = 4.5f,
                    ImageUrl = "http://example.com/laptop.jpg",
                    Stock = 10,
                    Description = "High performance laptop"
                },
                new Product
                {
                    ProductCode = "P002",
                    ProductName = "Smartphone",
                    CategoryId = 1,
                    Price = 800.00m,
                    Rating = 4.7f,
                    ImageUrl = "http://example.com/smartphone.jpg",
                    Stock = 20,
                    Description = "Latest model smartphone"
                }
            }
        };

        Mediator.Send(Arg.Any<SearchProductQuery>(), Arg.Any<CancellationToken>()).Returns(orderSearchData);

        var request = new SearchProductQuery
        {
            PageNumber = 0,
            PageSize = 10
        };
        var response = await _controller.Search(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();

        var dataResponse = (response as OkObjectResult)?.Value as PagingResult<Product>;
        dataResponse.Should().NotBeNull();
        dataResponse.TotalRecords.Should().Be(2);
        dataResponse.Records.Count().Should().Be(2);
    }
}
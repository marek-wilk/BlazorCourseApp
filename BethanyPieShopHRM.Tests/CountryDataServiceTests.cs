using BethanyPieShopHRM.App.Services;
using BethanyPieShopHRM.Shared;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BethanyPieShopHRM.Tests
{
    public class CountryDataServiceTests
    {
        private ICountryDataService _sut;
        private HttpClient _httpClient;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri("https://localhost:44340/") };
            _sut = new CountryDataService(_httpClient);
        }

        [Test]
        public async Task Should_call_send_async_with_proper_api_address_when_get_all_countries_is_called()
        {
            // Given
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new List<Country>()), Encoding.UTF8, "application/json")
                }).Verifiable();
            var targetUri = _httpClient.BaseAddress + "api/country";

            // When
            await _sut.GetAllCountries();

            // Then
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri.ToString() == targetUri),
            ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task Should_return_list_of_countries_when_get_all_countries_is_called()
        {
            // Given
            var allCountries = new List<Country>
            {
                new Country(),
                new Country(),
                new Country()
            };
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(allCountries), Encoding.UTF8,
                    "application/json")
            });

            // When
            var result = await _sut.GetAllCountries();

            // Then
            result.Should().BeOfType(typeof(List<Country>));
            result.Count().Should().Be(3);
        }

        [Test]
        public async Task Should_call_send_async_with_proper_id_in_api_address_when_get_country_by_id_is_called()
        {
            // Given
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new Country()), Encoding.UTF8, "application/json")
            }).Verifiable();
            var countryId = 123;
            var targetUri = _httpClient.BaseAddress + $"api/country/{countryId}";

            // When
            await _sut.GetCountryById(countryId);

            // Then
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri.ToString() == targetUri),
                ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task Should_return_country_with_proper_id_when_get_country_by_id_is_called()
        {
            // Given
            var countryId = 1234;
            var country = new Country {CountryId = countryId};
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(country), Encoding.UTF8,
                    "application/json")
            });

            // When
            var result = await _sut.GetCountryById(countryId);

            // Then
            result.Should().BeOfType(typeof(Country));
            result.CountryId.Should().Be(countryId);
        }
    }
}
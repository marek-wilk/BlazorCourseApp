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
    public class JobCategoryDataServiceTests
    {
        private IJobCategoryDataService _sut;
        private HttpClient _httpClient;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri("https://localhost:44340/") };
            _sut = new JobCategoryDataService(_httpClient);
        }

        [Test]
        public async Task Should_call_send_async_with_proper_api_address_when_get_all_job_categories_is_called()
        {
            // Given
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new List<JobCategory>()), Encoding.UTF8, "application/json")
            }).Verifiable();
            var targetUri = _httpClient.BaseAddress + "api/jobcategory";

            // When
            await _sut.GetAllJobCategories();

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
        public async Task Should_return_all_job_categories_when_get_all_job_categories_is_called()
        {
            // Given
            var allJobCategories = new List<JobCategory>
            {
                new JobCategory(),
                new JobCategory(),
                new JobCategory()
            };
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(allJobCategories), Encoding.UTF8, "application/json")
            }).Verifiable();
            var targetUri = _httpClient.BaseAddress + "api/jobcategory";

            // When
            var result = await _sut.GetAllJobCategories();

            // Then
            result.Should().BeOfType(typeof(List<JobCategory>));
            result.Count().Should().Be(3);
        }

        [Test]
        public async Task Should_call_send_async_with_proper_api_address_when_get_job_category_by_id_is_called()
        {
            // Given
            var jobCategoryId = 123;
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new JobCategory()), Encoding.UTF8, "application/json")
            }).Verifiable();
            var targetUri = _httpClient.BaseAddress + $"api/jobcategory/{jobCategoryId}";

            // When
            await _sut.GetJobCategoryById(jobCategoryId);

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
        public async Task Should_return_job_category_with_proper_id_when_get_job_category_by_id_is_called()
        {
            // Given
            var jobCategoryId = 123;
            var jobCategory = new JobCategory {JobCategoryId = jobCategoryId};
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(jobCategory), Encoding.UTF8, "application/json")
            }).Verifiable();
            var targetUri = _httpClient.BaseAddress + $"api/jobcategory/{jobCategoryId}";

            // When
            var result = await _sut.GetJobCategoryById(jobCategoryId);

            // Then
            result.Should().BeOfType(typeof(JobCategory));
            result.JobCategoryId.Should().Be(jobCategoryId);
        }
    }
}

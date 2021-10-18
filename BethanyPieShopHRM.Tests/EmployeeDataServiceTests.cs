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
    public class EmployeeDataServiceTests
    {
        private IEmployeeDataService _sut;
        private HttpClient _httpClient;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri("https://localhost:44340/") };
            _sut = new EmployeeDataService(_httpClient);
        }

        [Test]
        public async Task Should_call_send_async_with_proper_api_address_when_get_all_employees_is_called()
        {
            // Given
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new List<Employee>()), Encoding.UTF8, "application/json")
                }).Verifiable();
            var targetUri = _httpClient.BaseAddress + "api/employee";

            // When
            await _sut.GetAllEmployees();

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
        public async Task Should_return_list_of_all_employees_when_get_all_employees_is_called()
        {
            // Given
            var allEmployees = new List<Employee>
            {
                new Employee(),
                new Employee(),
                new Employee()
            };
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(allEmployees), Encoding.UTF8, "application/json")
            });

            // When
            var result = await _sut.GetAllEmployees();

            // Then
            result.Should().BeOfType(typeof(List<Employee>));
            result.Count().Should().Be(3);
        }

        [Test]
        public async Task Should_call_send_async_with_proper_api_address_when_get_employee_details_is_called()
        {
            // Given
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new Employee()), Encoding.UTF8, "application/json")
            }).Verifiable();
            var employeeId = 123;
            var targetUri = _httpClient.BaseAddress + $"api/employee/{employeeId}";

            // When
            await _sut.GetEmployeeDetails(employeeId);

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
        public async Task Should_return_employee_with_proper_id_when_get_employee_details_is_called()
        {
            // Given
            var employeeId = 123;
            var employee = new Employee {EmployeeId = employeeId};
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json")
            });

            // When
            var result = await _sut.GetEmployeeDetails(employeeId);

            // Then
            result.Should().BeOfType(typeof(Employee));
            result.EmployeeId.Should().Be(employeeId);
        }

        [Test]
        public async Task Should_call_send_async_with_proper_api_address_when_add_employee_is_called()
        {
            // Given
            var employee = new Employee();
            var httpMessageContent =
                new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = httpMessageContent
            }).Verifiable();
            var targetUri = _httpClient.BaseAddress + "api/employee";

            // When
            await _sut.AddEmployee(employee);

            // Then
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri.ToString() == targetUri),
                ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task Should_return_employee_when_add_employee_is_called_and_response_message_status_code_is_200()
        {
            // Given
            var employee = new Employee();
            var httpMessageContent =
                new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = httpMessageContent
            });

            // When
            var result = await _sut.AddEmployee(employee);

            // Then
            result.Should().BeEquivalentTo(employee);
        }

        [Test]
        public async Task Should_return_null_when_add_employee_is_called_and_response_message_status_code_is_not_200()
        {
            // Given
            var employee = new Employee(); 
            var httpMessageContent =
                new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = httpMessageContent
            });

            // When
            var result = await _sut.AddEmployee(employee);

            // Then
            result.Should().Be(null);
        }

        [Test]
        public async Task Should_call_send_async_with_proper_api_address_when_update_employee_is_called()
        {
            // Given
            var employee = new Employee();
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            }).Verifiable();
            var targetUri = _httpClient.BaseAddress + "api/employee";

            // When
            await _sut.UpdateEmployee(employee);

            // Then
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put
                    && req.RequestUri.ToString() == targetUri),
                ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task Should_call_send_async_with_proper_api_address_when_delete_employee_is_called()
        {
            // Given
            var employeeId = 1234;
             _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            }).Verifiable();
            var targetUri = _httpClient.BaseAddress + $"api/employee/{employeeId}";

            // When
            await _sut.DeleteEmployee(employeeId);

            // Then
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete 
                    && req.RequestUri.ToString() == targetUri),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}


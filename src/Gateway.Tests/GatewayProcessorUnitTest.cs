using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Gateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using Moq.Protected;
using Xunit;

namespace Gateway.Tests
{
    public class GatewayProcessorUnitTest
    {
        private readonly Mock<IAuthorizationMiddleWare> _authorizationMiddlewareMock;
        private readonly  Mock<HttpMessageHandler> _handlerMock;
        private HttpClient _httpClient;
        private IGatewayProcessor _gatewayProcessor;
        private IDictionary<string, string> _claims;
        private HttpRequest _incomingRequest;
        private HttpRequestMessage _downstreamRequest;

        public GatewayProcessorUnitTest()
        {
            _authorizationMiddlewareMock = new Mock<IAuthorizationMiddleWare>();
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        }

        [Fact]
        public async Task ProcessAsync_Should_Return_UnAuthorized_When_Not_Authorized()
        {
            // arrange
            RequestSetup("get");
            AuthorizationProcessorSetup(false);
            GatewaySetup();

            // act
            var response = await _gatewayProcessor.ProcessAsync(_incomingRequest, new[] { "any1" });

            // assert
            response.StatusCode.Should().Be(401);
        }

        [Fact]
        public async Task ProcessAsync_Should_Return_Downstream_StatusCode()
        {
            // arrange
            RequestSetup("get");
            ClaimsSetup();
            AuthorizationProcessorSetup(true);
            HttpClientSetup(HttpStatusCode.Accepted, "[{'id':1,'value':'1'}]", "http://test.com/");
            GatewaySetup();

            // act
            var response = await _gatewayProcessor.ProcessAsync(_incomingRequest, new[] { "any1" });

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }


        private void GatewaySetup()
        {
            _gatewayProcessor = new GatewayProcessor(_authorizationMiddlewareMock.Object,
                _httpClient);
        }

        private void HttpClientSetup(HttpStatusCode code, string content, string baseAddress)
        {
            _handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = code,
                    Content = new StringContent(content)
                })
                .Callback<HttpRequestMessage, CancellationToken>((m, c) => _downstreamRequest = m)
                .Verifiable();

            // use real http client with mocked handler here
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri(baseAddress),
            };
        }

        private void AuthorizationProcessorSetup(bool result)
        {
            _authorizationMiddlewareMock
                .Setup(x => x.ProcessAsync(It.IsAny<HttpRequest>(), It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(result);
        }

        private void RequestSetup(string method)
        {
            _incomingRequest = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Method = method
            };
        }

        private void ClaimsSetup()
        {
            _claims = new Dictionary<string, string>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Gateway.Application.Pipelines;
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
    public class CorrelationIdMiddlewareUnitTest
    {
        private readonly Context _context;
        public CorrelationIdMiddlewareUnitTest()
        {
            
        }
    }
}

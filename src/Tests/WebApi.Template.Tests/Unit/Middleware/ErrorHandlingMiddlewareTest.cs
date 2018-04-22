using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eshopworld.Core;
using Eshopworld.Tests.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using WebApi.Template.Infrastructure.Middleware;
using WebApi.Template.Model.Exceptions;
using Xunit;

namespace WebApi.Template.Tests.Unit.Middleware
{
    public class ErrorHandlingMiddlewareTest
    {
        [Fact, IsUnit]
        public void Test_ConstructorThrows_WithNullParameter()
        {
            var methodFailed = false;
            try
            {
                new ErrorHandlingMiddleware(null, null);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Contain(nameof(IBigBrother));
                methodFailed = true;
            }

            methodFailed.Should().BeTrue();
        }

        public class  Invoke
        {
            [Fact, IsUnit]
            public async Task Test_GenericExceptionAreHandled()
            {
                //Arrange
                var mockMiddleware = new Mock<ErrorHandlingMiddleware>(MockBehavior.Loose,
                    new RequestDelegate(_ => throw new Exception("Test Exp")), new Mock<IBigBrother>().Object);
                mockMiddleware.Setup(x => x.Invoke(It.IsAny<HttpContext>())).CallBase();
                mockMiddleware.Setup(x => x.HandleExceptionAsync(It.IsAny<HttpContext>(), It.IsAny<Exception>()))
                              .Returns(Task.CompletedTask);
                mockMiddleware.Setup(x => x.SendResponse(It.IsAny<HttpContext>(), It.IsAny<BaseException>()))
                              .Returns(Task.CompletedTask);

                //Act
                await mockMiddleware.Object.Invoke(new Mock<HttpContext>().Object);

                //Assert
                mockMiddleware.Verify(x => x.HandleExceptionAsync(It.IsAny<HttpContext>(), It.IsAny<Exception>()),
                    Times.Once);
            }

            [Fact, IsUnit]
            public async Task Test_GenericAggregateExceptionAreHandled()
            {
                var exceptionList = new List<Exception> {new Exception("Test exp 1"), new Exception("test Exp 2")};
                //Arrange
                var mockMiddleware = new Mock<ErrorHandlingMiddleware>(MockBehavior.Loose,
                    new RequestDelegate(_ => throw new AggregateException("My Aggregate Exp", exceptionList)), new Mock<IBigBrother>().Object);
                mockMiddleware.Setup(x => x.Invoke(It.IsAny<HttpContext>())).CallBase();
                mockMiddleware.Setup(x => x.HandleExceptionAsync(It.IsAny<HttpContext>(), It.IsAny<Exception>()))
                              .CallBase();
                mockMiddleware.Setup(x => x.SendResponse(It.IsAny<HttpContext>(), It.IsAny<BaseException>()))
                              .Returns(Task.CompletedTask);

                //Act
                await mockMiddleware.Object.Invoke(new Mock<HttpContext>().Object);

                //Assert
                mockMiddleware.Verify(x => x.HandleExceptionAsync(It.IsAny<HttpContext>(), It.IsAny<Exception>()),
                    Times.Once);
            }

            [Fact, IsUnit]
            public async Task Test_ValidationExceptionAreHandled()
            {
                //Arrange
                var mockMiddleware = new Mock<ErrorHandlingMiddleware>(MockBehavior.Loose,
                    new RequestDelegate(_ => throw new ValidationException("Test Exp")), new Mock<IBigBrother>().Object);
                mockMiddleware.Setup(x => x.Invoke(It.IsAny<HttpContext>())).CallBase();
                mockMiddleware.Setup(x => x.HandleValidationExceptionAsync(It.IsAny<HttpContext>(), It.IsAny<BaseException>()))
                              .Returns(Task.CompletedTask);
                mockMiddleware.Setup(x => x.SendResponse(It.IsAny<HttpContext>(), It.IsAny<BaseException>()))
                              .Returns(Task.CompletedTask);

                //Act
                await mockMiddleware.Object.Invoke(new Mock<HttpContext>().Object);

                //Assert
                mockMiddleware.Verify(x => x.HandleValidationExceptionAsync(It.IsAny<HttpContext>(), It.IsAny<BaseException>()),
                    Times.Once);
            }
        }
    }
}

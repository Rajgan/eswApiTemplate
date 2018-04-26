using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eshopworld.Tests.Core;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Moq;
using WebApi.Template.Controllers;
using WebApi.Template.Model;
using WebApi.Template.Model.Common;
using WebApi.Template.Model.DTO;
using WebApi.Template.Validations;
using Xunit;

namespace WebApi.Template.Tests.Unit.Controller
{
    public class ValuesControllerTest : BaseTest
    {
        public class ValuesGet
        {
            [Fact,IsUnit]
            public async Task Get_All()
            {
                using (var cnx = CreateInMemoryContext("Values_Get"))
                {
                    var mockValidator = new Mock<IValidator<SampleRequest>>();

                    var response = new ValuesController(cnx, mockValidator.Object).Get() as ObjectResult;

                    response.Should().NotBeNull();
                    var result = Assert.IsType<List<string>>(response.Value);
                    result.Should().HaveCountGreaterThan(0);
                }
            }
            
        }

        public class ValuesPost
        {
            /// <summary>
            /// This Example will tell you how to Mock Fluent Validation which returns True
            /// </summary>
            /// <returns></returns>
            [Fact,IsUnit]
            public async Task Post_PostiveCondition()
            {
                var mockValidator = new Mock<IValidator<SampleRequest>>();

                mockValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new ValidationResult());

                using (var cnx = CreateInMemoryContext("Values_Post"))
                {
                    var response = await new ValuesController(cnx, mockValidator.Object).Post(new SampleRequest() { ClientCode = "1", Prefix = " Test", ClientNo = 11 }) as OkObjectResult;

                    var output = response.Value as SampleResponse;

                    output.Should().NotBeNull();
                    output.Prefix.Should().Be("Test Prefix");

                }
            }

            /// <summary>
            /// This Example will tell you how to Mock Fluent Validation which returns validation Errors
            /// </summary>
            /// <returns></returns>
            [Fact, IsUnit]
            public async Task Post_TestForValidationError()
            {
                var mockValidator = new Mock<IValidator<SampleRequest>>();

                mockValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new ValidationResult(new List<ValidationFailure>()
                             {
                                 new ValidationFailure("TestField","Test Message"){ErrorCode = "1001"}
                             }));

                using (var cnx = CreateInMemoryContext("Values_Post"))
                {
                    // ********** Setting Call Base to True is important. ******************
                    var mockController = new Mock<ValuesController>(cnx, mockValidator.Object) {CallBase = true};
                    // Mock HttpContext Operation Id
                    mockController.Setup(x => x.AiOperationId).Returns(Guid.NewGuid().ToString);

                    var response = await mockController.Object.Post(new SampleRequest() { ClientCode = "1", Prefix = " Test", ClientNo = 11 });

                    var output = response.As<BadRequestObjectResult>().Value.As<ApiErrorResponse>();

                    output.Should().NotBeNull();
                    output.ErrorType.Should().Be((int)ErrorTypeCode.ValidationError);

                }
            }
            /// <summary>
            /// This Example will tell you how to Mock you controller and overide Virtual Methods
            /// </summary>
            /// <returns></returns>
            [Fact, IsUnit]
            public async Task Post_ThrowsError()
            {
                using (var cnx = CreateInMemoryContext("Values_Post"))
                {
                    // Here I am intented to use my real validation.
                    var mockController =
                        new Mock<ValuesController>(cnx, new SampleRequestValidator()) {CallBase = true};

                    mockController.Setup(x => x.AiOperationId).Returns(Guid.NewGuid().ToString);

                    var response = await mockController.Object.Post(new SampleRequest() { ClientCode = "1", Prefix = " Test", ClientNo = 21 });

                    var output = response.As<BadRequestObjectResult>().Value.As<ApiErrorResponse>(); ;

                    output.Should().NotBeNull();
                    output.ErrorType.Should().Be((int)ErrorTypeCode.BusinessError);

                }
            }
        }

        public class ValuesPut
        {
            [Fact,IsUnit]
            public async Task Put_PostiveConditionAsync()
            {
                using (var cnx = CreateInMemoryContext("Values_Put"))
                {
                  //  await Assert.ThrowsAsync<NotImplementedException>(() => new ValuesController(cnx).Put(1,new SampleRequest()));
                }
            }
        }
    }
}

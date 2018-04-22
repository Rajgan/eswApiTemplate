using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eshopworld.Tests.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Template.Controllers;
using WebApi.Template.Model.DTO;
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
                    var response = new ValuesController(cnx).Get() as ObjectResult;

                    response.Should().NotBeNull();
                    var result = Assert.IsType<List<string>>(response.Value);
                    result.Should().HaveCountGreaterThan(0);
                }
            }
            
        }

        public class ValuesPost
        {
            [Fact,IsUnit]
            public async Task Post_PostiveCondition()
            {
                using (var cnx = CreateInMemoryContext("Values_Post"))
                {
                    var response = await new ValuesController(cnx).Post(new SampleRequest(){ClientCode = "1",Prefix = " Test"}) as OkObjectResult;

                    var output = response.Value as SampleResponse;

                    output.Should().NotBeNull();
                    output.Prefix.Should().Be("Test Prefix");
                    
                }
            }

            [Fact, IsUnit]
            public async Task Post_ThrowsError()
            {
                using (var cnx = CreateInMemoryContext("Values_Post"))
                {
                    await Assert.ThrowsAsync<Exception>(() => new ValuesController(cnx).Post(new SampleRequest() { ClientCode = "0", Prefix = " Test" }));

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
                    await Assert.ThrowsAsync<NotImplementedException>(() => new ValuesController(cnx).Put(1,new SampleRequest()));
                }
            }
        }
    }
}

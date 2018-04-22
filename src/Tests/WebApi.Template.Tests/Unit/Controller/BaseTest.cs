using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApi.Template.DataAccess;
using WebApi.Template.Model.Domain;

namespace WebApi.Template.Tests.Unit.Controller
{
    public class BaseTest
    {
        internal static TemplateDbContext CreateInMemoryContext(string name = "default")
        {
            var optionsBuilder = new DbContextOptionsBuilder<TemplateDbContext>();
            optionsBuilder.UseInMemoryDatabase(name);
            var context = new TemplateDbContext(optionsBuilder.Options);
            
            context.NameAddresses.AddRange(CreateNameAddresseses());

            context.SaveChanges();

            return context;
        }

        private static List<NameAddresses> CreateNameAddresseses()
        {
            return new List<NameAddresses>()
            {
                new NameAddresses(){LastUpdateTime = DateTime.UtcNow,LastUpdateUser = "SC-156 MAD-OSL:AS"},
                new NameAddresses(){LastUpdateTime = DateTime.UtcNow,LastUpdateUser = "PLAT 2115 STG Routes: PMC"},
            };
        }
    }
}

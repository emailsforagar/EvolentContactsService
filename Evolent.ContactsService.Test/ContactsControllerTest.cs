using System;
using Xunit;
using Moq;
using Evolent.ContactsService.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Evolent.ContactsService.Controllers;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Evolent.ContactsService.Test
{
    public class ContactsControllerTest
    {
        [Fact]
        public async Task GetContactsTest()
        {
            var dbcontextoptions = new DbContextOptionsBuilder<dbsegapracdevContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var logger = new Mock<ILogger<ContactsController>>();

            using (var contactsContext = new dbsegapracdevContext(dbcontextoptions))
            {
                var newData = new Contacts()
                {
                    ContactId = 1,
                    FirstName = "Sagar",
                    LastName = "Vairagade",
                    Email = "Sagar.Vairagade@outlook.com",
                    Phonenumber = "123456789",
                    Status = "Active"
                };

                var sut = new ContactsController(contactsContext, logger.Object);
                var data = await sut.PostContacts(newData);

                var result = await sut.GetContacts();
                Assert.NotNull(result);

            }

        }
        [Fact]
        public async Task PostContactsTest()
        {
            var dbcontextoptions = new DbContextOptionsBuilder<dbsegapracdevContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var logger = new Mock<ILogger<ContactsController>>();

            using (var contactsContext = new dbsegapracdevContext(dbcontextoptions))
            {
                var newData = new Contacts()
                {
                    ContactId = 1,
                    FirstName = "Sagar",
                    LastName = "Vairagade",
                    Email = "Sagar.Vairagade@outlook.com",
                    Phonenumber = "123456789",
                    Status = "Active"
                };

                var sut = new ContactsController(contactsContext, logger.Object);
                var data = await sut.PostContacts(newData);
                var result = await sut.GetContacts();
                Assert.Equal(1, result.Value.Count());
            }
        }
        [Fact]
        public async Task PutContactsTest()
        {
            var dbcontextoptions = new DbContextOptionsBuilder<dbsegapracdevContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var logger = new Mock<ILogger<ContactsController>>();

            using (var contactsContext = new dbsegapracdevContext(dbcontextoptions))
            {
                var newData = new Contacts()
                {
                    ContactId = 1,
                    FirstName = "Sagar",
                    LastName = "Vairagade",
                    Email = "Sagar.Vairagade@outlook.com",
                    Phonenumber = "123456789",
                    Status = "Active"
                };

                var sut = new ContactsController(contactsContext, logger.Object);
                var data = await sut.PostContacts(newData);
                newData.Status = "Inactive";
                var dataupdate = await sut.PutContacts(newData.ContactId,newData);
                var getdata = await sut.GetContacts();
                var result = getdata.Value.ToArray();
                Assert.Equal("Inactive", result[0].Status);
            }
        }
        [Fact]
        public async Task DeleteContacts()
        {
            var dbcontextoptions = new DbContextOptionsBuilder<dbsegapracdevContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var logger = new Mock<ILogger<ContactsController>>();

            using (var contactsContext = new dbsegapracdevContext(dbcontextoptions))
            {
                var newData = new Contacts()
                {
                    ContactId = 1,
                    FirstName = "Sagar",
                    LastName = "Vairagade",
                    Email = "Sagar.Vairagade@outlook.com",
                    Phonenumber = "123456789",
                    Status = "Active"
                };

                var sut = new ContactsController(contactsContext, logger.Object);
                var data = await sut.PostContacts(newData);
                var dataupdate = await sut.DeleteContacts(newData.ContactId);
                var result = await sut.GetContacts();
                Assert.Equal(0, result.Value.Count());
            }
        }
    }
}

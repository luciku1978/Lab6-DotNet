using Lab6.Models;
using Lab6.Services;
using Lab6.Viewmodels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4ApiTests
{
    class UserUserRoleServiceTests
    {

        [Test]
        public void GetByIdShouldReturnUserRole()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnUserRole))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var userUserRoleService = new UserUserRoleService(null, context);

                User userToAdd = new User
                {
                    FirstName = "Ana",
                    LastName = "Marcus",
                    Username = "amarcus",
                    Email = "ana@yahoo.com",
                    Password = "1234567",
                    CreatedAt = DateTime.Now,
                    //Expenses = new List<Expense>(),
                    UserUserRoles = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Newbegginer",
                    Description = "A role for a new guy..."
                };
                context.UserRoles.Add(addUserRole);
                context.SaveChanges();

                context.UserUserRoles.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

                var userUserRoleGet = userUserRoleService.GetById(1);
                Assert.IsNotNull(userUserRoleGet.FirstOrDefaultAsync(uurole => uurole.EndTime == null));

            }
        }

        [Test]
        public void CreateShouldAddAnUserUserRole()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAnUserUserRole))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var userUserRoleService = new UserUserRoleService(null, context);

                User userToAdd = new User
                {
                    FirstName = "Ana",
                    LastName = "Marcus",
                    Username = "amarcus",
                    Email = "ana@yahoo.com",
                    Password = "1234567",
                    CreatedAt = DateTime.Now,
                    //Expenses = new List<Expense>(),
                    UserUserRoles = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Newbegginer",
                    Description = "A role for a new guy..."
                };
                context.UserRoles.Add(addUserRole);
                context.SaveChanges();

                context.UserUserRoles.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

              
                Assert.NotNull(userToAdd);
                Assert.AreEqual("Newbegginer", addUserRole.Name);
                Assert.AreEqual("Ana", userToAdd.FirstName);
            }
        }


        [Test]
        public void GetUserRoleNameByIdShouldReturnUserRoleName()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetUserRoleNameByIdShouldReturnUserRoleName))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var userUserRoleService = new UserUserRoleService(null, context);

                User userToAdd = new User
                {
                    FirstName = "Ana",
                    LastName = "Marcus",
                    Username = "amarcus",
                    Email = "ana@yahoo.com",
                    Password = "1234567",
                    CreatedAt = DateTime.Now,
                    //Expenses = new List<Expense>(),
                    UserUserRoles = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Newbegginer",
                    Description = "A role for a new guy..."
                };
                context.UserRoles.Add(addUserRole);
                context.SaveChanges();

                context.UserUserRoles.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

                string userRoleName = userUserRoleService.GetUserRoleNameById(userToAdd.Id);
                Assert.AreEqual("Newbegginer", userRoleName);
                Assert.AreEqual("Ana", userToAdd.FirstName);

            }
        }

    }

}


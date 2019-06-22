using Lab6.Models;
using Lab6.Services;
using Lab6.Validators;
using Lab6.Viewmodels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Linq;

namespace Lab4ApiTests
{
    public class UsersServiceTests
    {
        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING"
            });
        }

        //[Test]
        //public void ValidRegisterShouldCreateANewUser()
        //{
        //    var options = new DbContextOptionsBuilder<ExpensesDbContext>()
        //      .UseInMemoryDatabase(databaseName:nameof(ValidRegisterShouldCreateANewUser))// "ValidRegisterShouldCreateANewUser")
        //      .Options;

        //    using (var context = new ExpensesDbContext(options))
        //    {
        //        var usersService = new UsersService(context, config);
        //        var added = new LabII.DTOs.RegisterPostDTO

        //        {
        //            Email = "petre@aol.com",
        //            FirstName = "Petre",
        //            LastName = "Popescu",
        //            Password = "123456",
        //            Username = "ppetre",
        //        };
        //        var result = usersService.Register(added);

        //        Assert.IsNotNull(result);
        //        Assert.AreEqual(added.Username, result.Username);

        //    }
        //}

        [Test]
        public void ValidRegistrationShouldCreateANewUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegistrationShouldCreateANewUser))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {

                var regValidator = new RegisterValidator();
                var crValidator = new CreateValidator();
                var usersService = new UsersService(context, regValidator, crValidator, null, config);
                var added = new RegisterPostModel
                {
                    FirstName = "Catalin",
                    LastName = "Albulescu",
                    Username = "albuc",
                    Email = "ac@aol.com",
                    Password = "12345678",

                };
                var result = usersService.Register(added);

                Assert.IsNull(result);
                Assert.AreEqual(added.Username, context.Users.FirstOrDefault(u => u.Id == 1).Username);
                //Assert.AreEqual(1, context.UserUserRoles.FirstOrDefault(uur => uur.Id == 1).UserId);
            }
        }

        [Test]
        public void InvalidRegisterShouldReturnErrorsCollection()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
                         .UseInMemoryDatabase(databaseName: nameof(InvalidRegisterShouldReturnErrorsCollection))
                         .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var validator = new RegisterValidator();
                var crValidator = new CreateValidator();
                var usersService = new UsersService(context, validator,crValidator, null, config);
                var added = new Lab6.Viewmodels.RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "lastName1",
                    Username = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111"    //invalid password should invalidate register
                };

                var result = usersService.Register(added);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.ErrorMessages.Count());
            }
        }

        //[Test]
        //public void AuthenticateShouldLoginSuccessfullyTheUser()
        //{

        //    var options = new DbContextOptionsBuilder<ExpensesDbContext>()
        //      .UseInMemoryDatabase(databaseName:nameof(AuthenticateShouldLoginSuccessfullyTheUser))// "ValidUsernameAndPasswordShouldLoginSuccessfully")
        //      .Options;

        //    using (var context = new ExpensesDbContext(options))
        //    {
        //        var usersService = new UsersService(context, config);

        //    using (var context = new ExpensesDbContext(options))
        //    {
        //        var usersService = new UsersService(context,null, config);

        //        var added = new Lab6.Viewmodels.RegisterPostModel

        //        {
        //            Email = "petre@aol.com",
        //            FirstName = "Petre",
        //            LastName = "Popica",
        //            Password = "123456",
        //            Username = "ppetre",
        //        };
        //        usersService.Register(added);
        //        var loggedIn = new Lab6.Viewmodels.LoginPostModel
        //        {
        //            Username = "ppetre",
        //            Password = "123456"

        //        };
        //        var authoresult = usersService.Authenticate(added.Username, added.Password);

        //        Assert.IsNotNull(authoresult);
        //        Assert.AreEqual(1, authoresult.Id);
        //        Assert.AreEqual(loggedIn.Username, authoresult.Username);
        //        //Assert.AreEqual(loggedIn.Password, UsersService.);
        //    }


        //}

        [Test]
        public void AuthenticateShouldLoginTheRegisteredUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(AuthenticateShouldLoginTheRegisteredUser))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var validator = new RegisterValidator();
                var validatorUser = new UserRoleValidator();
                var crValidator = new CreateValidator();
                var userUserRoleService = new UserUserRoleService(validatorUser, context);
                var usersService = new UsersService(context, validator, crValidator, userUserRoleService, config);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Created for test"
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.SaveChanges();

                var added = new Lab6.Viewmodels.RegisterPostModel
                {
                    FirstName = "Catalin",
                    LastName = "Albulescu",
                    Username = "albuc",
                    Email = "ac@aol.com",
                    Password = "12345678",
                };
                var result = usersService.Register(added);

                var authenticated = new Lab6.Viewmodels.LoginPostModel
                {
                    Username = "albuc",
                    Password = "12345678"
                };
                //valid authentification
                var authresult = usersService.Authenticate(added.Username, added.Password);

                Assert.IsNotNull(authresult);
                Assert.AreEqual(1, authresult.Id);
                Assert.AreEqual(authenticated.Username, authresult.Username);

                //invalid user authentification
                //var authresult1 = usersService.Authenticate("unknown", "abcdefg");
                //Assert.IsNull(authresult1);
            }

        }
        //[Test]
        //public void ValidGetAllShouldDisplayAllUsers()
        //{
        //    var options = new DbContextOptionsBuilder<ExpensesDbContext>()
        //      .UseInMemoryDatabase(databaseName:nameof(AuthenticateShouldLoginSuccessfullyTheUser))// "ValidGetAllShouldDisplayAllUsers")
        //      .Options;

        //    using (var context = new ExpensesDbContext(options))
        //    {
        //        var usersService = new UsersService(context,null, config);

        //        var added = new Lab6.Viewmodels.RegisterPostModel

        //        {
        //            Email = "petre@aol.com",
        //            FirstName = "Petre",
        //            LastName = "Popescu",
        //            Password = "123456",
        //            Username = "ppetre",
        //        };
        //        usersService.Register(added);

        //        // Act
        //        var result = usersService.GetAll();

        //        // Assert
        //        Assert.IsNotEmpty(result);
        //        Assert.AreEqual(1, result.Count());

        //    }
        //}

        [Test]
        public void ValidGetAllShouldReturnAllRegisteredUsers()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidGetAllShouldReturnAllRegisteredUsers))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var regValidator = new RegisterValidator();
                var crValidator = new CreateValidator();
                var usersService = new UsersService(context, regValidator, crValidator, null, config);
                var added1 = new Lab6.Viewmodels.RegisterPostModel
                {
                    FirstName = "Catalin",
                    LastName = "Albulescu",
                    Username = "albuc",
                    Email = "ac@aol.com",
                    Password = "12345678",
                };
                var added2 = new Lab6.Viewmodels.RegisterPostModel
                {
                    FirstName = "testfname",
                    LastName = "testsname",
                    Username = "test_user",
                    Email = "test@yahoo.com",
                    Password = "1111111111"
                };
                usersService.Register(added1);
                usersService.Register(added2);

                int numberOfElements = usersService.GetAll().Count();

                Assert.NotZero(numberOfElements);
                Assert.AreEqual(2, numberOfElements);

            }
        }

        [Test]
        public void GetByIdShouldReturnAnValidUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
         .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnAnValidUser))
         .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var regValidator = new RegisterValidator();
                var crValidator = new CreateValidator();
                var usersService = new UsersService(context, regValidator,crValidator, null, config);
                var added1 = new Lab6.Viewmodels.RegisterPostModel
                {
                    FirstName = "firstName",
                    LastName = "lastName",
                    Username = "test_user1",
                    Email = "test1@yahoo.com",
                    Password = "111111111"
                };

                usersService.Register(added1);
                var userById = usersService.GetById(1);

                Assert.NotNull(userById);
                Assert.AreEqual("firstName", userById.FirstName);

            }
        }

        [Test]
        public void GetCurentUserShouldReturnAccesToKlaims()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
        .UseInMemoryDatabase(databaseName: nameof(GetCurentUserShouldReturnAccesToKlaims))
        .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var regValidator = new RegisterValidator();
                var crValidator = new CreateValidator();
                var validatorUser = new UserRoleValidator();
                var userUserRoleService = new UserUserRoleService(validatorUser, context);
                var usersService = new UsersService(context, regValidator,crValidator, userUserRoleService, config);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Created for test"
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.SaveChanges();

                var added = new Lab6.Viewmodels.RegisterPostModel
                {
                    FirstName = "firstName",
                    LastName = "lastName",
                    Username = "test_user1",
                    Email = "test@yahoo.com",
                    Password = "11111111"
                };
                var result = usersService.Register(added);

                var authenticated = new Lab6.Viewmodels.LoginPostModel
                {
                    Username = "test_user1",
                    Password = "11111111"
                };
                var authresult = usersService.Authenticate(added.Username, added.Password);

                
                //usersService.GetCurentUser(httpContext);

                Assert.IsNotNull(authresult);
            }
        }

        [Test]
        public void CreateShouldReturnNullIfValidUserGetModel()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(CreateShouldReturnNullIfValidUserGetModel))
            .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var regValidator = new RegisterValidator();
                var crValidator = new CreateValidator();
                var usersService = new UsersService(context, regValidator,crValidator, null, config);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Created for test"
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.SaveChanges();

                var added1 = new Lab6.Viewmodels.UserPostModel
                {
                    FirstName = "firstName",
                    LastName = "lastName",
                    UserName = "test_user",
                    Email = "test@yahoo.com",
                    Password = "11111111",
                    UserRole = "Regular",
                };

                var userCreated = usersService.Create(added1);

                Assert.IsNull(userCreated);
            }
        }

        [Test]
        public void ValidDeleteShouldRemoveTheUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(ValidDeleteShouldRemoveTheUser))
            .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var validator = new RegisterValidator();
                var crValidator = new CreateValidator();
                var usersService = new UsersService(context, validator,crValidator, null, config);
                var added = new Lab6.Viewmodels.RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "firstName1",
                    Username = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };

                var userCreated = usersService.Register(added);

                Assert.NotNull(userCreated);

                //Assert.AreEqual(0, usersService.GetAll().Count());

                var userDeleted = usersService.Delete(1);

                Assert.Null(userDeleted);
                Assert.AreEqual(0, usersService.GetAll().Count());

            }
        }


        [Test]
        public void ValidUpsertShouldModifyFieldsValues()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(ValidUpsertShouldModifyFieldsValues))
            .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var validator = new RegisterValidator();
                var crValidator = new CreateValidator();
                var usersService = new UsersService(context, validator,crValidator, null, config);
                var added = new Lab6.Viewmodels.UserPostModel
                {
                    FirstName = "Nume",
                    LastName = "Prenume",
                    UserName = "userName",
                    Email = "user@yahoo.com",
                    Password = "333333"
                };

                usersService.Create(added);

                var updated = new Lab6.Models.User
                {
                    FirstName = "Alin",
                    LastName = "Popescu",
                    Username = "popAlin",
                    Email = "pop@yahoo.com",
                    Password = "333333"
                };

                var userUpdated = usersService.Upsert(1, updated);

                Assert.NotNull(userUpdated);
                Assert.AreEqual("Alin", userUpdated.FirstName);
                Assert.AreEqual("Popescu", userUpdated.LastName);

            }
        }

    }
}
    

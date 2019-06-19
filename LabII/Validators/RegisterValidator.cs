using Lab6.Viewmodels;
using Lab6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Validators
{
    public interface IRegisterValidator
    {
        ErrorsCollection Validate(RegisterPostModel registerPostModel, ExpensesDbContext context);
    }
    public class RegisterValidator : IRegisterValidator
    {
        public ErrorsCollection Validate(RegisterPostModel registerPostModel, ExpensesDbContext context)
        {
            ErrorsCollection errorsCollection = new ErrorsCollection { Entity = nameof(RegisterPostModel) };
            User existing = context.Users.FirstOrDefault(u => u.Username == registerPostModel.Username);
            if (existing != null)
            {
                errorsCollection.ErrorMessages.Add($"The username {registerPostModel.Username} is already taken !");
            }
            if (registerPostModel.Password.Length < 7)
            {
                errorsCollection.ErrorMessages.Add("The password cannot be shorter than 7 characters !");
            }
            if (errorsCollection.ErrorMessages.Count > 0)
            {
                return errorsCollection;
            }
            return null;
        }

    }
}

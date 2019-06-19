using Lab6.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Type = Lab6.Models.Type;

namespace Lab6.Viewmodels
{
    public class ExpenseGetModel
    {
        
        public string Description { get; set; }

        [EnumDataType(typeof(Type))]

        public Type Type { get; set; }

        public string Location { get; set; }

        public DateTime Date { get; set; }

        public string Currency { get; set; }

        public double Sum { get; set; }

        public int NumberOfComments { get; set; }

        public User Owner { get; set; }

        public static ExpenseGetModel FromExpense(Expense expense)
        {

            return new ExpenseGetModel
            {
                Description = expense.Description,
                Type = expense.Type,
                Location = expense.Location,
                Date = expense.Date,
                Currency = expense.Currency,
                Sum = expense.Sum,
                NumberOfComments = expense.Comments.Count,
                Owner = expense.Owner
            };
        }
    }


}

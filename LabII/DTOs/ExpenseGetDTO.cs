using LabII.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Type = LabII.Models.Type;

namespace LabII.DTOs
{
    public class ExpenseGetDTO
    {
        
        public string Description { get; set; }

        [EnumDataType(typeof(Type))]

        public Type Type { get; set; }

        public string Location { get; set; }

        public DateTime Date { get; set; }

        public string Currency { get; set; }

        public double Sum { get; set; }

        public int NumberOfComments { get; set; }

        public static ExpenseGetDTO FromExpense(Expense expense)
        {

            return new ExpenseGetDTO
            {
                Description = expense.Description,
                Type = expense.Type,
                Location = expense.Location,
                Date = expense.Date,
                Currency = expense.Currency,
                Sum = expense.Sum,
                NumberOfComments = expense.Comments.Count

            };
        }
    }


}

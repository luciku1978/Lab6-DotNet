using Lab6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Viewmodels
{
    public class CommentGetModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        public int ExpenseId { get; set; }


        public static CommentGetModel FromComment(Comment comment)
        {

            return new CommentGetModel
            {
                Id = comment.Id,
                Text = comment.Text,
                ExpenseId = comment.Expense.Id,
                Important = comment.Important
            };
        }

    }
}
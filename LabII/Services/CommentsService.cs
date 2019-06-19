using Lab6.Viewmodels;
using Lab6.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Services
{
    public interface ICommentService
    {
        IEnumerable<CommentGetModel> GetAll(String filter);
        Comment Create(CommentPostModel comment, User addedBy);

        Comment Upsert(int id, Comment comment);

        Comment Delete(int id);

        Comment GetById(int id);
    }

    public class CommentsService : ICommentService
    {

        private ExpensesDbContext context;

        public CommentsService(ExpensesDbContext context)
        {
            this.context = context;
        }

        public Comment Create(CommentPostModel comment, User addedBy)
        {
            Comment commentAdd = CommentPostModel.ToComment(comment);
            commentAdd.Owner = addedBy;
            context.Comments.Add(commentAdd);
            context.SaveChanges();
            return commentAdd;
        }

        public Comment Delete(int id)
        {
            var existing = context.Comments.FirstOrDefault(comment => comment.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Comments.Remove(existing);
            context.SaveChanges();
            return existing;
        }

        
        public IEnumerable<CommentGetModel> GetAll(String filter)
        {
            IQueryable<Expense> result = context.Expenses.Include(c => c.Comments);

            List<CommentGetModel> resultFilteredComments = new List<CommentGetModel>();
            List<CommentGetModel> resultAllComments = new List<CommentGetModel>();

            foreach (Expense expense in result)
            {
                expense.Comments.ForEach(c =>
                {
                    if(c.Text==null || filter == null)
                    {
                        CommentGetModel comment = new CommentGetModel
                        {
                            Id = c.Id,
                            Important = c.Important,
                            Text = c.Text,
                            ExpenseId = expense.Id

                        };
                        resultAllComments.Add(comment);
                    }
                    else if (c.Text.Contains(filter))
                    {
                        CommentGetModel comment = new CommentGetModel
                        {
                            Id = c.Id,
                            Important = c.Important,
                            Text = c.Text,
                            ExpenseId = expense.Id

                        };
                        resultFilteredComments.Add(comment);

                    }
                });
            }
            if(filter == null)
            {
                return resultAllComments;
            }
            return resultFilteredComments;
        }

        public Comment GetById(int id)
        {
            return context.Comments.FirstOrDefault(c => c.Id == id);
        }

        public Comment Upsert(int id, Comment comment)
        {
            var existing = context.Comments.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (existing == null)
            {
                context.Comments.Add(comment);
                context.SaveChanges();
                return comment;

            }

            comment.Id = id;
            context.Comments.Update(comment);
            context.SaveChanges();
            return comment;
        }
    }

}


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
        PaginatedListModel<CommentGetModel> GetAll(String filter,int page);
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


        public PaginatedListModel<CommentGetModel> GetAll(string filter, int page)
        {
            IQueryable<Comment> result = context
                .Comments
                .Where(c => string.IsNullOrEmpty(filter) || c.Text.Contains(filter))
                .OrderBy(c => c.Id)
                .Include(c => c.Expense);

            PaginatedListModel<CommentGetModel> paginatedResult = new PaginatedListModel<CommentGetModel>();
            paginatedResult.CurrentPage = page;

            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedListModel<CommentGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedListModel<CommentGetModel>.EntriesPerPage)
                .Take(PaginatedListModel<CommentGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(f => CommentGetModel.FromComment(f)).ToList();

            return paginatedResult;
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


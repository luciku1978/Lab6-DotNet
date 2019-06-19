using Lab6.Viewmodels;
using Lab6.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Services
{
    public interface IExpenseService
    {

        PaginatedListModel<ExpenseGetModel> GetAll(int page, DateTime? from=null, DateTime? to=null, Models.Type? type=null);

        Expense GetById(int id);

        Expense Create(ExpensePostModel expense, User addedBy);

        Expense Upsert(int id, Expense expense);

        Expense Delete(int id);

    }
    public class ExpensesService : IExpenseService
    {       

        private ExpensesDbContext context;

        public ExpensesService(ExpensesDbContext context)
        {
            this.context = context;
        }


        public Expense Create(ExpensePostModel expense, User addedBy)
        {
            Expense addExp = ExpensePostModel.ToExpense(expense);
            addExp.Owner = addedBy;
            context.Expenses.Add(addExp);
            context.SaveChanges();
            return addExp;
        }

        public Expense Delete(int id)
        {
            var existing = context.Expenses.Include(x => x.Comments).FirstOrDefault(expense => expense.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Expenses.Remove(existing);
            context.SaveChanges();
            return existing;
        }
       
        public PaginatedListModel<ExpenseGetModel> GetAll(int page, DateTime? from = null, DateTime? to = null, Models.Type? type = null)
        {
            IQueryable<Expense> result = context
                .Expenses
                .OrderBy(e => e.Id)
                .Include(x => x.Comments);

            PaginatedListModel<ExpenseGetModel> paginatedResult = new PaginatedListModel<ExpenseGetModel>();
            paginatedResult.CurrentPage = page;

            if (from != null)
            {
                result = result.Where(e => e.Date >= from);
            }
            if (to != null)
            {
                result = result.Where(e => e.Date <= to);
            }
            if (type != null)
            {
                result = result.Where(e => e.Type == type);
            }
            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedListModel<ExpenseGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedListModel<ExpenseGetModel>.EntriesPerPage)
                .Take(PaginatedListModel<ExpenseGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(e => ExpenseGetModel.FromExpense(e)).ToList();

            return paginatedResult;
        }

        public Expense GetById(int id)
        {
            return context.Expenses.Include(x => x.Comments).FirstOrDefault(e => e.Id == id);
        }

        public Expense Upsert(int id, Expense expense)
        {
            var existing = context.Expenses.AsNoTracking().FirstOrDefault(e => e.Id == id);
            if (existing == null)
            {
                context.Expenses.Add(expense);
                context.SaveChanges();
                return expense;

            }

            expense.Id = id;
            context.Expenses.Update(expense);
            context.SaveChanges();
            return expense;
        }

    }

}

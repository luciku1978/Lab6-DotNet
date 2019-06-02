using LabII.DTOs;
using LabII.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabII.Services
{
    public interface IExpenseService
    {

        PaginatedList<ExpenseGetDTO> GetAll(int page, DateTime? from=null, DateTime? to=null, Models.Type? type=null);

        Expense GetById(int id);

        Expense Create(ExpensePostDTO user);

        Expense Upsert(int id, Expense user);

        Expense Delete(int id);

    }
    public class ExpenseService : IExpenseService
    {       

        private ExpensesDbContext context;

        public ExpenseService(ExpensesDbContext context)
        {
            this.context = context;
        }


        public Expense Create(ExpensePostDTO expense)
        {
            Expense addExp = ExpensePostDTO.ToExpense(expense);
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
       
        public PaginatedList<ExpenseGetDTO> GetAll(int page, DateTime? from = null, DateTime? to = null, Models.Type? type = null)
        {
            IQueryable<Expense> result = context
                .Expenses
                .OrderBy(e => e.Id)
                .Include(x => x.Comments);

            PaginatedList<ExpenseGetDTO> paginatedResult = new PaginatedList<ExpenseGetDTO>();
            paginatedResult.CurrentPage = page;

            //if (page == null && from == null && to == null && type == null)

            //{
            //    return result;
            //}
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
            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<ExpenseGetDTO>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<ExpenseGetDTO>.EntriesPerPage)
                .Take(PaginatedList<ExpenseGetDTO>.EntriesPerPage);
            paginatedResult.Entries = result.Select(e => ExpenseGetDTO.FromExpense(e)).ToList();

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

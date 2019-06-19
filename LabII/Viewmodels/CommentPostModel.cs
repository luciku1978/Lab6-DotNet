using Lab6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Viewmodels
{
    public class CommentPostModel
    {
        public string Text { get; set; }
        public bool Important { get; set; }



        public static Comment ToComment(CommentPostModel comment)
        {
            return new Comment
            {
                Text = comment.Text,
                Important = comment.Important,

            };
        }
    }
}

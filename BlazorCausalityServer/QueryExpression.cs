using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BlazorCausalityServer
{
    public class QueryExpression<T>
    {
        public bool Distinct { get; set; }
        public List<string> Includes { get; set; }
        public Expression<Func<T, bool>> Where { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public Expression<Func<T, object>> ThenBy { get; set; }
        public Expression<Func<T, object>> ThenByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}

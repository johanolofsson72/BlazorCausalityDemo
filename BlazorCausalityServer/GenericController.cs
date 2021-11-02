using BlazorCausalityServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serialize.Linq.Serializers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlazorCausality
{

    public class GenericController<T, C> : ControllerBase
        where T : EntityBase, new()
        where C : DbContext
    {
        private readonly ServerRepository<T, C> _manager;
        private static int switch_on;

        public GenericController(ServerRepository<T, C> manager) => _manager = manager;

        [HttpPost("get")]
        public async ValueTask<ActionResult<List<T>>> Get([FromBody] string jsonQuery)
        {
            try
            {
                List<Query> queryBuilder = JsonConvert.DeserializeObject<List<Query>>(jsonQuery);
                IQueryable<T> queryable = await _manager.GetSpecial();

                if (queryBuilder.Count > 0)
                {
                    QueryExpression<T> queryExpression = new();
                    DeserializeAndBuildQuery(queryBuilder, queryExpression);
                    queryable = ConcatAndBuildQuery(queryable, queryExpression);
                }

                Log.Warning("Get.SQL: {V}", queryable.ToQueryString());
                return queryable.ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Get: {Ex}", ex.ToString());
                return StatusCode(566);
            }
        }

        private static IQueryable<T> ConcatAndBuildQuery(IQueryable<T> queryable, QueryExpression<T> queryExpression)
        {
            if (queryExpression.Distinct && (queryExpression.OrderBy is not null || queryExpression.OrderByDescending is not null))
            {
                queryable = queryable.Distinct();
            }

            if (queryExpression.Includes is not null)
            {
                foreach (string where in queryExpression.Includes)
                {
                    queryable = queryable.Include(where);
                }
            }

            if (queryExpression.Where is not null)
            {
                queryable = queryable.Where(queryExpression.Where);
            }

            if (queryExpression.ThenBy is not null && queryExpression.OrderBy is not null)
            {
                queryable = queryable.OrderBy(queryExpression.OrderBy).ThenBy(queryExpression.ThenBy);
            }

            if (queryExpression.ThenBy is not null && queryExpression.OrderByDescending is not null)
            {
                queryable = queryable.OrderByDescending(queryExpression.OrderByDescending).ThenBy(queryExpression.ThenBy);
            }

            if (queryExpression.ThenByDescending is not null && queryExpression.OrderBy is not null)
            {
                queryable = queryable.OrderBy(queryExpression.OrderBy).ThenByDescending(queryExpression.ThenByDescending);
            }

            if (queryExpression.ThenByDescending is not null && queryExpression.OrderByDescending is not null)
            {
                queryable = queryable.OrderByDescending(queryExpression.OrderByDescending).ThenByDescending(queryExpression.ThenByDescending);
            }

            if (queryExpression.OrderBy is not null && (queryExpression.ThenBy is null && queryExpression.ThenByDescending is null))
            {
                queryable = queryable.OrderBy(queryExpression.OrderBy);
            }

            if (queryExpression.OrderByDescending is not null && (queryExpression.ThenBy is null && queryExpression.ThenByDescending is null))
            {
                queryable = queryable.OrderByDescending(queryExpression.OrderByDescending);
            }

            if (queryExpression.Skip > 0 && (queryExpression.OrderBy is not null || queryExpression.OrderByDescending is not null))
            {
                queryable = queryable.Skip(queryExpression.Skip);
            }

            if (queryExpression.Take > 0 && (queryExpression.OrderBy is not null || queryExpression.OrderByDescending is not null))
            {
                queryable = queryable.Take(queryExpression.Take);
            }

            return queryable;
        }

        private static void DeserializeAndBuildQuery(List<Query> queryBuilder, QueryExpression<T> queryExpression)
        {
            foreach (Query item in queryBuilder.OrderBy(q => q.Index))
            {
                ExpressionSerializer serializer = new(new BinarySerializer());

                switch (item.Type)
                {
                    case LinqType.Distinct:
                        queryExpression.Distinct = true;
                        break;
                    case LinqType.Include:
                        queryExpression.Includes = item.Expression.DeserializeToListOfStrings();
                        break;
                    case LinqType.Where:
                        queryExpression.Where = (Expression<Func<T, bool>>)item.Expression.DeserializeToExpression();
                        break;
                    case LinqType.OrderBy:
                        queryExpression.OrderBy = (Expression<Func<T, object>>)item.Expression.DeserializeToExpression();
                        break;
                    case LinqType.OrderByDescending:
                        queryExpression.OrderByDescending = (Expression<Func<T, object>>)item.Expression.DeserializeToExpression();
                        break;
                    case LinqType.ThenBy:
                        queryExpression.ThenBy = (Expression<Func<T, object>>)item.Expression.DeserializeToExpression();
                        break;
                    case LinqType.ThenByDescending:
                        queryExpression.ThenByDescending = (Expression<Func<T, object>>)item.Expression.DeserializeToExpression();
                        break;
                    case LinqType.Skip:
                        queryExpression.Skip = item.Expression.DeserializeToInt();
                        break;
                    case LinqType.Take:
                        queryExpression.Take = item.Expression.DeserializeToInt();
                        break;
                    default:
                        break;
                }

            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetById(string id)
        {
            try
            {
                T user = await _manager.GetById(Convert.ToInt32(id));
                return Ok(user);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"GetBinaryAsync: {ex}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<T>> Insert(T entity)
        {
            try
            {
                T user = await _manager.Insert(entity);
                return Ok(user);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"GetBinaryAsync: {ex}");
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult<T>> Update(T entity)
        {
            try
            {
                T user = await _manager.Update(entity);
                return Ok(user);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"GetBinaryAsync: {ex}");
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _manager.Delete(Convert.ToInt32(id));
                return Ok();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"GetBinaryAsync: {ex}");
                return StatusCode(500);
            }
        }

    }
}

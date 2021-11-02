using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCausality
{
    public class Linq<T> : ClientRepository<T>
        where T : class
    {
        private List<Query> QueryBuilder = new();
        private int QueryCounter { get; set; }

        public Linq(HttpClient http) : base(http)
        {
        }

        /// <summary>
        /// Perform a distinct select
        /// </summary>
        /// <returns></returns>
        public Linq<T> Distinct()
        {
            QueryBuilder.Add(
                new Query()
                {
                    Index = GetCurrentIndex(),
                    Type = LinqType.Distinct,
                    Expression = Array.Empty<byte>()
                });

            return this;
        }

        /// <summary>
        /// Included properties as comma seperated string
        /// </summary>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public Linq<T> Include(string includeProperties)
        {
            QueryBuilder.Add(
                new Query()
                {
                    Index = GetCurrentIndex(),
                    Type = LinqType.Include,
                    Expression = Encoding.ASCII.GetBytes(includeProperties)
                });

            return this;
        }

        /// <summary>
        /// Where
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Linq<T> Where(Expression<Func<T, bool>> predicate)
        {
            QueryBuilder.Add(
                new Query()
                {
                    Index = GetCurrentIndex(),
                    Type = LinqType.Where,
                    Expression = predicate.Serialize()
                });

            return this;
        }

        /// <summary>
        /// OrderBy
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Linq<T> OrderBy(Expression<Func<T, object>> predicate)
        {
            QueryBuilder.Add(
                new Query()
                {
                    Index = GetCurrentIndex(),
                    Type = LinqType.OrderBy,
                    Expression = predicate.Serialize()
                });

            return this;
        }

        /// <summary>
        /// OrderByDescending
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Linq<T> OrderByDescending(Expression<Func<T, object>> predicate)
        {
            QueryBuilder.Add(
                new Query()
                {
                    Index = GetCurrentIndex(),
                    Type = LinqType.OrderByDescending,
                    Expression = predicate.Serialize()
                });

            return this;
        }

        /// <summary>
        /// ThenBy
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Linq<T> ThenBy(Expression<Func<T, object>> predicate)
        {
            QueryBuilder.Add(
                new Query()
                {
                    Index = GetCurrentIndex(),
                    Type = LinqType.ThenBy,
                    Expression = predicate.Serialize()
                });

            return this;
        }

        /// <summary>
        /// ThenByDescending
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Linq<T> ThenByDescending(Expression<Func<T, object>> predicate)
        {
            QueryBuilder.Add(
                new Query()
                {
                    Index = GetCurrentIndex(),
                    Type = LinqType.ThenByDescending,
                    Expression = predicate.Serialize()
                });

            return this;
        }

        /// <summary>
        /// Skip
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Linq<T> Skip(int count)
        {
            QueryBuilder.Add(
                new Query()
                {
                    Index = GetCurrentIndex(),
                    Type = LinqType.Skip,
                    Expression = Encoding.ASCII.GetBytes(count.ToString())
                });

            return this;
        }

        /// <summary>
        /// Take
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Linq<T> Take(int count)
        {
            QueryBuilder.Add(
                new Query()
                {
                    Index = GetCurrentIndex(),
                    Type = LinqType.Take,
                    Expression = Encoding.ASCII.GetBytes(count.ToString())
                });

            return this;
        }

        /// <summary>
        /// Executes the query on the server
        /// </summary>
        /// <returns>List<T></returns>
        public async Task<List<T>> ToListAsync()
        {
            string jsonString = QueryBuilder.Serialize();

            CleanInstance();

            return await GetAsync(jsonString);
        }

        private int GetCurrentIndex()
        {
            QueryCounter++;
            return QueryCounter - 1;
        }

        private void CleanInstance()
        {
            QueryBuilder = new();
            QueryCounter = 0;
        }















        /// <summary>
        /// Join
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="inner"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        //public Linq<T> Join<U, K, V>(IEnumerable<U> inner, Expression<Func<T, K>> outerKeySelector, Expression<Func<U, K>> innerKeySelector, Expression<Func<T, U, V>> resultSelector)
        //{
        //    //var jsonString = JsonConvert.SerializeObject(inner, Formatting.Indented);
        //    //var arrayout = Encoding.ASCII.GetBytes(jsonString);

        //    var jsonString = JsonConvert.SerializeObject(new Inner<U>() { Value = inner }, Formatting.Indented);
        //    var arrayout = Encoding.ASCII.GetBytes(jsonString);

        //    QueryBuilder.Add(
        //        new Query() { 
        //            Index = GetCurrentIndex(), 
        //            Name = "Join", 
        //            Type = typeof(Expression<Func<object, object>>), 
        //            Expression1 = arrayout, 
        //            Expression2 = outerKeySelector.Serialize(), 
        //            Expression3 = innerKeySelector.Serialize(), 
        //            Expression4 = resultSelector.Serialize(), 
        //            QueryType1 = typeof(T),
        //            QueryType2 = typeof(U),
        //            QueryType3 = typeof(K),
        //            QueryType4 = typeof(V)
        //        });

        //    return this;
        //}

        /// <summary>
        /// SelectMany
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="selector"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        //public Linq<T> SelectMany<U, V>(Expression<Func<T, IEnumerable<U>>> selector, Expression<Func<T, U, V>> resultSelector)
        //{
        //    QueryBuilder.Add(
        //        new Query()
        //        {
        //            Index = GetCurrentIndex(),
        //            Name = "SelectMany",
        //            Type = typeof(Expression<Func<object, IEnumerable<object>>>),
        //            Expression1 = selector.Serialize(),
        //            Expression2 = resultSelector.Serialize(),
        //            Expression3 = Array.Empty<byte>(),
        //            Expression4 = Array.Empty<byte>(),
        //            QueryType1 = typeof(T),
        //            QueryType2 = typeof(U),
        //            QueryType3 = typeof(V),
        //            QueryType4 = typeof(T)
        //        });

        //    return this;
        //}


        //public Linq<T> Where(Expression<Func<T, bool>> predicate)
        //{
        //    PredicateWhere = predicate;

        //    return this;
        //}

        //public async Task<List<T>> ToListAsync()
        //{
        //    byte[] bytes = Array.Empty<byte>();

        //    if (PredicateWhere is not null)
        //    {
        //        var serializer = new ExpressionSerializer(new BinarySerializer());
        //        bytes = serializer.SerializeBinary(PredicateWhere);
        //    }

        //    CleanInstance();

        //    return await GetBinaryAsync(bytes);
        //}
        //public Linq<T> Skip(int count)
        //{
        //    string query = Data.Skip(count).ToString();
        //    Queries += query[(query.LastIndexOf("]") + 1)..];

        //    return this;
        //}



        //private static string CleanUpQuery(string query)
        //{


        //    return query
        //        .Replace("AndAlso", "&&")
        //        .Replace("OrElse", "||");
        //}


        //public Linq<T> OrderBy(string property, bool ascending)
        //{
        //    var source = Expression.Parameter(typeof(IQueryable<T>), "source");
        //    var item = Expression.Parameter(typeof(T), "item");
        //    var member = Expression.Property(item, property);
        //    var selector = Expression.Quote(Expression.Lambda(member, item));
        //    var body = Expression.Call(
        //        typeof(Queryable), ascending ? "OrderBy" : "OrderByDescending",
        //        new System.Type[] { item.Type, member.Type },
        //        source, selector);
        //    var expr = Expression.Lambda<Func<IQueryable<T>, IOrderedQueryable<T>>>(body, source);
        //    var expression = expr.Compile();
        //    var tt = expression.ToString();



        //    return this;
        //}

        //public Linq<T> OrderBy(Expression<Func<T, object>> keySelector)
        //{
        //    var xpo = Binary.ObjectToByteArray(keySelector);

        //    keySelector = CleanPredicate(keySelector);

        //    var x = Data.OrderBy(keySelector);

        //    var y = CleanOrderBy(x);

        //    string query = y.ToString();
        //    Queries += query[(query.LastIndexOf("]") + 1)..];

        //    return this;
        //}

        //public Linq<T> OrderByDescending(Expression<Func<T, object>> keySelector)
        //{
        //    keySelector = CleanPredicate(keySelector);

        //    string query = Data.OrderByDescending(keySelector).ToString();
        //    Queries += query[(query.LastIndexOf("]") + 1)..];

        //    return this;
        //}

        ////public async Task<List<R>> ToListAsync<R>()
        ////{
        ////    string query = Queries[(Queries.IndexOf(".") + 1)..];
        ////    string include = string.Join(",", this.Includes);

        ////    query = CleanUpQuery(query);
        ////    query = HttpUtility.UrlEncode(query);
        ////    include = HttpUtility.UrlEncode(include);

        ////    CleanInstance();

        ////    return await GetWithOtherReturnType<R>($"?linq={query}&incl={include}&resu={typeof(R).FullName}");
        ////}



        //public Linq<T> Select(Expression<Func<T, object>> predicate)
        //{
        //    predicate = CleanPredicate(predicate);

        //    string query = Data.Select(predicate).ToString();
        //    Queries += query[(query.LastIndexOf("]") + 1)..];

        //    return this;
        //}

        //public Linq<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        //{
        //    predicate = CleanPredicate(predicate);

        //    string query = Data.FirstOrDefault(predicate).ToString();
        //    Queries += query[(query.LastIndexOf("]") + 1)..];

        //    return this;
        //}



        /// <summary>
        /// Debug purpose
        /// </summary>
        /// <returns></returns>
        //public async Task<string> ToStringAsync()
        //{
        //    string query = Queries;//[(Queries.IndexOf(".") + 1)..];
        //    string include = string.Join(",", this.Includes);

        //    query = CleanUpQuery(query);
        //    //query = HttpUtility.UrlEncode(query);
        //    //include = HttpUtility.UrlEncode(include);

        //    CleanInstance();
        //    await Task.Delay(0);
        //    return query;
        //}

        //private async Task Log(string error)
        //{
        //    using StreamWriter log = new(Path.Combine("blazorcausality.log"), append: true);
        //    await log.WriteLineAsync(error);
        //}


        //public Linq<T> Join<U, K, V>(IEnumerable<U> inner, Expression<Func<T, K>> outerKeySelector, Expression<Func<U, K>> innerKeySelector, Expression<Func<T, U, V>> resultSelector)
        //{
        //    outerKeySelector = CleanOuterKeySelector<U, K, V>(outerKeySelector);
        //    innerKeySelector = CleanInnerKeySelector<U, K, V>(innerKeySelector);
        //    resultSelector = CleanResultSelector<U, K, V>(resultSelector);

        //    string query = Data.Join(inner, outerKeySelector, innerKeySelector, resultSelector).ToString();
        //    Queries += ".Join(" + typeof(U).Name + query[(query.LastIndexOf("]") + 1)..][1..];

        //    return this;
        //}

        //public Linq<T> SelectMany<U, V>(Expression<Func<T, IEnumerable<U>>> selector, Expression<Func<T, U, V>> resultSelector)
        //{
        //    selector = CleanSelector<U, V>(selector);
        //    resultSelector = CleanResultSelector(resultSelector);

        //    string query = Data.SelectMany(selector, resultSelector).ToString();
        //    Queries += query[(query.LastIndexOf("]") + 1)..];

        //    return this;
        //}

        //public Linq<T> Include(string entityCollection)
        //{
        //    Includes.Add(entityCollection);

        //    return this;
        //}


        //    private static Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> CleanOrderBy(Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> selector)
        //    {
        //        var serializer = new ExpressionSerializer(new BinarySerializer());
        //        var bytes = serializer.SerializeBinary(selector);
        //        var predicateDeserialized = serializer.DeserializeBinary(bytes);
        //        selector = (Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>)predicateDeserialized;
        //        return selector;
        //    }

        //    private static object Decompress(object compressed)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    private static object Compress(byte[] vs)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    private static Expression<Func<T, IEnumerable<U>>> CleanSelector<U, V>(Expression<Func<T, IEnumerable<U>>> selector)
        //    {
        //        var serializer = new ExpressionSerializer(new BinarySerializer());
        //        var bytes = serializer.SerializeBinary(selector);
        //        var predicateDeserialized = serializer.DeserializeBinary(bytes);
        //        selector = (Expression<Func<T, IEnumerable<U>>>)predicateDeserialized;
        //        return selector;
        //    }

        //    private static Expression<Func<T, U, V>> CleanResultSelector<U, V>(Expression<Func<T, U, V>> selector)
        //    {
        //        var serializer = new ExpressionSerializer(new BinarySerializer());
        //        var bytes = serializer.SerializeBinary(selector);
        //        var predicateDeserialized = serializer.DeserializeBinary(bytes);
        //        selector = (Expression<Func<T, U, V>>)predicateDeserialized;
        //        return selector;
        //    }

        //    private static Expression<Func<T, object>> CleanPredicate(Expression<Func<T, object>> predicate)
        //    {
        //        var serializer = new ExpressionSerializer(new BinarySerializer());
        //        var bytes = serializer.SerializeBinary(predicate);
        //        var predicateDeserialized = serializer.DeserializeBinary(bytes);
        //        predicate = (Expression<Func<T, object>>)predicateDeserialized;
        //        return predicate;
        //    }

        //    private static Expression<Func<T, string>> CleanPredicateString(Expression<Func<T, string>> predicate)
        //    {
        //        var serializer = new ExpressionSerializer(new BinarySerializer());
        //        var bytes = serializer.SerializeBinary(predicate);
        //        var predicateDeserialized = serializer.DeserializeBinary(bytes);
        //        predicate = (Expression<Func<T, string>>)predicateDeserialized;
        //        return predicate;
        //    }


        //    private static Expression<Func<T, bool>> CleanPredicate(Expression<Func<T, bool>> predicate)
        //    {
        //        var serializer = new ExpressionSerializer(new BinarySerializer());
        //        var bytes = serializer.SerializeBinary(predicate);
        //        var predicateDeserialized = serializer.DeserializeBinary(bytes);
        //        predicate = (Expression<Func<T, bool>>)predicateDeserialized;
        //        return predicate;
        //    }

        //    private static Expression<IEnumerable<U>> CleanInner<U, K, V>(Expression<IEnumerable<U>> inner)
        //    {
        //        var serializer = new ExpressionSerializer(new BinarySerializer());
        //        var bytes = serializer.SerializeBinary(inner);
        //        var predicateDeserialized = serializer.DeserializeBinary(bytes);
        //        inner = (Expression<IEnumerable<U>>)predicateDeserialized;
        //        return inner;
        //    }

        //    private static Expression<Func<T, K>> CleanOuterKeySelector<U, K, V>(Expression<Func<T, K>> outerKeySelector)
        //    {
        //        var serializer = new ExpressionSerializer(new BinarySerializer());
        //        var bytes = serializer.SerializeBinary(outerKeySelector);
        //        var predicateDeserialized = serializer.DeserializeBinary(bytes);
        //        outerKeySelector = (Expression<Func<T, K>>)predicateDeserialized;
        //        return outerKeySelector;
        //    }

        //    private static Expression<Func<U, K>> CleanInnerKeySelector<U, K, V>(Expression<Func<U, K>> innerKeySelector)
        //    {
        //        var serializer = new ExpressionSerializer(new BinarySerializer());
        //        var bytes = serializer.SerializeBinary(innerKeySelector);
        //        var predicateDeserialized = serializer.DeserializeBinary(bytes);
        //        innerKeySelector = (Expression<Func<U, K>>)predicateDeserialized;
        //        return innerKeySelector;
        //    }

        //    private static Expression<Func<T, U, V>> CleanResultSelector<U, K, V>(Expression<Func<T, U, V>> resultSelector)
        //    {
        //        var serializer = new ExpressionSerializer(new BinarySerializer());
        //        var bytes = serializer.SerializeBinary(resultSelector);
        //        var predicateDeserialized = serializer.DeserializeBinary(bytes);
        //        resultSelector = (Expression<Func<T, U, V>>)predicateDeserialized;
        //        return resultSelector;
        //    }

        //}

        //public static class Binary
        //{
        //    public static byte[] StructToBytes<T>(T t)
        //    {
        //        using (var ms = new MemoryStream())
        //        {
        //            var bf = new BinaryFormatter();
        //            bf.Serialize(ms, t);
        //            return ms.ToArray();
        //        }
        //    }

        //    public static T BytesToStruct<T>(byte[] bytes)
        //    {
        //        using (var memStream = new MemoryStream())
        //        {
        //            var binForm = new BinaryFormatter();
        //            memStream.Write(bytes, 0, bytes.Length);
        //            memStream.Seek(0, SeekOrigin.Begin);
        //            var obj = binForm.Deserialize(memStream);
        //            return (T)obj;
        //        }
        //    }

        //    /// <summary>
        //    /// Convert an object to a Byte Array, using Protobuf.
        //    /// </summary>
        //    public static byte[] ObjectToByteArray(object obj)
        //    {
        //        if (obj == null)
        //            return null;

        //        using var stream = new MemoryStream();

        //        Serializer.Serialize(stream, obj);

        //        return stream.ToArray();
        //    }

        //    /// <summary>
        //    /// Convert a byte array to an Object of T, using Protobuf.
        //    /// </summary>
        //    public static T ByteArrayToObject<T>(byte[] arrBytes)
        //    {
        //        using var stream = new MemoryStream();

        //        // Ensure that our stream is at the beginning.
        //        stream.Write(arrBytes, 0, arrBytes.Length);
        //        stream.Seek(0, SeekOrigin.Begin);

        //        return Serializer.Deserialize<T>(stream);
        //    }
        //}
    }
}

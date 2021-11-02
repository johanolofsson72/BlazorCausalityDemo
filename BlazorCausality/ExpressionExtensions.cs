using Newtonsoft.Json;
using Serialize.Linq.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BlazorCausality
{
    public static class ExpressionExtension
    {
        public static byte[] Serialize(this Expression expression)
        {
            ExpressionSerializer serializer = new ExpressionSerializer(new BinarySerializer());
            byte[] bytes = serializer.SerializeBinary(expression);
            return bytes;
        }

        public static Expression DeserializeToExpression(this byte[] expression)
        {
            ExpressionSerializer serializer = new ExpressionSerializer(new BinarySerializer());
            Expression predicateDeserialized = serializer.DeserializeBinary(expression);
            return predicateDeserialized;
        }

        public static int DeserializeToInt(this byte[] expression)
        {
            _ = int.TryParse(Encoding.ASCII.GetString(expression), out int value);
            return value;
        }

        public static List<string> DeserializeToListOfStrings(this byte[] expression)
        {
            string[] stringArray = Encoding.ASCII.GetString(expression).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return stringArray.ToList();
        }

    }

    public static class ListExtension
    {
        public static string Serialize(this List<Query> list)
        {
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }
    }

}

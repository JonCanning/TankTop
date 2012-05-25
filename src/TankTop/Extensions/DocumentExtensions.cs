using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TankTop.Dto;

namespace TankTop.Extensions
{
    public static class DocumentExtensions
    {
        public static object ToSerializable(this Document document)
        {
            return new
            {
                docid = document.DocId,
                fields = document.Fields,
                variables = document.Variables,
                categories = document.Categories
            };
        }

        public static void Check(this Document document)
        {
            document.Check(x => x.DocId);
            document.Check(x => x.Fields, x => x.Any());
        }

        public static Document AddField(this Document document, string key, string value)
        {
            document.Fields = document.Fields ?? new Dictionary<string, string>();
            document.Fields.Add(key, value);
            return document;
        }

        public static Document AddFields<T>(this Document document, T obj)
        {
            document.Fields = obj.ToDictionary();
            return document;
        }

        public static Document AddVariable(this Document document, int key, DateTime dateTime)
        {
            document.Variables = document.Variables ?? new Dictionary<int, float>();
            document.Variables.Add(key, dateTime.ToUnixEpoch());
            return document;
        }

        public static Document AddVariable(this Document document, int key, float value)
        {
            document.Variables = document.Variables ?? new Dictionary<int, float>();
            document.Variables.Add(key, value);
            return document;
        }

        public static Document AddCategory(this Document document, string key, string value)
        {
            document.Categories = document.Categories ?? new Dictionary<string, string>();
            document.Categories.Add(key, value);
            return document;
        }

        public static Document AddFieldFrom<T>(this Document document, T obj, Expression<Func<T, string>> expression) where T : class
        {
            var propertyName = expression.PropertyName().ToLower();
            var property = expression.Compile().Invoke(obj);
            var propertyValue = property.IsNull() ? string.Empty : property;
            document.AddField(propertyName, propertyValue);
            return document;
        }
    }
}
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

        public static Document<T> AddField<T>(this Document<T> document, Expression<Func<T, object>> field) where T : class
        {
            return document.AddFields(field);
        }

        public static Document<T> AddFields<T>(this Document<T> document, params Expression<Func<T, object>>[] fieldsToIndex) where T : class
        {
            if (fieldsToIndex.Any())
            {
                foreach (var field in fieldsToIndex)
                {
                    document.Fields = document.Fields ?? new Dictionary<string, string>();
                    document.Fields.Add(field.PropertyName().ToLower(), field.Compile().Invoke(document.Obj).ToString());
                }
            }
            return document;
        }

        public static Document<T> AddAllFields<T>(this Document<T> document) where T : class
        {
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var parameterExpression = Expression.Parameter(typeof(T));
                var memberExpression = Expression.Property(parameterExpression, propertyInfo);
                var lambdaExpression = Expression.Lambda<Func<T, object>>(memberExpression, parameterExpression);
                document.Fields = document.Fields ?? new Dictionary<string, string>();
                var propertyValue = lambdaExpression.Compile().Invoke(document.Obj);
                document.Fields.Add(propertyInfo.Name.ToLower(), propertyValue.IsNull() ? string.Empty : propertyValue.ToString());
            }
            return document;
        }

        public static Document AddVariable(this Document document, int key, DateTime dateTime)
        {
            document.Variables = document.Variables ?? new Dictionary<int, float>();
            document.Variables.Add(key, dateTime.ToUnixEpoch());
            return document;
        }

        public static Document<T> AddVariable<T>(this Document<T> document, int key, DateTime dateTime) where T : class
        {
            return document.CastTo<Document>().AddVariable(key, dateTime).CastTo<Document<T>>();
        }

        public static Document AddVariable(this Document document, int key, float value)
        {
            document.Variables = document.Variables ?? new Dictionary<int, float>();
            document.Variables.Add(key, value);
            return document;
        }

        public static Document<T> AddVariable<T>(this  Document<T> document, int key, float value) where T : class
        {
            return document.CastTo<Document>().AddVariable(key, value).CastTo<Document<T>>();
        }

        public static Document AddVariable(this Document document, int key, int value)
        {
            document.AddVariable(key, (float)value);
            return document;
        }

        public static Document<T> AddVariable<T>(this Document<T> document, int key, int value) where T : class
        {
            return document.CastTo<Document>().AddVariable(key, value).CastTo<Document<T>>();
        }

        public static Document AddVariable(this Document document, int key, decimal value)
        {
            document.AddVariable(key, (float)value);
            return document;
        }

        public static Document<T> AddVariable<T>(this Document<T> document, int key, decimal value) where T : class
        {
            return document.CastTo<Document>().AddVariable(key, value).CastTo<Document<T>>();
        }

        public static Document AddCategory(this Document document, string key, string value)
        {
            document.Categories = document.Categories ?? new Dictionary<string, string>();
            document.Categories.Add(key, value);
            return document;
        }

        public static Document<T> AddCategory<T>(this Document<T> document, string key, string value) where T : class
        {
            return document.CastTo<Document>().AddCategory(key, value).CastTo<Document<T>>();
        }

        public static Document AddCategories(this Document document, IDictionary<string, string> categories)
        {
            document.Categories = categories;
            return document;
        }

        public static Document<T> AddCategories<T>(this Document<T> document, IDictionary<string, string> categories) where T : class
        {
            return document.CastTo<Document>().AddCategories(categories).CastTo<Document<T>>();
        }
    }
}
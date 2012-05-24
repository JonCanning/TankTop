using System.Collections.Generic;
using System.Linq;
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
    }
}
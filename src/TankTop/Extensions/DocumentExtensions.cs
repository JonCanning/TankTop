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
    }
}
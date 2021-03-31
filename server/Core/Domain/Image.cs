using System;

namespace Recipes.Core.Domain
{
    public class Image
    {
        public Image() { }

        public Image(
            string url,
            string name)
        {
            Url = url;
            Name = name;
        }

        public string Url { get; protected set; }
        public string Name { get; protected set; }

        public void Upsert(string url, string name, DateTime dateTime)
        {
            Url = url;
            Name = name;
        }
    }
}

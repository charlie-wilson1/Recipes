namespace Recipes.Identity.Application.Common
{
    public static class StringModifiers
    {
        public static string CapitalizeWords(this string str)
        {
            var words = str.Split(" ");

            foreach(var word in words)
            {
                char.ToUpper(word[0]);
            }

            return string.Join(" ", words);
        }
    }
}

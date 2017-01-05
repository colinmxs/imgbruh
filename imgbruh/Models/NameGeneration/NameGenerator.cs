using System;

namespace imgbruh.Models.NameGeneration
{
    public interface INameGenerator
    {
        string Generate();
        string GenerateUnique(string[] reservedNames);
    }

    public abstract class AbstractNameGenerator : INameGenerator
    {
        public NameGenerator _nameGenerator;

        public string Generate()
        {
            return _nameGenerator.Generate();
        }

        public string GenerateUnique(string[] reservedNames)
        {
            return _nameGenerator.GenerateUnique(reservedNames);
        }
    }


    public class NameGenerator : INameGenerator
    {
        private readonly string[][] _subNames;
        private readonly Random _random;

        private NameGenerator(string[][] subNames)
        {
            _subNames = subNames;
            _random = new Random();
        }

        public static NameGenerator Create(string[][] subNames)
        {
            return new NameGenerator(subNames);
        }

        public string Generate()
        {
            var name = "";
            for (int i = 0; i < _subNames.Length; i++)
            {
                var inner = _subNames[i];
                var index = _random.Next(inner.Length);

                if (string.IsNullOrEmpty(name))
                    name = inner[index];
                else
                    name += inner[index];
            }
            return name;
        }

        public string GenerateUnique(string[] reservedNames)
        {
            var name = Generate();
            if (Array.Exists(reservedNames, element => element == name))
            {
                GenerateUnique(reservedNames);
            }
            return name;
        }
    }   

    public class UserNameGenerator : AbstractNameGenerator, INameGenerator
    {
        public UserNameGenerator()
        {
            var titles = new string[] { "Mister", "Master", "Miss", "Mrs", "Lady", "Sir", "Madam", "Lord", "Dr", "Elder", "Grandpa", "Grandma", "President", "King", "Queen", "Princess", "Aunt", "Uncle" };
            var firstNames = StringArrayHelper.GetFromSingleColumnedCsv("firstnames.csv");
            var lastNames = StringArrayHelper.GetFromSingleColumnedCsv("lastnames.csv");
            _nameGenerator = NameGenerator.Create(new string[][] { titles, firstNames, lastNames});
        }       
    }
    
    public class CodeNameGenerator : AbstractNameGenerator, INameGenerator
    {
        public CodeNameGenerator()
        {
            var adjectives = StringArrayHelper.GetFromSingleColumnedCsv("adjectives.csv");
            var nouns = StringArrayHelper.GetFromSingleColumnedCsv("nouns.csv");
            _nameGenerator = NameGenerator.Create(new string[][] { adjectives, nouns });
        }        
    }    
}
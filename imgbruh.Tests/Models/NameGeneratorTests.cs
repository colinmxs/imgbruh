using Microsoft.VisualStudio.TestTools.UnitTesting;
using imgbruh.Models.NameGeneration;

namespace imgbruh.Tests.Models
{
    [TestClass]
    public class NameGeneratorTests
    {
        [TestMethod]
        public void Generate_uses_string_from_each_stringarray()
        {
            var array1 = new string[] { "colin" };
            var array2 = new string[] { "theMan" };
            var array3 = new string[] { "smith" };
            var arrays = new string[][] { array1, array2, array3 };
            var nameGenerator = NameGenerator.Create(arrays);

            var result = nameGenerator.Generate();
            Assert.AreEqual("colintheMansmith", result);
        }

        [TestMethod]
        public void GenerateUnique_generates_unique_names()
        {
            var array1 = new string[] {"aa", "bb", "cc", "dd", "ee" };
            var array2 = new string[] { "ff", "gg", "hh", "ii", "jj" };
            var array3 = new string[] { "ll", "mm", "nn", "oo", "pp" };
            var usernameGenerator = NameGenerator.Create(new string[][] { array1, array2, array3});

            var totalCombos = array1.Length * array2.Length * array3.Length;
            var reservedUsernames = new string[totalCombos];
            var loopIndex = totalCombos;

            var numberOfLoops = 0;
            while(loopIndex > 0)
            {
                reservedUsernames[numberOfLoops] = usernameGenerator.GenerateUnique(reservedUsernames);
                loopIndex--;
                numberOfLoops++;
            }

            Assert.AreEqual(totalCombos, numberOfLoops);
        }
    }

    public class TestNameGenerator : AbstractNameGenerator
    {
        public TestNameGenerator()
        {
            
            Assert.AreEqual("colintheMansmith", _nameGenerator.Generate());
        }
    }
}

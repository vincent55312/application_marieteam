using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using client_marieteam;
using Xunit;

namespace TestsApp
{
    [TestClass]
    public class ParsingMethods
    {
        [TestMethod]
        [Theory]
        [DataRow("cookies", "cookies")]
        [DataRow("[image/test]", "")]
        [DataRow("[ image ! $ # 78 ]", "")]
        [DataRow("[image/c", "[image/c")]
        [DataRow("image/c]", "image/c]")]
        [DataRow("[]", "")]
        [DataRow("[", "[")]
        [DataRow("]", "]")]
        [DataRow("", "")]
        [DataRow("[[", "[[")]
        [DataRow("]]", "]]")]
        [DataRow("[[]", "")]
        [DataRow("[[]]", "]")]


        public void ParsingDeleteImageTest(string input, string expected)
        {
            Assert.AreEqual(expected, PDF.ParsingDeleteImage(input));
        }

        [TestMethod]
        [Theory]
        [DataRow("#NEWPAGE", "")]
        [DataRow("#NEW PAGE", "")]
        [DataRow("#NEWPAGE1", "")]
        [DataRow("0#NEWPAGE1#NEWPAGE2#NEWPAGE", "012")]
        public void ParsingPageTest(string input, string expected)
        {
            List<string> list = PDF.ParsingPage(input);
            string res = "";
            foreach (var item in list) res += item;
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        [Theory]
        [DataRow("[image]", "image,")]
        [DataRow("[image][image][image]", "image,image,image,")]
        public void ParsingImageTest(string input, string expected)
        {
            List<string> list = PDF.ParsingImage(input);
            string res = "";
            foreach (var item in list) res += item + ",";
            Assert.AreEqual(expected, res);
        }
    }
}

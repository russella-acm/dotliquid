using System;
using System.Linq;
using DotLiquid.Extensions;
using NUnit.Framework;

namespace DotLiquid.Tests
{
    [TestFixture]
    public class SymbolExtractionTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void HasComplexSymbolsFromNullOrEmptyTemplateText(string templateText)
        {
            var template = Template.Parse(templateText);
            Assert.IsFalse(template.HasComplexSymbols());
        }

        [Test]
        [TestCase("{{name}}", false)]
        [TestCase("{{Person.Name}}", true)]
        [TestCase("{% for item in List %}{{item}}{% endfor %}", true)]
        [TestCase("{% if name = 'Fred' %}Fred{% endif %}", false)]
        [TestCase("{% if points >= 50.00 %}Pass{% endif %}", false)]
        [TestCase("{% if name = 'Fred' %}{{MessageToFred}}{% endif %}", false)]
        [TestCase("{% if Person.Name = 'Fred' %}Fred{% endif %}", true)]
        [TestCase("{% if name = 'Fred' %}{{Message.ToFred}}{% endif %}", true)]
        [TestCase("{% if name = 'Fred' %}{% if age <= 20 %}Young{% else %}Old{% endif %}Fred{% endif %}", false)]
        [TestCase("{% if name = 'Fred' %}{% if Person.Age <= 20 %}Young{% else %}Old{% endif %}Fred{% endif %}", true)]
        [TestCase("{% case name %}{% when 'Fred' %}Fred{% when 'Sally' %}Sally{% else %}Someone else{% endcase %}", false)]
        [TestCase("{% if name = 'Fred' %}Fred{% for item in List %}{{item}}{% endfor %}{% endif %}", true)]
        [TestCase("{% if name = 'Fred' %}{{MessageToFred}}{% for item in List %}{{item}}{% endfor %}{% endif %}", true)]
        [TestCase("{% if Person.Name = 'Fred' %}Fred{% for item in List %}{{item}}{% endfor %}{% endif %}", true)]
        [TestCase("{% if name = 'Fred' %}{{Message.ToFred}}{% endif %}", true)]
        [TestCase("{% if name = 'Fred' %}{% if age <= 20 %}{% for item in List %}{{item}}{% endfor %}Young{% else %}Old{% endif %}Fred{% endif %}", true)]
        [TestCase("{% if name = 'Fred' %}{% if Person.Age <= 20 %}{% for item in List %}{{item}}{% endfor %}Young{% else %}Old{% endif %}Fred{% endif %}", true)]
        [TestCase("{% case name %}{% when 'Fred' %}{% for item in List %}{{item}}{% endfor %}Fred{% when 'Sally' %}Sally{% else %}Someone else{% endcase %}", true)]
        [TestCase("{% case age %}{% when 1 %}{% if cat = 'max' %}{{ maxoldmessage }}{%else%}{{oldmessage}}{% endif %}{% when 2 %}{{anothermessage}}{% else %}{{ youngmessage }}Some text{% endcase %}", false)]
        [TestCase("{% case age %}{% when 1 %}{% if cat = 'max' %}{{ maxoldmessage }}{%else%}{{old.message}}{% endif %}{% when 2 %}{{anothermessage}}{% else %}{{ youngmessage }}Some text{% endcase %}", true)]
        [TestCase("{% case person.age %}{% when 1 %}{% if cat = 'max' %}{{ maxoldmessage }}{%else%}{{oldmessage}}{% endif %}{% when 2 %}{{anothermessage}}{% else %}{{ youngmessage }}Some text{% endcase %}", true)]
        public void HasComplexSymbols(string templateText, bool expectedResult)
        {
            var template = Template.Parse(templateText);
            Assert.AreEqual(expectedResult, template.HasComplexSymbols());
        }

        [Test]
        [TestCase("{{name}}", "name|string")]
        [TestCase("{{Person.Name}}", null)]
        [TestCase("{% for item in List %}{{item}}{% endfor %}", null)]
        [TestCase("{% if name = 'Fred' %}Fred{% endif %}", "name|string")]
        [TestCase("{% if 'Fred' = name %}Fred{% endif %}", "name|string")]
        [TestCase("{% if points >= 50.00 %}Pass{% endif %}", "points|number")]
        [TestCase("{% if 50.00 <= points %}Pass{% endif %}", "points|number")]
        [TestCase("{% if name = 'Fred' %}{{MessageToFred}}{% endif %}", "name|string,MessageToFred|string")]
        [TestCase("{% if Person.Name = 'Fred' %}Fred{% endif %}", null)]
        [TestCase("{% if name = 'Fred' %}{{Message.ToFred}}{% endif %}", null)]
        [TestCase("{% if name = 'Fred' %}{% if age <= 20 %}Young{% else %}Old{% endif %}Fred{% endif %}", "name|string,age|number")]
        [TestCase("{% if name = 'Fred' %}{% if Person.Age <= 20 %}Young{% else %}Old{% endif %}Fred{% endif %}", null)]
        [TestCase("{% case name %}{% when 'Fred' %}Fred{% when 'Sally' %}Sally{% else %}Someone else{% endcase %}", "name|string")]
        [TestCase("{% if name = 'Fred' %}Fred{% for item in List %}{{item}}{% endfor %}{% endif %}", null)]
        [TestCase("{% if name = 'Fred' %}{{MessageToFred}}{% for item in List %}{{item}}{% endfor %}{% endif %}", null)]
        [TestCase("{% if Person.Name = 'Fred' %}Fred{% for item in List %}{{item}}{% endfor %}{% endif %}", null)]
        [TestCase("{% if name = 'Fred' %}{{Message.ToFred}}{% endif %}", null)]
        [TestCase("{% if name = 'Fred' %}{% if age <= 20 %}{% for item in List %}{{item}}{% endfor %}Young{% else %}Old{% endif %}Fred{% endif %}", null)]
        [TestCase("{% if name = 'Fred' %}{% if Person.Age <= 20 %}{% for item in List %}{{item}}{% endfor %}Young{% else %}Old{% endif %}Fred{% endif %}", null)]
        [TestCase("{% case name %}{% when 'Fred' %}{% for item in List %}{{item}}{% endfor %}Fred{% when 'Sally' %}Sally{% else %}Someone else{% endcase %}", null)]
        [TestCase("{% case age %}{% when 1 %}{% if cat = 'max' %}{{ maxoldmessage }}{%else%}{{oldmessage}}{% endif %}{% when 2 %}{{anothermessage}}{% else %}{{ youngmessage }}Some text{% endcase %}", "age|number,cat|string,maxoldmessage|string,oldmessage|string,anothermessage|string,youngmessage|string")]
        [TestCase("{% case age %}{% when 1 %}{% if cat = 'max' %}{{ maxoldmessage }}{%else%}{{old.message}}{% endif %}{% when 2 %}{{anothermessage}}{% else %}{{ youngmessage }}Some text{% endcase %}", null)]
        [TestCase("{% case mark %}{% when 0.00 %}Fail{% when 20.00 %}Unsatisfactory{% when 40.00%}Needs Improvement{% when 60.00%}Satisfactory{% when 80.00%}Good{% when 80.00%}Perfect{% endcase %}", "mark|number")]
        [TestCase("{% case position %}{% when 1 %}One{% when 2 %}Two{% when 3 %}Three{% endcase %}", "position|number")]
        [TestCase("{% case position %}{% when 1 %}One{% when '2' %}Two{% when '3' %}{% endcase %}", "position|string")]
        [TestCase("{% case person.age %}{% when 1 %}{% if cat = 'max' %}{{ maxoldmessage }}{%else%}{{oldmessage}}{% endif %}{% when 2 %}{{anothermessage}}{% else %}{{ youngmessage }}Some text{% endcase %}", null)]
        [TestCase("{% if isRecurring %}Recurring{% endif %}", "isRecurring|boolean")]
        [TestCase("{% if isRecurring = true %}Recurring{% endif %}", "isRecurring|boolean")]
        [TestCase("{% if isRecurring = false%}Recurring{% endif %}", "isRecurring|boolean")]
        [TestCase("{% case isRecurring %}{% when true %}Recurring{% when false %}Not Recurring{% endcase %}", "isRecurring|boolean")]
        [TestCase("{% case position %}{% when 1 %}One{% when 2 %}Two{% when 3 %}Three{% else %}Unknown{% endcase %}", "position|number")]
        [TestCase("{% case position %}{% when 1 %}One{% when 2 %}Two{% when 3 %}Three{% else %}{{UnkownMessage}}{% endcase %}", "position|number,UnkownMessage|string")]
        public void GetSimpleSymbols(string templateText, string expectedSymbols)
        {
            var template = Template.Parse(templateText);
            var symbols = template.GetSimpleSymbols();

            if (expectedSymbols == null)
            {
                Assert.IsNull(symbols);
                return;
            }

            var expectedSymbolsSplit = expectedSymbols.Split(',');
            Assert.AreEqual(expectedSymbolsSplit.Count(), symbols.Count());
            Assert.IsTrue(expectedSymbolsSplit.All(e => symbols.Count(s => s.ToString().Equals(e)) == 1));
        }

        [Test]
        [TestCase("{{...}}")]
        [TestCase("{{.Property}}")]
        [TestCase("{{Property.}}")]
        [TestCase("{{.Property.}}")]
        [TestCase("{{5}}")]
        [TestCase("{{true}}")]
        [TestCase("{{'hello'}}")]
        [TestCase("{{\"hello\"}}")]
        public void GetSimpleSymbolsNonVariableNames(string input)
        {
            var template = Template.Parse(input);
            var symbols = template.GetSimpleSymbols();

            Assert.IsFalse(symbols.Any());
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void GetSimpleSymbolsFromNullOrEmptyTemplateText(string templateText)
        {
            var template = Template.Parse(templateText);
            var symbols = template.GetSimpleSymbols();
            
            Assert.IsNotNull(symbols);
            Assert.IsFalse(symbols.Any());
        }
    }
}

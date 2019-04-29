using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


    class ReputationsPointsAsTextTests : BaseTest
    {
        [Test]
        public void Should_format_correctly()
        {
            Assert.That(ReputationsPointsAsText.Run(0),Is.EqualTo("0 Punkte"));
            Assert.That(ReputationsPointsAsText.Run(999),Is.EqualTo("999 Punkte"));
            Assert.That(ReputationsPointsAsText.Run(1000),Is.EqualTo("1 Tsd. Punkte"));
            Assert.That(ReputationsPointsAsText.Run(1001),Is.EqualTo("1 Tsd. Punkte"));

        }
    }


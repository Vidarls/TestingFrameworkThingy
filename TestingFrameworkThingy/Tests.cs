using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TestingFrameworkThingy
{
 
    public class Tests : TestFixtureBase<Thingy>
    {
        public override Thingy Given()
        {
            	return new Thingy();
        }

        public override Command When(Thingy given)
        {
            return new DoThingyCommand(given);
        }

        [Test]
        public void Thingy_should_get_done()
        {
            Assert.True(Sut.ThingyDone);
        }
    }

    public class DoThingyCommand : Command
    {
        private Thingy _given;

        public DoThingyCommand(Thingy given)
        {
            _given = given;
        }

        public override void Execute()
        {
            _given.DoThingy();
        }

        public override string  ToString()
        {
            return "Doing thingy";
        }
    }

    public class Thingy
    {
        public void DoThingy()
        {
            ThingyDone = true;
        }

        public bool ThingyDone { get; set; }
    }
}

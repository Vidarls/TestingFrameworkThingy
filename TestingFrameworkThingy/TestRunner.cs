using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace TestingFrameworkThingy
{
    public class TestRunner
    {
        public void Run()
        {
            //var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(GetTestTypes);
            var types = GetTestTypes( typeof (Tests).Assembly);
            foreach (var type in types)
            {
                processType(type);
            }
        }

        private void processType(Type type)
        {
            var instance = (IAmTestBase) Activator.CreateInstance(type);
            
            try
            {
                var given = instance.Given();
                instance.AssignSut(given);
                Console.WriteLine(given);
                var when = (Command)instance.DoWhen(given);
                Console.WriteLine(when);
                when.Execute();
                RunAssertions(instance);
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message); 
                return;
            }
            
        }

        private void RunAssertions(IAmTestBase instance)
        {
            var type = instance.GetType();
            var allMethods = type.GetMethods();
            var testMethods =
                allMethods.Where(m => m.GetCustomAttributes(true).Any(a => a.GetType() == typeof (TestAttribute)));
            foreach (var testMethod in testMethods)
            {
                try
                {
                    testMethod.Invoke(instance,new object[0]);

                    Console.Write("[Pass] ");
                }
                catch (Exception)
                {
                    Console.Write("[Fail] ");
                    
                }
                Console.WriteLine(testMethod.Name.Replace("_"," "));
            }
        }

        private IEnumerable<Type> GetTestTypes(Assembly type)
        {
            return type.GetTypes().Where(t => typeof (IAmTestBase).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);
        }
    }

    [TestFixture]
    public abstract class TestFixtureBase<T> : IAmTestBase
    {
        protected T Sut;
        public abstract T Given();
        public object DoWhen(object given)
        {
            return When((T)given);
        }

        public abstract Command When(T given);

        [TestFixtureSetUp]
        public void Setup()
        {
            Sut = Given();
            var when = When(Sut);
            when.Execute();
        }

        public void AssignSut(object sut)
        {
            Sut = (T) sut;
        }

        object IAmTestBase.Given()
        {
            return Given();
        }
    }

    internal interface IAmTestBase
    {
        object Given();
        object DoWhen(object given);
        void Setup();
        void AssignSut(object sut);
    }

    public abstract class Command
    {
        public abstract void Execute();
    }
}

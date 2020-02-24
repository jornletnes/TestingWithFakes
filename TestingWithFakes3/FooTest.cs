using Moq;
using NUnit.Framework;

namespace TestingWithFakes3
{
    class FooTest
    {
        [Test]
        public void TestDoAThing()
        {
            //Arrange
            Foo foo = new Foo();
            int expected = 42;
            Mock<IBar> fakeBar = new Mock<IBar>();
            fakeBar.Setup(f => f.DoTheOtherThing()).Returns(expected);

            //Act
            int result = foo.DoAThing(fakeBar.Object);

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
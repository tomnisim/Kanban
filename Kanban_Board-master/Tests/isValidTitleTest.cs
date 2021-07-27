using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using NUnit.Framework;

namespace Tests
{

    public class isValidTitleTest
    {
        Task task;

        [SetUp]
        public void Setup()
        {
            task = new Task();
        }


        [TestCase("tom")]
        [TestCase("123465798")]
        [TestCase("tom134543464565")]
        [TestCase("tom!@#$#^^*&(*")]
        [TestCase("אבאבבאבאבאבאב")]
        [TestCase("חלךכמלךבדחלך112314134")]
        [TestCase("tom tom tom tom")]
        [TestCase("t")]
        [TestCase("12345678912345678912345678912345678912345678912345")]

        public void TestGoodTitle(string title)
        {
            //arrange

            // act
            bool ans = task.isValidTitle(title);
            // assert
            Assert.AreEqual(true, ans);
        }
        [Test]
        [TestCase("123456789123456789123456789123456789123456718912345")]
        [TestCase("tomitfakjldajfkldafjkldsgjlksdhtrvjsdklfjfkljkldfhdjkldfhkjl4378957#@$%$&*jklfdsjklsdfjklsfkjljkldkjlgkjlgjkgkjlgkjlgjkldjkldkjldfjkldjkldjkldjkldg")]
        [TestCase("toaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaam")]
        public void TestToLongTitle(string title)
        {
            //arrange
            // act
            var ExceptionMessage = Assert.Throws<Exception>(() => task.isValidTitle(title));
            // assert
            Assert.That(ExceptionMessage.Message, Is.EqualTo("the title must have maximun 50 characters"));
        }

        [Test]
        [TestCase("")]
        public void TestEmptyTitle(string title)
        {
            //arrange
            // act
            var ExceptionMessage = Assert.Throws<Exception>(() => task.isValidTitle(title));
            // assert
            Assert.That(ExceptionMessage.Message, Is.EqualTo("the title can not be empty"));
        }

        [Test]

        public void TestNullTitle()
        {
            //arrange
            string title = null;
            // act
            var ExceptionMessage = Assert.Throws<Exception>(() => task.isValidTitle(title));
            // assert
            Assert.That(ExceptionMessage.Message, Is.EqualTo("the title can not be null"));
        }
    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntroSE.Kanban.Backend.BusinessLayer;

namespace Tests
{
    public class isValidDueDateTest
    {
        //DateTime is not a nullabe object, therefore if should be tested
        
        [TestCase(2022,12,12,2021,12,12)]
        [TestCase(3022, 12, 12, 2021, 12, 12)]
        [TestCase(2022, 12, 12, 1984, 12, 12)]
        [TestCase(2022, 12, 12, 2022, 12, 12)]
        [TestCase(2026, 1, 1, 2022, 1, 1)]

        public void TestDueDate(int dueDateYear, int dueDateMonth, int dueDateDay, int creationTimeYear, int creationTimeMonth, int creationTimeDay)
        {
            //arrange
            DateTime creationTime = new DateTime(creationTimeYear, creationTimeMonth, creationTimeDay);
            DateTime dueDate = new DateTime(dueDateYear, dueDateMonth, dueDateDay);
            Task task = new Task(creationTime);
            //act
            bool ans = task.isValidDueDate(dueDate, creationTime);
            //assert
            Assert.IsTrue(ans);

        }

        [TestCase(2020,12,12,2021,12,12)]
        [TestCase(1999, 12, 12, 2021, 12, 12)]
        [TestCase(2022, 12, 12, 2056, 3, 5)]
        [TestCase(2021, 12, 12, 2021, 12, 13)]
        [TestCase(1999, 12, 12, 1999, 12, 15)]
        [TestCase(2010, 12, 12, 2011, 3, 5)]
        public void TestDueDateFail(int dueDateYear, int dueDateMonth, int dueDateDay, int creationTimeYear, int creationTimeMonth, int creationTimeDay)
        {
            //arrange
            Task task = new Task(new DateTime(creationTimeYear, creationTimeMonth, creationTimeDay));
            //act
            var ExceptionMessage = Assert.Throws<Exception>(() => task.isValidDueDate(new DateTime(dueDateYear, dueDateMonth, dueDateDay), new DateTime(creationTimeYear, creationTimeMonth, creationTimeDay)));
            // assert
            Assert.That(ExceptionMessage.Message, Is.EqualTo("invalid due date"));
        }
       
    }
}

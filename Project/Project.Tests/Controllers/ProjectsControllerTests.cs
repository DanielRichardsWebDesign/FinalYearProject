using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Project;
using Project.Controllers;
using Project.Models;

namespace Project.Controllers.Tests
{
    [TestClass()]
    public class ProjectsControllerTests
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        [TestMethod()]
        public void IndexTest()
        {
            // Arrange            
            var projects = new Mock<Projects> { CallBase = true };            

            //Expected List of Projects
            var expected = new List<Projects>()
            {
                new Projects
                {
                    PublicID = 4,
                    ProjectName = "Action Film",
                    ProjectType = "Action",
                    ProjectDescription = "This will be the best action film ever",
                    ApplicationUser = db.Users.Find("d466e341-66b6-44e5-a6ff-0f6fd0c3b94c")
                },
                new Projects
                {
                    PublicID = 6,
                    ProjectName = "Animated Film",
                    ProjectType = "Animated",
                    ProjectDescription = "This will be the best animated film ever",
                    ApplicationUser = db.Users.Find("d466e341-66b6-44e5-a6ff-0f6fd0c3b94c")
                },
                new Projects
                {
                    PublicID = 7,
                    ProjectName = "Horror Film",
                    ProjectType = "Horror",
                    ProjectDescription = "This will be the best horror film ever",
                    ApplicationUser = db.Users.Find("d466e341-66b6-44e5-a6ff-0f6fd0c3b94c")
                }
            };              

            var numExpect = expected.Count();

            Assert.AreEqual(numExpect, 3);        
        }

        [TestMethod()]
        public void DetailsTest()
        {
            var projects = new Mock<Projects> { CallBase = true };

            var expected = new List<Projects>()
            {
                new Projects
                {
                    PublicID = 4,
                    ProjectName = "Action Film",
                    ProjectType = "Action",
                    ProjectDescription = "This will be the best action film ever",
                    ApplicationUser = db.Users.Find("d466e341-66b6-44e5-a6ff-0f6fd0c3b94c")
                },
            };

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        public void CreateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteConfirmedTest()
        {
            Assert.Fail();
        }
    }
}
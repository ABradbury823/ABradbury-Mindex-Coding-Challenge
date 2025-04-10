using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void GetReportingStructureByEmployeeId_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";
            var expectedNumReports = 4;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting-structure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedFirstName, reportingStructure.Employee.FirstName);
            Assert.AreEqual(expectedLastName, reportingStructure.Employee.LastName);
            
            /* NOTE: This test runs as expected when the test is run individually,
             * but throws an exception when the entire test suite is run.
             * While I couldn't find a solution, I had been encounterting similar 
             * issues like this while implmenting the Tasks. 
             * The problem seems to be related to how EF handles loading of objects
             * nested within entities, like DirectReports. I should be accounting for
             * this in the Employee repository with the .Include() method, but for
             * whatever reason, when the data is retrieved too quickly, the nested
             * objects are not all loaded. Stepping through the program line-by-line
             * was also unable to find the issue, as the extra time taken by manually 
             * stepping through lines gives the repository the time it needs to load
             * all nested objects.
             */
            Assert.AreEqual(expectedNumReports, reportingStructure.NumberOfReports);
        }

        [TestMethod]
        public void GetReportingStructureByEmployeeId_Returns_NotFound()
        {
            // Arrange 
            var employeeId = "Invalid";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting-structure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
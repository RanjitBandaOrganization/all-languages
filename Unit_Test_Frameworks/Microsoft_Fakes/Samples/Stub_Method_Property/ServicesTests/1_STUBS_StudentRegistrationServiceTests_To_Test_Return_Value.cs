using DataAccess;
using DataAccess.Fakes;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

namespace ServicesTests
{
    //These tests will test the return values when method is called/property is called
    // I.E.  To provide values to the properties
    //(1st for method, 2nd for property)
    [TestClass]
    public class STUBS_StudentRegistrationServiceTests_To_Test_Return_Value_1
    {
        //This is testing the interaction
        [TestMethod]
        public void FindStudent_FindByIdInt32_STUB_TO_TEST_METHOD_RETURN_VALUE()
        {
            Student student = new Student() { FirstName = "TestFirstName", Id = 123, LastName = "TestLastName" }; 
            //Stub for student repository interface 
            // return value for the FindById method is set in this interface
            IStudentRepository stubStudentRepository = new StubIStudentRepository
            {
                FindByIdInt32 = (x) => { return student;  }
            };

            StudentRegistrationService studentRegistrationService = 
                    new StudentRegistrationService(stubStudentRepository);

            //ACT
            Student returnedStudent = studentRegistrationService.FindStudent(123);
            //ASSERT
            Assert.AreEqual(student, returnedStudent, "Returned Student is not the same one =~ stub didn't work for method in returning value");
        }

        [TestMethod]
        public void FindStudent_IsStudentSavedSet_STUB_TO_TEST_PROPERTY_RETURN_VALUE()
        {
            //Stub for student repository interface 
            // return value for the IsStudentSavedGet property is set in this interface
            IStudentRepository stubStudentRepository = new StubIStudentRepository()
            {
                IsStudentSavedGet = () => { return true; }
            };
            
            StudentRegistrationService studentRegistrationService =
                    new StudentRegistrationService(stubStudentRepository);

            //ACT
            bool testIsStudentSavedGetStub = studentRegistrationService.IsStudentSavedInRepository;
            //ASSERT
            Assert.IsTrue(testIsStudentSavedGetStub, "stub didn't work for property in returning value");
        }
    }
}

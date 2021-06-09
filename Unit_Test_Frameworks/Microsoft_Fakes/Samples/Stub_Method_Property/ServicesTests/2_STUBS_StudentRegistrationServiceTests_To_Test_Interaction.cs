using DataAccess;
using DataAccess.Fakes;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

namespace ServicesTests
{
    //These tests will test the interaction (1st for method, 2nd for property)
    [TestClass]
    public class STUBS_StudentRegistrationServiceTests_To_Test_Interaction_2
    {
        //This is testing the interaction
        [TestMethod]
        public void RegisterNewStudent_SavesTheStudent_STUB_TO_TEST_METHOD_INTERACTION()
        {
            bool wasSaveCalled = false; 
            //Stub for student repository interface 
            // fake implementation for Save method in this interface
            IStudentRepository stubStudentRepository = new StubIStudentRepository
            {
                SaveStudent = (x) => { wasSaveCalled = true; }
            };

            StudentRegistrationService studentRegistrationService = 
                    new StudentRegistrationService(stubStudentRepository);

            Student student = new Student();
            //ACT
            studentRegistrationService.RegisterNewStudent(student);
            //ASSERT
            Assert.IsTrue(wasSaveCalled);
        }

        [TestMethod]
        public void RegisterNewStudent_IsStudentSavedSet_STUB_TO_TEST_PROPERTY_INTERACTION()
        {
            bool wasIsStudentSavedPropertySet = false;

            //Stub for student repository interface 
            // fake implementation for the property below
            IStudentRepository stubStudentRepository = new StubIStudentRepository()
            {
                IsStudentSavedSetBoolean = (value) => { wasIsStudentSavedPropertySet = true; }
            };
            
            StudentRegistrationService studentRegistrationService =
                    new StudentRegistrationService(stubStudentRepository);

            Student student = new Student();
            //ACT
            studentRegistrationService.RegisterNewStudent(student);
            //ASSERT
            Assert.IsTrue(wasIsStudentSavedPropertySet);
        }


        //TODO - To return values when method is called

    }
}

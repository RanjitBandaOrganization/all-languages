using DataAccess;
using Domain;

namespace Services
{
    public class StudentRegistrationService
    {
        private readonly IStudentRepository _studentRepository;

        public bool IsStudentSavedInRepository
        {
            get
            {
                return _studentRepository.IsStudentSaved;
            }
            set
            {
                _studentRepository.IsStudentSaved = value;
            }
        }

        public StudentRegistrationService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void RegisterNewStudent(Student student)
        {
            _studentRepository.IsStudentSaved = true;
            _studentRepository.Save(student);
        }

        public Student FindStudent(int id)
        {
            Student student = _studentRepository.FindById(id);
            return student;
        }
    }
}
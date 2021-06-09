using DataAccess;
using Domain;

namespace Services
{
    public class StudentRegistrationPresenter
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentView _studentView;

        public StudentRegistrationPresenter(IStudentView studentView, IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
            _studentView = studentView;
        }

        public void RegisterNewStudent(Student student)
        {
            if (student == null)
            {
                _studentView.WasStudentSaved = false;
            }
            else
            {
                _studentRepository.Save(student);
                _studentView.WasStudentSaved = true;
            }
        }
    }
}
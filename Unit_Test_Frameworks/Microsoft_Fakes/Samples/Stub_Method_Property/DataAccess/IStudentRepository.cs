using Domain;

namespace DataAccess
{
    public interface IStudentRepository
    {
        bool IsStudentSaved { get; set; }
        Student FindById(int id);
        void Save(Student student);
    }
}
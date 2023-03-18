using EFCoreAcademy.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAcademy;
//48
public class StudentRepository : IStudentRepository
{
    private ApplicationDbContext DbContext { get; }
    public StudentRepository(ApplicationDbContext dbContext) 
    {
        DbContext = dbContext;
    }
    public async Task<int> CreateStudentAsync(Student student)
    {
        DbContext.Students.Add(student);
        await DbContext.SaveChangesAsync();
        //EFCore assigns Id to the added student
        return student.Id;
    }

    public async Task DeleteStudentAsync(int studentId)
    {
        var student = await DbContext.Students.FindAsync(studentId);
        if (student != null)
        {
            DbContext.Students.Remove(student);
            await DbContext.SaveChangesAsync();
        }
        throw new InvalidOperationException("Id not found");
    }

    public async Task<Student?> GetStudentByIdAsync(int studentId)
    {
        return await DbContext.Students.FindAsync(studentId);
    }

    public async Task<List<Student>> GetStudentsAsync()
    {
        return await DbContext.Students.ToListAsync();
    }

    public async Task UpdateStudentAsync(Student student)
    {
        DbContext.Students.Attach(student);
        DbContext.Entry(student).State = EntityState.Modified;
        await DbContext.SaveChangesAsync();
    }
}

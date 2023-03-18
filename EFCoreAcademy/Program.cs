//43)
//Installing EF Core Tools: dotnet tool install --global dotnet-ef
//Creating migrations: dotnet ef migrations add InitialMigration
// https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli
// 44)
// dotnet ef dbcontext scaffold "filename=EFCoreAcademy.db" Microsoft.EntityFrameworkCore.Sqlite
// Instead of filename you could specify the connection string if you are using another dbms (for example Oracle)
// https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli
// https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-strings
using EFCoreAcademy;
using EFCoreAcademy.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite().Options;
var dbContext = new ApplicationDbContext(options);

dbContext.Database.Migrate();
//45
//ProcessInsert();
//46
ProcessDelete();
ProcessInsert();
ProcessSelect();
//47
ProcessUpdate();
//49
ProcessRepository();

//45
void ProcessInsert()
{
    //46
    dbContext = new ApplicationDbContext(options);

    var address = new Address { City = "Hamburg", Street = "Demostreet", Zip = "24225", HouseNumber = 1 };
    var professor = new Professor { FirstName = "Jonathan", LastName = "Schoolman", Address = address };
    var student1 = new Student { FirstName = "John", LastName = "Doe", Address= address };
    var student2 = new Student { FirstName = "Maria", LastName = "Maker", Address = address };
    var class1 = new Class { Professor = professor, Students = new List<Student> { student1, student2 }, Title = "IT" };

    dbContext.Addresses.Add(address);
    dbContext.Classes.Add(class1);
    dbContext.Students.Add(student1);
    dbContext.Students.Add(student2);
    dbContext.Professors.Add(professor);

    dbContext.SaveChanges();
    dbContext.Dispose();
}

//46
void ProcessDelete()
{
    var professors = dbContext.Professors.ToList();
    var classes = dbContext.Classes.ToList();
    var addresses = dbContext.Addresses.ToList();
    var students = dbContext.Students.ToList();

    dbContext.RemoveRange(professors);
    dbContext.RemoveRange(classes);
    dbContext.RemoveRange(addresses);
    dbContext.RemoveRange(students);

    dbContext.SaveChanges();
    dbContext.Dispose();
}

void ProcessSelect()
{
    dbContext = new ApplicationDbContext(options);
    //var professor = dbContext.Professors.Single(p => p.FirstName=="Jonathan");
    //dbContext.Dispose();

    //Address and Classes are navigation properties, here they are null
    //EFCore doesn't load the standard related entities(affects performance, expensive using joins)
    //EFCore lets you decide which relations you want to load
    var professor = dbContext.Professors.Include(p => p.Address).Single(p => p.FirstName == "Jonathan");
    var student = dbContext.Students.Include(s=> s.Classes).Where(s=> s.FirstName == "Maria").ToList();
    dbContext.Dispose();

    //47
    //EFCore keeps track of changes that you do to your entities
    //eg: change FirstName of var student, EFCore will keep track of this, SaveChanges() persists these changes to db
}

//47
void ProcessUpdate()
{
    dbContext = new ApplicationDbContext(options);
    //var student = dbContext.Students.First(); //student is already tracked , we use dbContext and dbSet Students to track student
    //student.FirstName = "Tim";
    //dbContext.SaveChanges();
    //dbContext.Dispose();

    //dbContext = new ApplicationDbContext(options);
    //student = dbContext.Students.First();
    //Console.ReadLine();

    ////changes to navigation properties like classes, address
    //// Standard behaviour of ef core is to use change tracking
    var student = dbContext.Students.Include(s => s.Classes).First(); //student is already tracked , we use dbContext and dbSet Students to track student
    student.FirstName = "Tim";
    student.Classes = new List<Class>();
    dbContext.SaveChanges();

    dbContext.Dispose();
    dbContext = new ApplicationDbContext(options);

    //student = dbContext.Students.First();
    //Console.ReadLine();

    //AsNoTracking
    student = dbContext.Students.AsNoTracking().Include(s => s.Classes).First();
    student.FirstName = "John";
    dbContext.SaveChanges();
    dbContext.Dispose();

    dbContext = new ApplicationDbContext(options);
    student = dbContext.Students.First();
    dbContext.Dispose(); //my code
    //Console.ReadLine();

    //Also Attach an entity to our EFCore dbContext and set the state ourselves
    dbContext = new ApplicationDbContext(options);
    student = dbContext.Students.AsNoTracking().First();
    student.FirstName = "John";
    dbContext.Students.Entry(student).State = EntityState.Modified;
    dbContext.SaveChanges();
    dbContext.Dispose();

    dbContext = new ApplicationDbContext(options);
    student = dbContext.Students.First();
    //Console.ReadLine();

    //Attach entity eg:2
    dbContext = new ApplicationDbContext(options);
    var studentUntracked = new Student() { Id = student.Id, FirstName= "Dennis", LastName = "Luckman"};
    dbContext.Students.Attach(studentUntracked);
    dbContext.Students.Entry(studentUntracked).State = EntityState.Modified;
    dbContext.SaveChanges();
    dbContext.Dispose();

    dbContext = new ApplicationDbContext(options);
    student = dbContext.Students.First();
    dbContext.Dispose();
    //Console.ReadLine();
}

//49
async void ProcessRepository()
{
    dbContext = new ApplicationDbContext(options);
    var repository = new GenericRepository<Student>(dbContext);

    //select
    var students = await repository.GetAsync(null, null);
    var student = await repository.GetByIdAsync(students.First().Id);

    //Includes
    student = await repository.GetByIdAsync(student.Id, (student)=> student.Address, (student) => student.Classes);

    //Filters
    Expression<Func<Student, bool>> filter = (student) => student.FirstName == "Maria";
    students = await repository.GetFilteredAsync(new[] { filter }, null, null);
    Console.ReadLine();
}


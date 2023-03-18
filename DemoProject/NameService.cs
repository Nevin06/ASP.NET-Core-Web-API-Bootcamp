//51 Dependency Injection
namespace DemoProject;

public class NameService
{
    public string[] Names { get; }
    = new[]
    {
        "John",
        "Jim",
        "Maria",
        "Yana"
    };
    public SomeOtherService SomeOtherService { get; }

    private Random random= new Random();
    public string GetRandomName()
    {
        return Names[random.Next(Names.Length)];
    }

    public NameService(SomeOtherService someOtherService) 
    {
        SomeOtherService = someOtherService;
    }
}

namespace MinimalAPICheatsheet;
//84 routing using instance
public class PersonHandler
{
    public Person HandleGet()
    {
        return new Person("John", "Doe");
    }
    public Person HandleGetById(int id)
    {
        return new Person("Maria", "Maker");
    }
}

public record Person(string FirstName, string LastName);

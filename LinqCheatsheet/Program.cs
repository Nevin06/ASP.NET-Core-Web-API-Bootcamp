using LinqCheatsheet;

var lawyers = new[]
{
    new Lawyer()
    {
        FirstName = "John",
        LastName = "Doe"
    },
    new Lawyer()
    {
        FirstName = "Maria",
        LastName = "Maker"
    }
};

var clients = new[]
{
    new Client()
    {
        FirstName = "Tim",
        LastName = "Funny"
    },
    new Client()
    {
        FirstName = "Jim",
        LastName = "Decker"
    },
    new Client()
    {
        FirstName = "Yana",
        LastName = "Cat"
    }
};

var cases = new[]
{
    new Case()
    {
        Title = "Car Accident",
        AmountInDispute = 10000,
        CaseType = CaseType.Commercial,
        Client = clients[0],
        Lawyer = lawyers[0]
    },
    new Case()
    {
        Title = "Molding Flat",
        AmountInDispute = 65000,
        CaseType = CaseType.ProBono,
        Client = clients[0],
        Lawyer = lawyers[0]
    },
    new Case()
    {
        Title = "Death Threat",
        AmountInDispute = 15000,
        CaseType = CaseType.Commercial,
        Client = clients[1],
        Lawyer = lawyers[1]
    },
    new Case()
    {
        Title = "Robbery",
        AmountInDispute = 1500,
        CaseType = CaseType.Commercial,
        Client = clients[2],
        Lawyer = lawyers[1]
    },
};

// Assign cases to our client and our lawyer using first LINQ method 29) WHERE
foreach (Lawyer lawyer in lawyers)
    lawyer.Cases = cases.Where(c => c.Lawyer == lawyer).ToList();

foreach (Client client in clients)
    client.Cases = cases.Where(c => c.Client == client).ToList();

// 30) First and Single ,First entry in a list using a condition
//var workingFirstExample = lawyers.First(l => l.FirstName == "Joe");

try
{
    var firstExceptionExample = lawyers.First(l => l.FirstName == "Joh");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex);
}

//FirstorDefault returns value for the specified data type, if no matching element is found
//for classes thats null and for value types thats the default value, for eg : int it is 0
var firstOrDefaultExample = lawyers.FirstOrDefault(l => l.FirstName == "John");

//Single works like first, but ensures only a single element matches thes specified condition
var workingSingExample = lawyers.Single(l => l.FirstName == "John");

//SingleorDefault returns value for the specified data type, if no matching element is found
//for classes thats null and for value types thats the default value, for eg : int it is 0
//Everytjing else works just like Single
var workingSingOrDefaultExample = lawyers.SingleOrDefault(l => l.FirstName == "John");

// 31) Any returns true if any element satisfies a certain condition
// All returns true only if all elements in a collection satisfies a certain condition
var proBonoLawyers = lawyers.Where(l => l.Cases.Any(c => c.CaseType == CaseType.ProBono));
var commercialOnlyLawyers = lawyers.Where(l => l.Cases.All(c => c.CaseType == CaseType.Commercial));

// Working with numbers
var sumOfAmountInDispute = cases.Sum(c => c.AmountInDispute);
var avgOfAmountInDispute = cases.Average(c => c.AmountInDispute);
var maxOfAmountInDispute = cases.Max(c => c.AmountInDispute);
var minOfAmountInDispute = cases.Min(c => c.AmountInDispute);

//OrderBy
var lawyersByAmountInDisputeAsc = lawyers.OrderBy(l => l.Cases.Sum(c => c.AmountInDispute));
var lawyersByAmountInDisputeDsc = lawyers.OrderByDescending(l => l.Cases.Sum(c => c.AmountInDispute));

//Select
var caseTitles = cases.Select(c => c.Title).ToList();
var caseTitle = cases.Select(c => c.Title);
var lawyerNames = lawyers.Select(l => l.FirstName + ", " + l.LastName);
//Select returns list of lists
var casesPerlawyer = lawyers.Select(l => l.Cases);
//SelectMany returns a flat list
var casesPerLawyerFlat = lawyers.SelectMany(l => l.Cases);

//Fluent ,chaining Linq queries
var caseTitlesOfCommercialOnlyLawyers = lawyers
    .Where(l => l.Cases.All(c => c.CaseType == CaseType.Commercial))
    .SelectMany(l => l.Cases)
    .Select(c => c.Title);

//LINQ - Challenge
//1
//var commercialOnlylawyersOrderByMoneyInDispute = lawyers
//    .Where(l => l.Cases.All(c => c.CaseType == CaseType.Commercial))
//    .OrderBy(l => l.Cases.Sum(l => l.AmountInDispute));

var lawyersOrderByMoneyInDisputeCommercialOnly = lawyers
    .OrderBy(l => l.Cases
    .Where(c => c.CaseType == CaseType.Commercial)
    .Sum(c => c.AmountInDispute));


//2
var allCasesFromClients = clients.Select(c => c.Cases);

//3
var allCasesFromClientsasFlatennedList = clients.SelectMany(c => c.Cases);

//4
var listOfStrings = cases.Select(c => c.Lawyer.FirstName + ", " + c.Lawyer.LastName + ", " + c.Client.FirstName + ", " + c.Client.LastName)
    ;


Console.ReadLine();

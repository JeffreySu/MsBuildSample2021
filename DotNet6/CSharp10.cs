using System;

namespace DotNet6
{
    public class CSharp10
    {
    }

    public record /*struct*/ Person
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public string ShowMyName() => $"My name is {FirstName} {LastName}.";

    }
}

// for Program.cs
//app.MapGet("/Name", (Func<string>)(() =>
//{
//    var person = new DotNet6.Person()
//    {
//        FirstName = "Jeffrey",
//        LastName = "Su"
//    };
//    return $"{person.ShowMyName()}";

//    //var person2 = person with { LastName = "Wang" };
//    //return $"{person.ShowMyName()} ({person.GetHashCode()})  ->  {person2.ShowMyName()} ({person2.GetHashCode()})";
//}));
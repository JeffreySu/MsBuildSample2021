var app2 = WebApplication.Create(args);
app2.MapGet("/", (Func<string>)(() => "Hello World! ~~~~~~~~~~~"));

app2.MapGet("/Name", (Func<string>)(() =>
{
    var person = new DotNet6.Person()
    {
        FirstName = "Jeffrey",
        LastName = "Su"
    };

    //return $"{person.ShowMyName()}";

    var person2 = person with { LastName = "Wang" };
    return $"{person.ShowMyName()} ({person.GetHashCode()})  ->  {person2.ShowMyName()} ({person2.GetHashCode()})";
}));

app2.Run();
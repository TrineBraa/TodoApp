using System.Data.SqlClient;
using Dapper;
using TodoApp.Model;

//Dette er backend
var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var app = builder.Build();
const string connStr = @"Data Source=DESKTOP-1HV37H6\SQLEXPRESS;Initial Catalog=Todo;Integrated Security=True;Connect Timeout=30;Encrypt=False";
//connStr er den tråden som lar deg koble deg til SQL DB.

//Dette er en API uten backend foreløpig. Denne DB variabelen er DB for nå.
//var inMemoryDB = new List<TodoItem>
//{
//    new TodoItem("Handle matvarer"),
//    new TodoItem("Vaske gulv") {Done= new DateTime(2020,11,4)},
//    new TodoItem("Bade hunden"), 
//    new TodoItem("Vaske klær"),
//};
//Local DB trengs ikke når det skrives om til SQL, da vill DB informasjonen ligge i SQL.


//Map er et nøkkel ord for å skape en kobling mellom URL fra frontend og backend. nøkkelordet Get, er for å få informasjonen fra backend.
//Du kan ha flere Maps til forskjellige eller samme URL, kodeord er Post(Lage ny data), Put(endre og oppdatere data), Get(LEse data) og Delete(slette data) 
app.MapGet("/todo"/*Denne URLen kan du velge selv*, knytter URL kode til C# kode gjør at frontend kan snakke med API*/, async () =>
{
    var connection = new SqlConnection(connStr); //Setter opp en kobling til SQL DB.
    const string SQL = "SELECT Id, Text, Done FROM Todo"; //Du skriver hva du ønsker fra DB.
    var todoItems = await connection.QueryAsync<TodoItem>(SQL); //Denne venter på at du får en kobling før den ber om SQL koden.
    return todoItems; //Her får du tilbake svaret fra SQL som returneres i funksjonen.
});
app.MapPost("/todo", async (TodoItem todoItem) => //Må ta imot et objekt. dataen fra javascript MÅ matche modelen.
{
    //inMemoryDB.Add(todoItem); Ikke lengre en lokal DB.

    var connection = new SqlConnection(connStr); //Denne setter igjen en kobling til SQL.
    const string SQL = "INSERT INTO Todo (Id, Text) VALUES (@Id, @Text)"; //SQL code knyttet til det å sette inn noe nytt i DB.
    var rowsAffected = await connection.ExecuteAsync(SQL, todoItem); //Må sende inn et objekt som inneholder de feltene som du ber om i SQL.
    return rowsAffected;

});
app.MapPut("/todo", async (TodoItem todoItem) => //Her tas det imot en id for å gjøre endringer til den valgte oppgaven med den id.
{
    //Kode for Lokal DB i koden.
    //var todoItem = inMemoryDB.SingleOrDefault(ti => ti.Id == id);
    //todoItem.Done = DateTime.Today;

    //Koden for SQL DB
    todoItem.Done = DateTime.Today;
    var connection = new SqlConnection(connStr); //Denne setter igjen en kobling til SQL.
    const string SQL = "UPDATE Todo SET Done = @Done WHERE Id = @id"; //SQL code knyttet til det å sette Done i DB.
    var rowsAffected = await connection.ExecuteAsync(SQL, todoItem); //Må sende inn et objekt som inneholder de feltene som du ber om i SQL.
    return rowsAffected;
});

app.MapDelete("/todo/{id}", async (Guid id) => //Her tas det imot en id for å slette oppgaver med matchende id.
{
    //Kode for lokal DB
    //inMemoryDB.RemoveAll(ti => ti.Id == id);


    //Kode for SQL DB

    var connection = new SqlConnection(connStr); //Denne setter igjen en kobling til SQL.
    const string SQL = "DELETE FROM Todo WHERE Id = @Id"; //SQL code knyttet til det å slette noe i DB med den Id.
    var rowsAffected = await connection.ExecuteAsync(SQL, new {Id=id}); //Her brukes et anonymt objekt hvor Id er lik id.
    return rowsAffected;


});

app.UseStaticFiles();
app.Run();


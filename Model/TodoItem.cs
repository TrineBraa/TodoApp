namespace TodoApp.Model
{
    //Dette er en View Model
    //Denne har ikke så mye logikk, den har bare verdier (Props) du kan lese og skrive. Rene data objekter
    public class TodoItem
    {
        public Guid Id { get; set; } //GUID er Global universal identification, denne lager en random unik ID, 128 bits struktur. blir aldri generert lik kode.
        public string Text { get; set; }
        public DateTime? Done { get; set; } //Ved og sette et ? gjør du denne variabelen nullable, så den kan være null.

        public TodoItem(string text) : this() // Denne ctor starter ved å gjøre ctor under før den gjør det som står i denne. Så objektet får en ID uansett.
        {
            Text = text;
        }

        public TodoItem()
        {
            Id = Guid.NewGuid();
            //Kan være kritisk og ha en tom constructor så rammeverket skal få et objekt av dette fra javaScript.
        }
    }
}

public class Persona
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string Email { get; set; }
    public int Età { get; set; }

    public Persona(string nome, string cognome)
    {
        this.Nome = nome;
        this.Cognome = cognome;
    }
}
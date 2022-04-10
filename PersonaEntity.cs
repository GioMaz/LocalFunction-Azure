using System;
using Azure.Data.Tables;

class PersonaEntity: ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public Azure.ETag ETag { get; set; }
    public string Email { get; set; }
    public int Età { get; set; }

    public PersonaEntity(Persona p)
    {
        this.PartitionKey = p.Cognome;
        this.RowKey = p.Nome;
        this.Email = p.Email;
        this.Età = p.Età;
    }
    
    public PersonaEntity()
    {
    }

    public Persona ToPersona() {
        Persona p = new Persona(PartitionKey, RowKey);
        p.Email = this.Email;
        p.Età = this.Età;
        return p;
    }
}
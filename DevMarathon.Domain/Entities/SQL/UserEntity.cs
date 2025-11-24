using System.ComponentModel.DataAnnotations.Schema;

namespace DevMarathon.Domain.Entities.SQL;

public class UserEntity:EntityBase
{
    [Column("phone_number")]
    public string PhoneNumber{set;get;}
    [Column("verified")]
    public bool Verified{set;get;}
    
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Univali.Api.Entities;

public class Address
{
    ///////////////
    // ID
    ///////////////
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    ///////////////
    // Street
    ///////////////
    [Required]
    [MaxLength(50)]
    public string Street {get; set;} = string.Empty;

    ///////////////
    // City
    ///////////////
    [Required]
    [MaxLength(50)]
    public string City {get; set;} = string.Empty;

    ///////////////
    // Customer
    ///////////////
    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }
    public int CustomerId { get; set; }
}


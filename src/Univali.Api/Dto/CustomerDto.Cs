namespace Univali.Api.Dto;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name {get; set;} = string.Empty;
    public string Cpf {get; set;} = string.Empty;
}

public class CustomerForUpdateDto
{
    public string Name {get; set;} = string.Empty;
    public string Cpf {get; set;} = string.Empty;
}

public class CustomerForCreateDto
{
    public string Name {get; set;} = string.Empty;
    public string Cpf {get; set;} = string.Empty;
}

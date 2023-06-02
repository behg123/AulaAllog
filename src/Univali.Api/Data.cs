using Univali.Api.Controllers;
using Univali.Api.Entities;

namespace Univali.Api
{
    //Classes sem parametro
    public class Data
    {
        public List<Customer> Customers {get; set;}
        private static Data instance = new Data();

        private Data()
        {
            Customers = new List<Customer>
            {
                new Customer{
                    Id = 1,
                    Name = "Joao",
                    Cpf = "128975641",
                    Addresses = new List<Address>(){
                        new Address(){
                            Id = 1,
                            Street = "Verão",
                            City = "Elvira"
                        },
                        new Address(){
                            Id = 2,
                            Street = "Joao Sacavem",
                            City = "Navegantes"
                        }
                    }
                },
                new Customer{
                    Id = 2,
                    Name = "Renan",
                    Cpf = "751478625",
                    Addresses = new List<Address>(){
                        new Address(){
                            Id = 3,
                            Street = "Mario Dias",
                            City = "Blumenau"
                        }
                    }
                }
            };

        }

        public static Data instanceAcess(){
            if(instance == null)
            {
                instance = new Data();
            } 
            return instance;
        }
        

    }


}
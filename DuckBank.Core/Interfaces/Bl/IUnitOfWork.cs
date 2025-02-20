namespace DuckBank.Core.Interfaces.Bl
{
    public interface IUnitOfWork
    {
        public IAHorroBl Ahorro { get;  }

        public ICliente Cliente { get;  }        
    }
}

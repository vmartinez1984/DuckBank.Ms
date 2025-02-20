using DuckBank.Core.Interfaces.Bl;

namespace DuckBank.BusinessLayer.Bl
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAHorroBl Ahorro { get; }

        public ICliente Cliente { get; }

        public UnitOfWork(
            IAHorroBl aHorroBl,
            ICliente cliente
        )
        {
            Ahorro = aHorroBl;
            Cliente = cliente;
        }
    }
}

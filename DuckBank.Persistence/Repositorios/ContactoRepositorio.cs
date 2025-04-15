using DuckBank.Persistence.Entities;
using DuckBank.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DuckBank.Persistence.Repositorios
{
    public class ContactoRepositorio : BaseRepositorio, IContactoRepositorio
    {
        private readonly IMongoCollection<Cliente> _collection;

        public ContactoRepositorio(IConfiguration configurations) : base(configurations)
        {
            _collection = _mongoDatabase.GetCollection<Cliente>("Clientes");
        }

        public async Task ActualizarAsync(string clienteId, Contacto contacto)
        {
            Cliente cliente;
            Contacto contacto1;

            cliente = await ObtenerPorIdAsync(clienteId);
            contacto1 = cliente.Contactos.FirstOrDefault(x=> x.Id == contacto.Id);
            contacto1.Cuenta = contacto.Cuenta;
            contacto1.Alias = contacto.Alias;
            contacto1.Nombre = contacto.Nombre;
            
            await ActualizarAsync(cliente);
        }

        public async Task<int> AgregarAsync(string clienteId, Contacto contacto)
        {
            Cliente cliente;

            cliente = await ObtenerPorIdAsync(clienteId);
            if(cliente.Contactos is null) 
                cliente.Contactos = new List<Contacto>();
            contacto.Id = cliente.Contactos.Count +1;
            cliente.Contactos.Add(contacto);
            await ActualizarAsync(cliente);

            return contacto.Id;
        }

        private async Task ActualizarAsync(Cliente item) => await _collection.ReplaceOneAsync(x => x._id == item._id, item);

        private async Task<Cliente> ObtenerPorIdAsync(string idEncodedKey)
        {
            Cliente cliente;

            if (int.TryParse(idEncodedKey, out int id))
                return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

            cliente = await _collection.Find(x => x.EncodedKey == idEncodedKey).FirstOrDefaultAsync();
            if(cliente != null)
                return cliente;
            cliente = await _collection.Find(x => x.Otros["Guid"] == idEncodedKey).FirstOrDefaultAsync();

            return cliente;
        }

        public async Task<List<Contacto>> ObtenerAsync(string clienteId)
        {
            Cliente cliente;

            cliente = await ObtenerPorIdAsync(clienteId);

            return cliente.Contactos;
        }

        public async Task BorrarAsync(string clienteId, string contactoId)
        {
            Cliente cliente;
            Contacto contacto1;

            cliente = await ObtenerPorIdAsync(clienteId);
            if (int.TryParse(contactoId, out int id))
                contacto1 = cliente.Contactos.FirstOrDefault(x => x.Id == id);
            else 
                contacto1 = cliente.Contactos.FirstOrDefault(x=> x.EncodedKey == contactoId);
            contacto1.EstaActivo = false;            

            await ActualizarAsync(cliente);
        }
    }
}

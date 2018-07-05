using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarteiraVirtualWebService
{
    public class IsIntegridadeValid
    {
        private Cliente cliente;
        private int idCliente;
        private CarteiraEntities db = new CarteiraEntities();

        public IsIntegridadeValid(int idCliente)
        {
            this.idCliente = idCliente;
        }

        // Cliente
        public Cliente getCliente()
        {
            if (idCliente > 0)
            {
                cliente = db.Cliente.Where(m => m.idCliente == idCliente).FirstOrDefault();
            }

            return cliente;
        }

        // Verifica se Cliente existe
        public bool IsClienteValid()
        {
            bool retorno = false;

            cliente = getCliente();

            if (cliente != null && cliente.idCliente > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        // Validar Integridade do Cliente com a Carteira
        public bool ValidClienteCarteira()
        {
            bool retorno = false;

            var carteira = db.Carteira.Where(m => m.fkCliente == idCliente).FirstOrDefault();

            if (carteira != null && carteira.idCarteira > 0)
            {
                retorno = true;
            }
            
            return retorno;
        }

    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CarteiraVirtualWebService
{
    /// <summary>
    /// Summary description for PJVWebServiceCarteira
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PJVWebServiceCarteira : System.Web.Services.WebService
    {

        private CarteiraVirtualEntities db = new CarteiraVirtualEntities();

        [WebMethod]
        public string Inserir(string param)
        {
            Carteira carteira = new Carteira();
            RetornoMensagem retorno = new RetornoMensagem("");
            string mensagem = "";
            bool query = true;

            try
            {
                carteira = JsonConvert.DeserializeObject<Carteira>(param);

                // Validação de Integridade do Cliente com a Carteira
                if (carteira.fkCliente > 0)
                {
                    int idCliente = Convert.ToInt32(carteira.fkCliente);

                    IsIntegridadeValid valid = new IsIntegridadeValid(idCliente);

                    // Valida se Cliente existe
                    if (!valid.IsClienteValid())
                    {
                        mensagem = "Cliente não encontrado!";
                        query = false;
                    }
                    // Cliente no Momento pode ter apenas uma carteira
                    else if (valid.ValidClienteCarteira())
                    {
                        mensagem = "O Cliente já possuí uma carteira Vinculado";
                        query = false;
                    }
                }

                if (query)
                {
                    db.Carteira.Add(carteira);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                mensagem = "ERRO: Falha na inclusão do registro: " + ex.Message;
            }

            // Retorno das mensagens em Json
            mensagem = retorno.Carteira(mensagem);
 
            return mensagem;
        }

        [WebMethod]
        public string Alterar(string param)
        {
            Carteira carteira = new Carteira();
            RetornoMensagem retorno = new RetornoMensagem("");
            string mensagem = "";

            // Deserializa o Objeto json
            carteira = JsonConvert.DeserializeObject<Carteira>(param);

            try
            {
                // Validação de Integridade do Cliente com a Carteira
                if (carteira.idCarteira > 0 && carteira.fkCliente > 0)
                {
                    int idCliente = Convert.ToInt32(carteira.fkCliente);

                    IsIntegridadeValid valid = new IsIntegridadeValid(idCliente);

                    if (valid.ValidClienteCarteira())
                    {
                        var carteiraUpdate = db.Carteira.Where(m => m.idCarteira == carteira.idCarteira).FirstOrDefault();
                        carteiraUpdate.nomeCarteira = carteira.nomeCarteira;

                        if (db.SaveChanges() <= 0) mensagem = "Falha na atualização da Carteira";

                    } else
                    {
                        // Cliente não faz parte da carteira - proteção contra fraudes
                        mensagem = "Erro na alteração da Carteira";
                    }
                }
            }
            catch(DbUpdateException ex)
            {
                mensagem = "ERRO: Falha na inclusão do registro: " + ex.Message;
            }

            // Retorno das mensagens em Json
            mensagem = retorno.Carteira(mensagem);

            return mensagem;
        }

    }
}

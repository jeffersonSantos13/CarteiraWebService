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
        private CarteiraEntities db = new CarteiraEntities();
        private RetornoMensagem retorno = new RetornoMensagem("");
        private Carteira carteira = new Carteira();

        // Mensagem de retorno
        private string mensagem = "";

        [WebMethod]
        public void Inserir(string param)
        {          
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
                        mensagem = "Cliente não encontrado!";
                    // Cliente no Momento pode ter apenas uma carteira
                    else if (valid.ValidClienteCarteira())
                        mensagem = "O Cliente já possuí uma carteira Vinculado";
                    else
                    {
                        db.Carteira.Add(carteira);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }

            // Retorno das mensagens em Json
            mensagem = retorno.Response(mensagem);

            Context.Response.Write(mensagem);
        }

        [WebMethod]
        public void Alterar(string param)
        {
            try
            {
                // Deserializa o Objeto json
                carteira = JsonConvert.DeserializeObject<Carteira>(param);

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
            catch(Exception ex)
            {
                mensagem = ex.Message;
            }

            // Retorno das mensagens em Json
            mensagem = retorno.Response(mensagem);

            Context.Response.Write(mensagem);
        }

        [WebMethod]
        public string AtualizarValorCarteira(int idCliente, decimal saldo)
        {

            RetornoMensagem retorno = new RetornoMensagem("");
            string mensagem = "";

            if (idCliente > 0)
            {
                IsIntegridadeValid valid = new IsIntegridadeValid(idCliente);

                if (valid.ValidClienteCarteira())
                {
                    if (saldo > 0)
                    {
                        try
                        {
                            Carteira carteira = new Carteira();
                            carteira = db.Carteira.Where(m => m.fkCliente == idCliente).FirstOrDefault();

                            if (carteira != null)
                            {
                                carteira.saldo = Convert.ToDecimal(saldo);
                            }

                            if (db.SaveChanges() <= 0) mensagem = "Falha na atualização do saldo na Carteira";
                        }
                        catch (DbUpdateException ex)
                        {
                            mensagem = ex.Message;
                        }
                        
                    } else
                    {
                        mensagem = "Saldo para atualização da carteira não informado ou valor abaixo de zero(0)";
                    }
                } else
                {
                    mensagem = "Cliente não possuí uma carteira";
                }
            } else
            {
                mensagem = "Cliente não informado!";
            }

            return mensagem;
        }

    }
}

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
    public class PJVWebServiceParceiro : System.Web.Services.WebService
    {

        private CarteiraEntities db = new CarteiraEntities();

        [WebMethod]
        public string Inserir(string param)
        {
            Parceiro parceiro = new Parceiro();
            RetornoMensagem retorno = new RetornoMensagem("");
            string mensagem = "";

            try
            {
                parceiro = JsonConvert.DeserializeObject<Parceiro>(param);


                db.Parceiro.Add(parceiro);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                mensagem = "ERRO: Falha na inclusão do registro: " + ex.Message;
            }

            // Retorno das mensagens em Json
            mensagem = retorno.Parceiro(mensagem);
 
            return mensagem;
        }

        [WebMethod]
        public string Alterar(string param)
        {
            Parceiro parceiro = new Parceiro();
            RetornoMensagem retorno = new RetornoMensagem("");
            string mensagem = "";

            // Deserializa o Objeto json
            parceiro = JsonConvert.DeserializeObject<Parceiro>(param);

            try
            {
                // Validação de Integridade do Cliente com a Carteira
                if (parceiro.idParceiro > 0)
                {

    
                    //var update = db.Carteira.Where(m => m.idParceiro == parceiro.idParceiro).FirstOrDefault();
                    //update.CNPJ = parceiro.CNPJ;
                    //update.emailParceiro = parceiro.emailParceiro;
                    //update.razaoParceiro = parceiro.razaoParceiro;
                   // update.cpfParceiro = parceiro.cpfParceiro;

                    if (db.SaveChanges() <= 0) mensagem = "Falha na atualização da Carteira";


                }
            }
            catch(DbUpdateException ex)
            {
                mensagem = "ERRO: Falha na inclusão do registro: " + ex.Message;
            }

            // Retorno das mensagens em Json
            mensagem = retorno.Parceiro(mensagem);

            return mensagem;
        }

    }
}

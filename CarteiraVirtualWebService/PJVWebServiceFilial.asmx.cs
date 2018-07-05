using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Globalization;


namespace CarteiraVirtualWebService
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PJVWebServiceFilial : System.Web.Services.WebService
    {

        private CarteiraEntities db = new CarteiraEntities();
        private Mensagem retorno = new Mensagem();
        private Valid val = new Valid();

        [WebMethod]
        public void Inserir(string param)
        {
            ParceiroFilial filial = new ParceiroFilial(); 
 
            try
            {
                filial = JsonConvert.DeserializeObject<ParceiroFilial>(param);

                if (!(val.ValidaChaveEstrangeira((int) filial.fkParceiro)))
                {
                    retorno.erro = true;
                    retorno.response = "202";
                }
                else
                {
                    db.ParceiroFilial.Add(filial);
                    db.SaveChanges();
                    retorno.erro = false;
                    retorno.response = "200";

                }


            }
            catch (Exception ex)
            {
            retorno.erro = true;
            retorno.response = "202";
            }

            Context.Response.Write(retorno);
        }

        [WebMethod]
        public void Alterar(string param)
        {
            ParceiroFilial filial = new ParceiroFilial();

            // Deserializa o Objeto json
            filial = JsonConvert.DeserializeObject<ParceiroFilial>(param);

            try
            {
                // Validação de Integridade do Cliente com a Carteira
                if (filial.fkParceiro > 0 && filial.idFilial > 0)
                {

                    if (!(val.ValidaChaveEstrangeira((int)filial.fkParceiro)))
                    {
                        retorno.erro = true;
                        retorno.response = "202";
                    }
                    else
                    {
                        var update = db.ParceiroFilial.Where(m => m.idFilial == filial.idFilial).FirstOrDefault();
                        update.fkParceiro = filial.fkParceiro;
                        update.desconto = filial.desconto;

                        if (db.SaveChanges() <= 0)
                        {
                            retorno.erro = false;
                            retorno.response = "200";
                        }
                    }


                }
            }
            catch(DbUpdateException ex)
            {
                retorno.erro = true;
                retorno.response = "202";
            }

            Context.Response.Write(retorno);
        }

    }
}

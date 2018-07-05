using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using CarteiraVirtualWebService;
using Newtonsoft.Json;

namespace CarteiraVirtualWebService
{
    /// <summary>
    /// Descrição resumida de PJVWebServiceFinanceiro
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    // [System.Web.Script.Services.ScriptService]
    public class PJVWebServiceFinanceiro : System.Web.Services.WebService
    {

        private CarteiraEntities db = new CarteiraEntities();
        private Mensagem retorno = new Mensagem();

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
 
        public void Inserir(String fin)
        {
            bool erro = true;
            Movimento mov = new Movimento();
            mov = JsonConvert.DeserializeObject<Movimento>(fin);

            try
            {
                DateTime date = DateTime.Today;
                mov.dtMovimento = date;

                if ((mov.fkCarConta == null || mov.fkCarConta <= 0) && erro)
                {
                    erro = false;
                }

                if ((mov.fkCarDestino != null || mov.fkCarDestino <= 0) && erro)
                {
                    erro = false;
                }



                if (erro)
                {
                    db.Movimento.Add(mov);
                    if(db.SaveChanges() > 0)
                    {
                        retorno.erro = false;
                        retorno.response = "200";

                    }
                }
                else
                {
                    retorno.erro = true;
                    retorno.response = "202";
                }

            }
            catch (ArgumentNullException arg)
            {
                Console.WriteLine("ERRO: Parâmetro inválido: {0}", arg.Message);
                retorno.erro = true;
                retorno.response = "202";
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("ERRO: Parâmetro com valor inválido: {0}", ex.Message);
                retorno.erro = true;
                retorno.response = "202";
            }

            Context.Response.Write(retorno);
        }


        public string Consultar(char Mov, int ordemDt = 0, int ordemOp = 0, int Conta = 0, int Destino = 0)
        {
            Movimento mov = new Movimento();

            var lista = db.Movimento.Where(m => m.idMovimento > 0);

            if(Conta > 0)
            {
                lista = lista.Where(m => m.fkCarConta == Conta);
            }

            if(Destino > 0)
            {
                lista = lista.Where(m => m.fkCarDestino == Destino);
            }
            // SE FOR MOV = 'A', TRAZ TODOS OS MOVIMENTOS
            if (!"A".Equals(Mov))
            {
                lista = lista.Where(m => m.acaoMov.Equals(Mov));
            }

            // ORDEM DA DATA
            if (ordemDt == 1 )
            {
                lista = lista.OrderBy(m => m.dtMovimento);
            }
            else
            {
                lista = lista.OrderByDescending(m => m.dtMovimento);
            }

            //MOVIMENTO
            if(ordemOp == 1)
            {
                lista = lista.OrderBy(m => m.acaoMov);
            }else if(ordemOp == 2)
            {
                lista = lista.OrderByDescending(m => m.acaoMov);
            }


            var retorno = lista.ToList();

            return JsonConvert.SerializeObject(retorno);
        }

    }
}

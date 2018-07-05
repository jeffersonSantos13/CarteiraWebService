using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CarteiraVirtualWebService
{
    /// <summary>
    /// Descrição resumida de PJVWebServiceSenha
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    public class PJVWebServiceSenha : System.Web.Services.WebService
    {
            
        private CarteiraEntities db = new CarteiraEntities();

        private RetornoMensagem retorno = new RetornoMensagem("");

        // Mensagem de retorno
        private string mensagem = "";

        [WebMethod]
        public string Inserir(string param)
        {
            Senha senha = new Senha();

            try
            {
                senha = JsonConvert.DeserializeObject<Senha>(param);

                if (senha.fkCarteira > 0)
                {

                    Carteira carteira = new Carteira();
                    carteira = db.Carteira.Where(m => m.fkCliente == senha.fkCarteira).FirstOrDefault();

                    if (carteira != null)
                    {
                        int idCliente = Convert.ToInt32(carteira.fkCliente);

                        IsIntegridadeValid valid = new IsIntegridadeValid(idCliente);

                        // Valida se Cliente existe
                        if (!valid.IsClienteValid())
                        {
                            mensagem = "Cliente não encontrado!";
                        }
                        // Cliente no Momento pode ter apenas uma carteira
                        else if (!valid.ValidClienteCarteira())
                        {
                            mensagem = "O Cliente não possuí uma carteira!";
                        } else if (senha.canal.Equals(""))
                        {
                            mensagem = "Canal de validação da senha não informado!";
                        } else 
                        {
                            db.Senha.Add(senha);
                            db.SaveChanges();
                        }

                    } else
                    {
                        mensagem = "Nenhuma carteira encontrada";
                    }
                }
                else
                {
                    mensagem = "ERRO: Nenhuma carteira mencionada";
                }

            }

            catch (Exception ex)
            {
                mensagem = "ERRO: Falha na inclusão do registro: " + ex.Message;
            }

            // Retorno das mensagens em Json
            mensagem = retorno.Response(mensagem);

            return mensagem;
        }
    }

}

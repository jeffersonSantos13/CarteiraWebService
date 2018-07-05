using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarteiraVirtualWebService
{
    // Classe responsável pelo retorno de erros
    public class RetornoMensagem
    {
        private string retorno;
        private bool valido;

        public RetornoMensagem(string retorno)
        {
            this.retorno = retorno;
        }

        public string Response(string msg)
        {
            Mensagem mensagem = new Mensagem();

            try
            {
                if (msg != null && !msg.Equals(""))
                {
                    mensagem.erro = false;
                    mensagem.response = msg;
                    valido = false;
                    retorno = JsonConvert.SerializeObject(mensagem);
                } else
                {
                    mensagem.erro = true;
                    mensagem.response = "200";
                    valido = true;
                    retorno = JsonConvert.SerializeObject(mensagem);
                }

            }
            catch (Exception e)
            {
                mensagem.erro = false;
                mensagem.response = "Falha na Conversão para Json: " + e.Message;
            }

            return retorno;
        }

        public string Login(string msg, int idCliente)
        {
            Login mensagem = new Login();

            try
            {
                if (msg != null && !msg.Equals(""))
                {
                    mensagem.erro = false;
                    mensagem.response = msg;
                    mensagem.id = 0;
                    valido = false;
                    retorno = JsonConvert.SerializeObject(mensagem);
                }
                else
                {
                    mensagem.erro = true;
                    mensagem.response = "200";
                    mensagem.id = idCliente;
                    valido = true;
                    retorno = JsonConvert.SerializeObject(mensagem);
                }

            }
            catch (Exception e)
            {
                mensagem.erro = false;
                mensagem.id = 0;
                mensagem.response = "Falha na Conversão para Json: " + e.Message;
            }

            return retorno;
        }

        public string Parceiro(string msg)
        {
            Mensagem mensagem = new Mensagem();

            try
            {
                if (msg != null && !msg.Equals(""))
                {
                    mensagem.erro = false;
                    mensagem.response = msg;
                    valido = false;
                    retorno = JsonConvert.SerializeObject(mensagem);
                }
                else
                {
                    mensagem.erro = true;
                    mensagem.response = "";
                    valido = true;
                    retorno = JsonConvert.SerializeObject(mensagem);
                }

            }
            catch (Exception e)
            {
                mensagem.erro = false;
                mensagem.response = "Falha na Conversão para Json: " + e.Message;
            }

            return retorno;
        }

        public string ParceiroFilial(string msg)
        {
            Mensagem mensagem = new Mensagem();

            try
            {
                if (msg != null && !msg.Equals(""))
                {
                    mensagem.erro = false;
                    mensagem.response = msg;
                    valido = false;
                    retorno = JsonConvert.SerializeObject(mensagem);
                }
                else
                {
                    mensagem.erro = true;
                    mensagem.response = "";
                    valido = true;
                    retorno = JsonConvert.SerializeObject(mensagem);
                }

            }
            catch (Exception e)
            {
                mensagem.erro = false;
                mensagem.response = "Falha na Conversão para Json: " + e.Message;
            }

            return retorno;
        }

        

        public bool getValido()
        {
            return valido;
        }
    }
}
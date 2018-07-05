using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace CarteiraVirtualWebService
{
    /// <summary>
    /// Summary description for PJVWebServiceUsuario
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PJVWebServiceUsuario : System.Web.Services.WebService
    {
        private CarteiraEntities db = new CarteiraEntities();
        private RetornoMensagem retorno = new RetornoMensagem("");

        // Validações gerais
        private Valid _valid = new Valid();

        // Mensagem de retorno
        private string mensagem = "";

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void CadastrarUsuario(string param)
        {
            try
            {
                Usuarios usuarios = new Usuarios();
                usuarios = JsonConvert.DeserializeObject<Usuarios>(param);

                var _this = usuarios;

                if (_this != null)
                {
                    if (IsUsuarioValido(_this))
                    {
                        Encrypt encrypt = new Encrypt();

                        string senha = encrypt.RetornarMD5(_this.senha);

                        if (!senha.Equals(""))
                            _this.senha = senha;

                        db.Usuarios.Add(_this);
                        db.SaveChanges();
                    }
                    
                } else
                    mensagem = "Parâmetro incorreto";

            } catch(Exception e)
            {
                mensagem = e.Message;
            }

            // Retorno das mensagens em Json
            mensagem = retorno.Response(mensagem);

            Context.Response.Write(mensagem);
        }

        // Login
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Login(string email, string senha)
        {
            int idRetorno = 0;

            if (!email.Equals(""))
            {
                if (!senha.Equals(""))
                {
                    Encrypt encrypt = new Encrypt();

                    string senhaHash = encrypt.RetornarMD5(senha);

                    var login = db.Usuarios.Where(m => m.email == email && m.senha == senhaHash).FirstOrDefault();

                    var _this = login;

                    if (_this == null)
                        mensagem = "E-mail ou senha inválidos";
                    else
                        idRetorno = _this.idUsuario;
                }
                else
                    mensagem = "Senha não informado.";
            }
            else
                mensagem = "E-mail não informado.";

            // Retorno da mensagem em Json
            mensagem = retorno.Login(mensagem, idRetorno);

            Context.Response.Write(mensagem);
        }

        public bool IsUsuarioValido(Usuarios usuarios)
        {
            bool retorno = true;

            var _this = usuarios;

            if (_this != null)
            {
                if (_this.nome == null || _this.nome.Equals(""))
                {
                    mensagem = "Nome do Usuário não informado.";
                    retorno = false;
                }
                else if (_this.senha == null || _this.senha.Equals(""))
                {
                    mensagem = "Senha do Usuário não informado."; ;
                    retorno = false;
                }
                else if (_this.email == null || _this.email.Equals(""))
                {
                    mensagem = "E-mail informado inválido.";
                    retorno = false;
                }
                else if (!_valid.IsEmail(_this.email.ToString()))
                {
                    mensagem = "E-mail informado inválido.";
                    retorno = false;
                }
                //else if(_this.fkpermissoes == null || _this.fkpermissoes.Equals(""))
                //{
                //    mensagem = "Nenhuma permissão informado para o Usuário.";
                //    retorno = false;
                //}
            }
            else
                retorno = false;

            return retorno;
        }

    }
}

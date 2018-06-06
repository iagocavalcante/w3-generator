using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Xml;

namespace W3TraceCore
{
    public class Trace
    {

        #region [Variáveis Privadas]

        private Util objUtil = new Util();
        private String mClassTAG = "TRC   ";

        #endregion

        #region [Variáveis Públicas]

        public String msTraceTAG = "";
        public String msTraceNivelDebug = "";
        public String msTracePath = "";
        public String msTraceDias = "";
        public String msThreadID = "00000000";

        #endregion

        #region [Funções Privadas]

        /// <summary> LerTagConfig
        /// Metodo para leitura do Arquivo XML de configuração
        /// </summary>
        /// <param name="psCaminho">String representando caminho do arquivo XML</param>
        /// <param name="psGrupoTag">String representando o Grupo para leitura</param>
        /// <param name="psTag">String representando a TAG que contem um conteudo a ser lido</param>
        /// <param name="psAtributo">String representando o atributo na TAG</param>
        /// <param name="psCriterio">String representando o valor do atributo na TAG</param>
        /// <returns>String valor contido na TAG do atributo especificado </returns>
        public string LerTagConfig(String psCaminho, String psConfig, String psGrupoTag, String psTag, String psAtributo, String psCriterio)
        {
            String sPesquisa = "";

            //Carregando o Arquivo
            XmlDocument oXML = new XmlDocument();
            oXML.Load(psCaminho);

            //Pesquisar no Arquivo
            //Com atributo
            if (psAtributo != "")
            {
                sPesquisa = psConfig + "/" + psGrupoTag + "/" + psTag + "[@" + psAtributo + "=" + psCriterio + "]";
                XmlNode oNode = oXML.SelectSingleNode(sPesquisa);
                return oNode.Attributes[psAtributo].Value;
            }
            //Direto no nó
            else
            {
                sPesquisa = psConfig + "/" + psGrupoTag + "/" + psTag;
                XmlNode oNode = oXML.SelectSingleNode(sPesquisa);
                return oNode.InnerText;
            }
        }

        /// <summary> LerTagConfig
        /// Metodo sobrescrito para leitura do Arquivo XML de configuração
        /// </summary>
        /// <param name="psCaminho">String representando caminho do arquivo XML</param>
        /// <param name="psGrupoTag">String representando o Grupo para leitura</param>
        /// <param name="psTag">String representando a TAG com a informação</param>
        /// <returns>String valor do conteúdo da TAG </returns>
        public string LerTagConfig(String psCaminho, String psConfig, String psGrupoTag, String psTag)
        {
            return LerTagConfig(psCaminho, psConfig, psGrupoTag, psTag, "", "");
        }

        /// <summary> LerTagConfig
        /// Metodo para leitura do Arquivo XML de configuração
        /// </summary>
        /// <param name="psCaminho">String representando caminho do arquivo XML</param>
        /// <param name="psGrupoTag">String representando o Grupo para leitura</param>
        /// <param name="psTag">String representando a TAG que contem um conteudo a ser lido</param>
        /// <param name="psAtributo">String representando o Atributo de Criterio na TAG</param>
        /// <param name="psCriterio">String representando o valor do atributo na TAG</param>
        /// <param name="psDado">String representando o valor do dado na TAG</param>
        /// <returns>String valor contido na TAG do atributo especificado </returns>
        public string LerTagConfig(String psCaminho, String psConfig, String psGrupoTag, String psTag, String psAtributo, String psCriterio, String psDado)
        {
            String sPesquisa = "";

            //Carregando o Arquivo
            XmlDocument oXML = new XmlDocument();
            oXML.Load(psCaminho);

            //Pesquisar no Arquivo
            //Com atributo
            if (psAtributo != "")
            {
                sPesquisa = psConfig + "/" + psGrupoTag + "/" + psTag + "[@" + psAtributo + "=" + psCriterio + "]";
                XmlNode oNode = oXML.SelectSingleNode(sPesquisa);
                return oNode.Attributes[psDado].Value;
            }
            //Direto no nó
            else
            {
                sPesquisa = psConfig + "/" + psGrupoTag + "/" + psTag;
                XmlNode oNode = oXML.SelectSingleNode(sPesquisa);
                return oNode.InnerText;
            }
        }

        /// <summary> LerTagConfig
        /// Metodo para leitura do Arquivo XML de configuração
        /// </summary>
        /// <param name="psCaminho">String representando caminho do arquivo XML</param>
        /// <param name="psGrupoTag">String representando o Grupo para leitura</param>
        /// <param name="psTag">String representando a TAG que contem um conteudo a ser lido</param>
        /// <param name="psAtributo">String representando o Atributo de Criterio na TAG</param>
        /// <returns>String valor contido na TAG do atributo especificado </returns>
        public string LerTagConfig(String psCaminho, String psConfig, String psGrupoTag, String psTag, String psAtributo)
        {
            String sPesquisa = "";

            //Carregando o Arquivo
            XmlDocument oXML = new XmlDocument();
            oXML.Load(psCaminho);

            //Pesquisar no Arquivo
            //Com atributo
            if (psAtributo != "")
            {
                sPesquisa = psConfig + "/" + psGrupoTag + "/" + psTag + "[@" + psAtributo + "]";
                XmlNode oNode = oXML.SelectSingleNode(sPesquisa);
                return oNode.Attributes[0].Value.ToString();
            }
            //Direto no nó
            else
            {
                sPesquisa = psConfig + "/" + psGrupoTag + "/" + psTag;
                XmlNode oNode = oXML.SelectSingleNode(sPesquisa);
                return oNode.InnerText;
            }
        }



        #endregion

        #region [Funções Públicas]

        /// <summary> Trace
        /// Metodo construtor do Trace do WebService
        /// </summary>
        public Trace()
        {
            String sCaminho = "";
            String sConfig = "";

            //Configurar Trace
            sCaminho = AppDomain.CurrentDomain.BaseDirectory.ToString() + "TRACE.xml";
            msTraceTAG = LerTagConfig(sCaminho, sConfig, "Trace", "traceTAG");
            msTraceNivelDebug = LerTagConfig(sCaminho, sConfig, "Trace", "traceNivelDebug");
            msTracePath = LerTagConfig(sCaminho, sConfig, "Trace", "tracePath");
            msTraceDias = LerTagConfig(sCaminho, sConfig, "Trace", "traceDias");
            objUtil.ConfigTrace(msTracePath, Convert.ToInt32(msTraceDias));

            msThreadID = msThreadID + Convert.ToString(System.Threading.Thread.CurrentThread.ManagedThreadId);
            msThreadID = msThreadID.Substring(msThreadID.Length - 7);
        }

        /// <summary> Trace (TAG da classe de origem)
        /// Metodo construtor do Trace do WebService
        /// </summary>
        /// <param name="pClassTAG">String com tag da classe de origem</param>
        public Trace(String pClassTAG, string c)
        {
            String sCaminho = "";
            String sConfig = c;

            //Configurar Trace
            mClassTAG = pClassTAG + " ";
            sCaminho = AppDomain.CurrentDomain.BaseDirectory.ToString() + "TRACE.xml";
            msTraceTAG = LerTagConfig(sCaminho, sConfig, "Trace", "traceTAG");
            msTraceNivelDebug = LerTagConfig(sCaminho, sConfig, "Trace", "traceNivelDebug");
            msTracePath = LerTagConfig(sCaminho, sConfig, "Trace", "tracePath");
            msTraceDias = LerTagConfig(sCaminho, sConfig, "Trace", "traceDias");
            objUtil.ConfigTrace(msTracePath, Convert.ToInt32(msTraceDias));

            msThreadID = msThreadID + Convert.ToString(System.Threading.Thread.CurrentThread.ManagedThreadId);
            msThreadID = msThreadID.Substring(msThreadID.Length - 7);
        }

        /// <summary> TratarErro
        /// Metodo PRIVADO que trata qualquer erro ocorrido no WEB SERVICE
        /// </summary>
        /// <param name="pErro">String com mensagem de erro</param>
        public void TratarErro(String psOrigem, String psErro)
        {
            GravarTrace("ERRO: " + psOrigem + "@" + psErro, 0);
        }


        /// <summary> TratarErro
        /// Metodo PRIVADO que trata qualquer erro ocorrido no WEB SERVICE
        /// </summary>
        /// <param name="pExeption">Exception que traz os dados de um erro tratado</param>
        public void TratarErro(Exception pExeption)
        {
            GravarTrace("ERRO: " + pExeption.Source + "@" + pExeption.Message, 0);
        }


        /// <summary> GravarTrace
        /// Grava o Trace do Web Service aceitando nivel de debug default = 0
        /// </summary>
        /// <param name="psMensagem">String representando uma mensagem para gravação </param>
        /// 
        public void GravarTrace(String psMensagem)
        {
            GravarTrace(psMensagem, 0);
        }


        /// <summary> GravarTrace
        /// Grava o Trace do Web Service
        /// </summary>
        /// <param name="psMensagem">String representando uma mensagem para gravação </param>
        /// <param name="piNivelDebug">Inteiro representando o nivel de debug para mensagem</param>
        ///
        public void GravarTrace(String psMensagem, int piNivelDebug)
        {
            if (piNivelDebug <= Convert.ToInt32(msTraceNivelDebug))
            {
                psMensagem = mClassTAG + "[" + msThreadID + "]" + psMensagem;
                objUtil.GravarTrace(msTraceTAG, psMensagem);
            }
        }

        /// <summary> GravarMetodo
        /// Grava os dados de entrada do método no Trace do Web Service
        /// </summary>
        /// <param name="parametros">String representando os parametros de entrada do método</param>
        ///
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void GravarMetodo(dynamic parametros)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            string classe = sf.GetMethod().DeclaringType.Name;
            string metodo = sf.GetMethod().Name;
            string parameters = JsonConvert.SerializeObject(parametros);
            GravarTrace("\nCLASSE:\n" + classe + "\nMÉTODO:\n" + metodo + "\nPARÂMETROS:\n" + parameters, 1);
        }

        /// <summary> GravarRetorno
        /// Grava os dados de retorno do método no Trace do Web Service
        /// </summary>
        /// <param name="retorno" cref="Retorno">Objeto de retorno</param>
        /// <returns cref="Retorno"></returns>
        ///
        public Retorno GravarRetorno(string tipo, Retorno retorno)
        {
            GravarTrace(tipo + JsonConvert.SerializeObject(retorno), 2);
            return retorno;
        }

        /// <summary> GravarRetornoStr
        /// Grava os dados de retorno do método no Trace do Web Service
        /// </summary>
        /// <param name="retorno" cref="Retorno">Objeto de retorno</param>
        /// <returns cref="Retorno"></returns>
        ///
        public Retorno GravarRetornoStr(string tipo, Retorno retorno)
        {
            GravarTrace(tipo + JsonConvert.SerializeObject(retorno.dados), 2);
            return retorno;
        }

        /// <summary> GravarErro
        /// Grava o erro do método no Trace do Web Service
        /// </summary>
        /// <param name="ex" cref="Exception">Objeto de exceção</param>
        /// <returns cref="Retorno"></returns>
        ///
        public Retorno GravarErro(Exception ex)
        {
            Retorno retorno = new Retorno();
            retorno.sucesso = false;
            retorno.dados = ex;
            GravarTrace("\nERRO:\n" + JsonConvert.SerializeObject(retorno), 1);
            return retorno;
        }

        #endregion

        #region MÉTODOS SIMPLIFICADOS

        /// <summary> Trace (TAG da classe de origem)
        /// Metodo construtor do Trace do WebService
        /// </summary>
        /// <param name="pClassTAG">String com tag da classe de origem</param>
        public Trace(String pClassTAG, string path, int x)
        {
            mClassTAG = pClassTAG;
            msTracePath = path;

            msThreadID = msThreadID + Convert.ToString(System.Threading.Thread.CurrentThread.ManagedThreadId);
            msThreadID = msThreadID.Substring(msThreadID.Length - 7);
        }

        /// <summary> GravarTrace
        /// Grava o Trace do Web Service
        /// </summary>
        /// <param name="psMensagem">String representando uma mensagem para gravação </param>
        /// <param name="piNivelDebug">Inteiro representando o nivel de debug para mensagem</param>
        ///
        public void CriarMensagem(String psMensagem)
        {
            psMensagem = mClassTAG + " [" + msThreadID + "]" + psMensagem;
            CriarArquivo(psMensagem);
        }

        /// <summary>
        /// Gravar mensagens em um arquivo Texto
        /// </summary>
        /// <param name="strIDFile">Identificador do Arquivo</param>
        /// <param name="strMsg">Mensagem a ser gravada</param>
        private int CriarArquivo(string strMsg)
        {
            string strArq;

            try
            {
                //Criar diretório se necessário    
                if (!Directory.Exists(msTracePath)) Directory.CreateDirectory(msTracePath);

                //Gera Nome do Arquivo de Trace
                strArq = msTracePath + @"\" + mClassTAG + DateTime.Today.ToString("yyyyMMdd") + @".log";
                return SalvarArquivo(strArq, strMsg);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Gravar mensagens em um arquivo Texto
        /// </summary>
        /// <param name="strArq">Nome do Arquivo</param>
        /// <param name="strMsg">Mensagem a ser gravada</param>
        private int SalvarArquivo(string strArq, string strMsg)
        {
            string strCopyArq;
            StreamWriter objSW;

            try
            {
                //Criando o arquivo
                objSW = new StreamWriter(strArq, true);

                //Gravar arquivo
                objSW.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss") + "] " + strMsg);

                //Fechar arquivo
                objSW.Close();
                objSW = null;

                //Testar se estoura o aumento do arquivo
                FileInfo objFile = new FileInfo(strArq);

                if (objFile.Length > 3145728) //Arquivo maior que 3 MB
                {
                    strCopyArq = msTracePath + @"\" + objFile.Name.Substring(0, objFile.Name.Length - 4) + @"_" + DateTime.Now.ToString("HHmmss") + @".log";
                    objFile.MoveTo(strCopyArq);
                }
                objFile = null;
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static void PostTrace(string url, string tag, string msg)
        {
            var parametros = new
            {
                tag = tag,
                msg = msg
            };

            Uri uri = new Uri(url);
            var request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Timeout = 10000;

            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            string dados = JsonConvert.SerializeObject(parametros);
            writer.Write(dados);
            writer.Close();

            var response = request.GetResponse();
        }

        public static string GetInnerExceptions(Exception e, string msgs = "")
        {
            if (e == null) return string.Empty;
            //if (msgs == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "=>InnerException: " + GetInnerExceptions(e.InnerException, msgs);
            else
                msgs += e.Message;
            return msgs;
        }
        #endregion

    }
}

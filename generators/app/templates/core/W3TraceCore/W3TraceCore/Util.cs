using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace W3TraceCore
{
    public class Util
    {

        #region [enum Públicas]
        public enum TipoAcao
        {
            Validar = 1,
            Executar = 2,
            Confirmar = 4,
            Cancelar = 8,
        }
        #endregion


        #region [Variáveis Privadas]

        private string mstrPATH;
        private int mintDiasTrace;
        private string licensa = "3853BD41A45C877ED59377A11EC59138";

        #endregion


        #region [Funções Públicas]


        /// <summary>
        /// Configurar o caminho e quantidade de dias no trace para geração de arquivo
        /// </summary>
        /// <param name="strPath">Diretório do Arquivo Texto</param>
        /// <param name="intDiasTrace">Quantidade de dias de trace para limpeza automática</param>
        public void ConfigTrace(string strPath, int intDiasTrace)
        {
            if (strPath.Substring(1, 1) != @"\")
                strPath = strPath + @"\";
            mstrPATH = strPath;
            mintDiasTrace = intDiasTrace;
        }


        /// <summary>
        /// Gravar mensagens em um arquivo Texto
        /// </summary>
        /// <param name="strIDFile">Identificador do Arquivo</param>
        /// <param name="strMsg">Mensagem a ser gravada</param>
        /// <param name="strPATH">Caminho do arquivo</param>
        public int GravarTrace(string strIDFile, string strMsg, string strPATH)
        {
            string strArq;

            try
            {
                //Criar diretório se necessário    
                if (strPATH == "") strPATH = @"c:\";
                if (!Directory.Exists(strPATH)) Directory.CreateDirectory(strPATH);

                //Gera Nome do Arquivo de Trace
                strArq = strPATH + @"\" + strIDFile + DateTime.Today.ToString("yyyyMMdd") + @".log";
				
				ExcluirArquivo(mstrPATH, strIDFile);
				
                return Trace(strArq, strMsg);
            }
            catch
            {
                return 0;
            }
        }
		
		public void ExcluirArquivo(string mstrPATH, string strIDFile)
        {
            try
            {
                DateTime d = DateTime.Now;
                string[] files = Directory.GetFiles(mstrPATH, strIDFile +"2*");

                foreach (var fl in files)
                {
                    DateTime creation = File.GetCreationTime(fl);

                    if (creation.Date <= d.AddDays(-mintDiasTrace).Date)
                    {
                        File.Delete(fl);
                    }  

                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gravar mensagens em um arquivo Texto
        /// </summary>
        /// <param name="strIDFile">Identificador do Arquivo</param>
        /// <param name="strMsg">Mensagem a ser gravada</param>
        public int GravarTrace(string strIDFile, string strMsg)
        {
            string strArq;

            try
            {
                //Criar diretório se necessário    
                if (mstrPATH == "") mstrPATH = @"c:\";
                if (!Directory.Exists(mstrPATH)) Directory.CreateDirectory(mstrPATH);

                //Gera Nome do Arquivo de Trace
                strArq = mstrPATH + @"\" + strIDFile + DateTime.Today.ToString("yyyyMMdd") + @".log";
                return Trace(strArq, strMsg);
            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        /// Extrai o diretório do arquivo
        /// </summary>
        /// <param name="strArquivo">Caminho completo do arquivo</param>
        public string ExtrairDiretorio(ref string strArquivo)
        {
            string strArqAux = strArquivo;
            try
            {
                int intPos = strArqAux.LastIndexOf("\\");
                string strDiretorio = strArqAux.Substring(0, intPos);
                strArqAux = strArqAux.Substring(intPos + 1);
                strArquivo = strArqAux;
                if (strDiretorio.EndsWith("\\"))
                {
                    strDiretorio = strDiretorio.Substring(0, strDiretorio.Length - 1);
                }
                return strDiretorio;
            }
            catch
            {
                return "";
            }
        }

        public void convertToANSI(string arqOrigem, string arqDestino)
        {
            StreamReader sr = new StreamReader(arqOrigem);
            StreamWriter sw = new StreamWriter(arqDestino, false, Encoding.Default);

            sw.WriteLine(sr.ReadToEnd());

            sw.Close();
            sr.Close();
        }

        public static void FillProperties<T>(ref T obj, JObject json) where T : class
        {
            Type type = obj.GetType();
            IEnumerable<PropertyInfo> properties = type.GetRuntimeProperties();
            foreach (PropertyInfo property in properties)
            {
                object val = json.GetValue(property.Name);
                if (val != null && val.ToString().Length > 0)
                {
                    if (property.PropertyType == typeof(string))
                        val = val.ToString().ToUpper();
                    property.SetValue(obj, Convert.ChangeType(val, property.PropertyType), null);
                }
            }
        }

        public static string ConvertListToJSON<T>(List<T> listaObj) where T : class
        {
            JArray jArray = new JArray();
            foreach (T obj in listaObj)
            {
                var json = JObject.FromObject(obj);
                jArray.Add(json);
            }
            return JsonConvert.SerializeObject(jArray);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }        

        #endregion


        #region [Funções Privadas]

        /// <summary>
        /// Gravar mensagens em um arquivo Texto
        /// </summary>
        /// <param name="strArq">Nome do Arquivo</param>
        /// <param name="strMsg">Mensagem a ser gravada</param>
        private int Trace(string strArq, string strMsg)
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
                    strCopyArq = mstrPATH + @"\" + objFile.Name.Substring(0, objFile.Name.Length - 4) + @"_" + DateTime.Now.ToString("HHmmss") + @".log";
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


        #endregion

    }
}

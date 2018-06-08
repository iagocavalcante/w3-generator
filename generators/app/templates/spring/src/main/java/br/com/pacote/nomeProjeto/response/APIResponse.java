package br.com.<%= pacote %>.<%= nomeProjeto %>.response;

public class APIResponse{	
	private String dados    = "";
	
	private String codResposta = "200";
	
	private String msgResposta = "OK";

	public APIResponse() {
	}

	public String getDados() {
		return dados;
	}

	public void setDados(String dados) {
		this.dados = dados;
	}

	public String getCodResposta() {
		return codResposta;
	}

	public void setCodResposta(String codResposta) {
		this.codResposta = codResposta;
	}

	public String getMsgResposta() {
		return msgResposta;
	}

	public void setMsgResposta(String msgResposta) {
		this.msgResposta = msgResposta;
	}

	@Override
	public String toString() {
		return "{\"dados\": " + dados + ", \"codResposta\": \"" + codResposta + "\", \"msgResposta\": \"" + msgResposta + "\"}";
	}
}
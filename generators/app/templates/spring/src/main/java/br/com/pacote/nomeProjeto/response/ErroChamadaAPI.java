package br.com.<%= pacote %>.<%= nomeProjeto %>.response;

public class ErroChamadaAPI {
	public String codigo;
	
	public String mensagem;

	public String getCodigo() {
		return codigo;
	}

	public void setCodigo(String codigo) {
		this.codigo = codigo;
	}

	public String getMensagem() {
		return mensagem;
	}

	public void setMensagem(String mensagem) {
		this.mensagem = mensagem;
	}
}

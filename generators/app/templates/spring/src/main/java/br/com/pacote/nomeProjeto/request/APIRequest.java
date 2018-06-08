package br.com.<%= pacote %>.<%= nomeProjeto %>.request;

import io.swagger.annotations.ApiModelProperty;

public class APIRequest {
	@ApiModelProperty(notes="Email do cadastro")
	public String email;
	
	@ApiModelProperty(notes="Nome do usuario")
	private String nome;

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getNome() {
		return nome;
	}

	public void setNome(String nome) {
		this.nome = nome;
	}
}

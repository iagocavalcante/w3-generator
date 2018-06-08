package br.com.<%= pacote %>.<%= nomeProjeto %>.controller;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.web.client.RestTemplateBuilder;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;

import br.com.<%= pacote %>.<%= nomeProjeto %>.request.APIRequest;
import br.com.<%= pacote %>.<%= nomeProjeto %>.response.APIResponse;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;

@RestController
@RequestMapping("/demo")
@Api(description="Servico de Demonstracao")
public class APIController {

	private static Logger logger = LoggerFactory.getLogger(APIController.class);

	RestTemplate rest;

	@Autowired
	public APIController(RestTemplateBuilder builder) {
		rest = builder.build();
	}

	@RequestMapping(value = "/post", consumes = "application/json", method = RequestMethod.POST)
	@ApiOperation(value="Cadastra usuario")
	public ResponseEntity<APIResponse> post(@RequestBody APIRequest request) {
		logger.info("[REQUEST]: " + request.toString());

		APIResponse response = new APIResponse();
		
		response.setDados("Welcome " + request.getNome());

		return new ResponseEntity<APIResponse>(response, HttpStatus.OK);
	}
}
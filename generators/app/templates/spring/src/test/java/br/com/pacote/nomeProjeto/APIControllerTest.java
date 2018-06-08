package br.com.<%= pacote %>.<%= nomeProjeto %>;

import static org.junit.Assert.assertEquals;
import static org.springframework.test.web.client.match.MockRestRequestMatchers.requestTo;
import static org.springframework.test.web.client.response.MockRestResponseCreators.withSuccess;

import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.client.RestClientTest;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.test.web.client.MockRestServiceServer;

import br.com.<%= pacote %>.<%= nomeProjeto %>.controller.APIController;
import br.com.<%= pacote %>.<%= nomeProjeto %>.request.APIRequest;
import br.com.<%= pacote %>.<%= nomeProjeto %>.response.APIResponse;

@RunWith(SpringRunner.class)
@RestClientTest(APIController.class)
public class APIControllerTest {
	@Autowired
	APIController controller;

	@Autowired
	private MockRestServiceServer server;
	
	@Test
	public void postComSucesso() throws Exception {
		APIRequest request = new APIRequest();
		request.setEmail("demo@mail.com");
		request.setNome("Demonstration");
		
		APIResponse apiResponse = new APIResponse();
		apiResponse.setCodResposta("200");
		apiResponse.setMsgResposta("OK");
		apiResponse.setDados("Welcome Demonstration");

        this.server.expect(requestTo("/demo/post")).andRespond(withSuccess());
        
        ResponseEntity<APIResponse> resp = this.controller.post(request);
        
        assertEquals(HttpStatus.OK, resp.getStatusCode());
        assertEquals(apiResponse.getDados(), resp.getBody().getDados());
	}
}

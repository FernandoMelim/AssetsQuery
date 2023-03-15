# AssetsQuery

## Configura��es
As configura��es utilizadas s�o simples e elas est�o presentes no arquivo appsettings.Development.json.
As configura��es de chave "YahooSettings" podem ser mantidas da forma em que est�o, pois s�o relacionadas aos endpoint da API do Yahoo.
A configura��o de chave "ConnectionStrings" cont�m a "SqlConnection", que � a conection string do banco que ser� utilizado. Estou subindo a minha connection string como exemplo para apenas substituirem os valores.
Sugiro que utilizem o SQL Server, pois foi o SGBD que utilizei.

O script para a cria��o das tabelas no banco est� presente na pasta principal do projeto em um arquivo chamado "CreateDatabaseQuery.txt".

## Endpoints
O projeto possui dois endpoints, presentes no "AssetsController". O endpoint de post � respons�vel por obter os dados do ativo soliciado fazendo uma requisi��o para a API do yahoo. Ap�s obter os dados, eles s�o tratados e salvos no banco. O endpoint de GET � respons�vel por obter os dados do ativo solicitado exibindo as informa��es especificadas.
Ambos endpoints recebem como par�metro o nome do ativo solicitado, como por exemplo: PETR4.SA e ABEV3.SA.


## Arquitetura
O projeto possui 4 camadas, que s�o: Presentation, Application, Domain e Infrastructure. <br/>

Presentation: Respons�vel por armazenar apenas as configura��es, tais como as poss�veis vari�veis de ambiente, e os controllers da aplica��o.<br/>
Application: Respons�vel por conter os Services, que s�o respons�veis pelas regras de neg�cio da aplica��o e que podem solicitar aos reposit�rios algum opera��o no banco de dados.<br/>
Domain: Respons�vel por conter as entidades e enums do projeto.<br/>
Infrastructure: respons�vel por conter os reposit�rios e a implementa��o de acesso ao banco de acordo com o sgbd e ORM usado. Por ser um projeto pequeno, optei por utilizar o Dapper, mas eu usaria o EntityFramework para projetos maiores pela facilidade de utiliza��o.<br/><br/>


Foram feitos testes unit�rios utilizando o xUnit no projeto chamado Tests. <br/><br/>

Link para a descri��o do teste t�cnico: https://github.com/guideti/variacao-ativo
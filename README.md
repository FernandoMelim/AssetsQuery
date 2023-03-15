# AssetsQuery

## Configurações
As configurações utilizadas são simples e elas estão presentes no arquivo appsettings.Development.json.
As configurações de chave "YahooSettings" podem ser mantidas da forma em que estão, pois são relacionadas aos endpoint da API do Yahoo.
A configuração de chave "ConnectionStrings" contém a "SqlConnection", que é a conection string do banco que será utilizado. Estou subindo a minha connection string como exemplo para apenas substituirem os valores.
Sugiro que utilizem o SQL Server, pois foi o SGBD que utilizei.

O script para a criação das tabelas no banco está presente na pasta principal do projeto em um arquivo chamado "CreateDatabaseQuery.txt".

## Endpoints
O projeto possui dois endpoints, presentes no "AssetsController". O endpoint de post é responsável por obter os dados do ativo soliciado fazendo uma requisição para a API do yahoo. Após obter os dados, eles são tratados e salvos no banco. O endpoint de GET é responsável por obter os dados do ativo solicitado exibindo as informações especificadas.
Ambos endpoints recebem como parâmetro o nome do ativo solicitado, como por exemplo: PETR4.SA e ABEV3.SA.


## Arquitetura
O projeto possui 4 camadas, que são: Presentation, Application, Domain e Infrastructure. <br/>

Presentation: Responsável por armazenar apenas as configurações, tais como as possíveis variáveis de ambiente, e os controllers da aplicação.<br/>
Application: Responsável por conter os Services, que são responsáveis pelas regras de negócio da aplicação e que podem solicitar aos repositórios algum operação no banco de dados.<br/>
Domain: Responsável por conter as entidades e enums do projeto.<br/>
Infrastructure: responsável por conter os repositórios e a implementação de acesso ao banco de acordo com o sgbd e ORM usado. Por ser um projeto pequeno, optei por utilizar o Dapper, mas eu usaria o EntityFramework para projetos maiores pela facilidade de utilização.<br/><br/>


Foram feitos testes unitários utilizando o xUnit no projeto chamado Tests. <br/><br/>

Link para a descrição do teste técnico: https://github.com/guideti/variacao-ativo
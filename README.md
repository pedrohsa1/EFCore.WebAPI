# Web Crawler - Tribunal de Justiça BA

[![N|Solid](https://cldup.com/dTxpPi9lDf.thumb.png)](https://nodesource.com/products/nsolid)

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

O objetivo desses scripts é desenvolver um Crawler que faça a consulta dos dados do processo 0809979-67.2015.8.05.0080 no site do Segundo Grau do Tribunal de Justiça da BA (http://esaj.tjba.jus.br/cpo/sg/open.do). O projeto é constituído por uma aplicação Web Api e outra Console.

  - Projeto Api possui as operações básicas CRUD para as tabelas modeladas, usando  a camada de persistência e o framework Entity.
  - Na aplicação Console ScrapingTjba, fazemos o scraping ao executar projeto do console, buscando as informações e usamos as APIs para gravar-las  no banco de dados Mysql.

# Componentes do Projeto

  - SGBD Mysql
  - .NET Entity Framework Core
  
# Criação do Banco de Dados

  - Configurar a Conexão em appsettings.json
  - Executar no Console do Gerenciador de Pacotes o comando update-migration

# APIs

  - GET api/processo
  - GET api/processo/filtro/{numerodo_processo}
  - POST api/processo
  - PUT api/processo/{id}
  - DELETE api/processo/delete/{id}
  - GET api/movimentacao
  - PUT api/movimentacao/{id}
  - DELETE api/movimentacao/delete/5





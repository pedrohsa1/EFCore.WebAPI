# Web Crawler - Tribunal de Justiça BA

[![N|Solid](https://cldup.com/dTxpPi9lDf.thumb.png)](https://nodesource.com/products/nsolid)

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

O objetivo desses scripts é desenvolver um Crawler que faça a consulta dos dados do processo 0809979-67.2015.8.05.0080 no site do Segundo Grau do Tribunal de Justiça da BA (http://esaj.tjba.jus.br/cpo/sg/open.do). O projeto é constituído com uma aplicação Web Api e Console.

  - Projeto Api possui as operações básicas CRUD para as tabelas modeladas, usando  a camada de persistência e o framework Entity.
  - Na aplicação Console, a ScrapingTjba, fazemos o scraping das informações e usamos as APIs para gravar as informações.

# APIs

  - GET api/processo
  - GET api/processo/filtro/{numerodo_processo}
  - POST api/processo
  - PUT api/processo/{id}
  - DELETE api/processo/delete/{id}
  - GET api/movimentacao
  - GET api/movimentacao
  - PUT api/movimentacao/{id}
  - DELETE api/movimentacao/delete/5





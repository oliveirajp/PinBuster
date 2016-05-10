# Pinbuster


## API
  - Api address [http://pinbusterapitest.azurewebsites.net/api/](http://pinbusterapitest.azurewebsites.net/api/)

  - **Utilizador**

    - **GET utilizadores**  [http://pinbusterapitest.azurewebsites.net/api/utilizador](http://pinbusterapitest.azurewebsites.net/api/utilizador)
    
     `Resposta json:
     {"data":[{"utilizador_id":"1","nome":"teste","imagem":"imagem_teste","raio":"5000"},{.....} }
    `

    - **POST utilizador**  [http://pinbusterapitest.azurewebsites.net/api/utilizador](http://pinbusterapitest.azurewebsites.net/api/utilizador)
    
     `json a enviar:
     {nome : 'string', imagem : 'string', raio : 'int', face_id : 'string'}
     `

     `Resposta json:
     {"data": 'done' }
     `

    - **GET utilizador individual**  [http://pinbusterapitest.azurewebsites.net/api/utilizador/x](http://pinbusterapitest.azurewebsites.net/api/utilizador/6)
    
     `Resposta json:
     {"utilizador_id":"6","nome":"postName","imagem":"imagemPost","raio":"20000","face_id":"6"}
     `


    - **DELETE utilizador individual**  [http://pinbusterapitest.azurewebsites.net/api/utilizador/x](http://pinbusterapitest.azurewebsites.net/api/utilizador/6)
    
     `Resposta json:
     {}
     `






  - **Follow**

    - **GET follow**  [http://pinbusterapitest.azurewebsites.net/api/follow](http://pinbusterapitest.azurewebsites.net/api/follow)
    
     `Resposta json:
     {{"data":[{"follower":"6","followed":"1"} , {....}]}
     `
    - **POST follow**  [http://pinbusterapitest.azurewebsites.net/api/follow](http://pinbusterapitest.azurewebsites.net/api/follow)
    
     `json a enviar:
     {"follower" : string,"followed" : string}
     `

     `Resposta json:
     {"data": 'done' }
     `
    - **GET utilizadores seguidos por utilizador_ id**  [http://pinbusterapitest.azurewebsites.net/api/follow/:face_id?f=follower](http://pinbusterapitest.azurewebsites.net/api/follow/:face_id?f=follower)
    
     `Resposta json:
     {"data":[{"follower":"6","followed":"1"}]}
     `
    - **GET utilizadores que seguem utilizador_ id**  [http://pinbusterapitest.azurewebsites.net/api/follow/:face_id?f=followed](http://pinbusterapitest.azurewebsites.net/api/follow/:face_id?f=followed)
    
     `Resposta json:
     {"data":[{"follower":"1","followed":"6"}]}
     `

    - **DELETE follow de utilizador_ id a um outro utilizador**  [http://pinbusterapitest.azurewebsites.net/api/follow/:face_id?unfollow=id](http://pinbusterapitest.azurewebsites.net/api/follow/:face_id?unfollow=id)
    
     `Resposta json:
     {"data": 'done' }
     `





  - **Mensagem**
- **GET mensagens com informação do utilizador**  [http://pinbusterapitest.azurewebsites.net/api/message_user](http://pinbusterapitest.azurewebsites.net/api/message_user)
    
    
      `Resposta json:
     {"data":[{"mensagem_id":"7","latitude":"-8.597355","longitude":"41.1756418","data":"Sun Feb 10 2013 23:11:11 GMT+0000 (GMT Standard Time)","tempo_limite":"100000","raio":"1500","face_id":"5","conteudo":"Ã¡ espera da camisola","localizacao":"Porto","categoria":"normal","nome":"null","imagem":"null"},{...}]}
     `

    - **POST mensagem**  [http://pinbusterapitest.azurewebsites.net/api/mensagem](http://pinbusterapitest.azurewebsites.net/api/mensagem)
    
     `json a enviar:
     {
"latitude": 41.1756418,
"longitude" : -8.597355,
"data" : "20130210 11:11:11 PM",
"tempo_limite" : 100000,
"raio" : 1500,
"face_id" : "5",
"conteudo" : "á espera da camisola",
"localizacao" : "Porto"
}
     `

     `Resposta json:
     {"data": 'done' }
     `

    - **GET mensagens**  [http://pinbusterapitest.azurewebsites.net/api/mensagem](http://pinbusterapitest.azurewebsites.net/api/mensagem)
    
     `Resposta json:
     {"data":[{"mensagem_id":"2","latitude":"41.177489","longitude":"-8.598343","data":"Mon Jun 18 2012 10:34:09 GMT+0000 (Coordinated Universal Time)","tempo_limite":"0","raio":"1000","face_id":"6","conteudo":"Feupinha","localizacao":"Porto"},{....}]}
     `



    - **GET mensagens de face_id**  [http://pinbusterapitest.azurewebsites.net/api/mensagem/:face_id](http://pinbusterapitest.azurewebsites.net/api/mensagem/:face_id)
    
     `Resposta json:
{"data":[{"mensagem_id":"7","latitude":"-8.597355","longitude":"41.1756418","data":"Sun Feb 10 2013 23:11:11 GMT+0000 (Coordinated Universal Time)","tempo_limite":"100000","raio":"1500","face_id":"5","conteudo":"Ã¡ espera da camisola","localizacao":"Porto"},{....}]}
     `








  - **Achievement** - falta implementar algumas coisas, nao esta tudo a funcionar


    - **GET achievements**  [http://pinbusterapitest.azurewebsites.net/api/achievement](http://pinbusterapitest.azurewebsites.net/api/achievement)
    
     `Resposta json:
     {"data":[{
"cidade"  : 'string',
descricao   : 'string',
nome   : 'string',
mensagem_id  : int,
face_id : string
},{...}]}
     `

    - **POST achievements**  [http://pinbusterapitest.azurewebsites.net/api/achievement](http://pinbusterapitest.azurewebsites.net/api/achievement)
    
     `json a enviar:
     {
"cidade"  : 'string',
descricao   : 'string',
nome   : 'string',
face_id  : string
}
     `

      `Resposta json:
     {"data": 'done' }
     `

    - **GET achievements de um utilizador**  [http://pinbusterapitest.azurewebsites.net/api/achievement/:face_id](http://pinbusterapitest.azurewebsites.net/api/achievement/6)
    
    
      `Resposta json:
     {"data":[{"face_id":"6","nome":"porto lindo","MessagesNeeded":"2","MessagesFound":"1"},{"face_id":"6","nome":"terc","MessagesNeeded":"1","MessagesFound":"1"}]}
     `

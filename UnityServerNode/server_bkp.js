var app = require("express")(); //instancia o express em app
var http = require("http").Server(app); //cria um servidor
var io = require("socket.io")(http); // cria uma conexão
var shortId = require("shortid"); // instancia a lib shortid

io.onconnection("connection", function (socket) {
  //Processa a conexão
  socket.on("CALLBACK_SOCKET", function (pack_json) {
    //processamento
    console.log("id do player é: " + pack_json.id);

    socket.emit;
  });
});

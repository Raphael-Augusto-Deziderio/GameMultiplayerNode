const { count } = require("console");
const { Socket } = require("dgram");
var express = require("express");
var app = express();
var http = require("http").Server(app);
var io = require("socket.io")(http);

var clientLookup = {};

io.on("connection", function (socket) {
  var currentPlayer;

  socket.on("JOIN_ROOM", function (pack) {
    currentPlayer = {
      name: pack.name,
      id: socket.id,
      position: "0d0,0d0,0d0",
      rotation: "0d0,0d0,0d0",
    };

    clientLookup[currentPlayer.id] = currentPlayer;

    socket.emit("JOIN_SUCCESS", currentPlayer);

    //envia o jogador atual para TODOS  os jogadores online
    socket.broadcast.emit("SPAWN_PLAYER", currentPlayer);

    //agora enviar TODOS os jogadores para o jogador atual
    for (client in clientLookup) {
      if (clientLookup[client].id != currentPlayer.id) {
        socket.emit("SPAWN_PLAYER", clientLookup[client]);
      }
    }
  });

  //ENVIA A POSIÇÃO DO PLAYER PARA TODOS OS PLAYERS ONLINE
  socket.on("MOVE_AND_ROT", function (pack) {
    var data = {
      id: currentPlayer.id,
      position: pack.position,
      rotation: pack.rotation,
    };

    currentPlayer.position = data.position;
    currentPlayer.rotation = data.rotation;
    data.position = currentPlayer.position;
    data.rotation = currentPlayer.rotation;

    socket.broadcast.emit("UPDATE_POS_ROT", data);
  });
});

http.listen(3000, function () {
  console.log("server listen on 3000!");
});

console.log("----------------NodeJS Server is running------------------");

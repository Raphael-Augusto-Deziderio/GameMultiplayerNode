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
      order: Object.keys(clientLookup).length + 1,
    };

    clientLookup[currentPlayer.id] = currentPlayer;

    //CRIA O PLAYER PRA ELE MESMO
    socket.emit("JOIN_SUCCESS", currentPlayer);

    //SPAWNA O JOGADOR PARA TODOS OS PLAYER ONLINE
    socket.broadcast.emit("SPAWN_PLAYER", currentPlayer);

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

    console.log("id: " + data.id);
    console.log("position: " + data.position);
    console.log("rotation: " + data.rotation);
    socket.broadcast.emit("UPDATE_POS_ROT", data);
  });
});

http.listen(3000, function () {
  console.log("server listen on 3000!");
});

console.log("----------------NodeJS Server is running------------------");



var gameId;

$( document ).ready(function() {
    
    let params = (new URL(document.location)).searchParams;
    gameId = params.get("gameId");
    if(gameId != null)
    {
    }
    else
    {
        $('#board').hide();
    }
});




function InitializeTictactoe (){
    clearBoard();
    var clientMove = GameIdPromise();
    clientMove.then((response) => {
        console.log(response);

        let baseUrl = window.location.href.split('?')[0]; 
        let boardGameUrl =  baseUrl + "?gameId=" + response;
        window.location = boardGameUrl;
    })
    .catch((error) => {
        console.log(error);
        SetMessage("Server not responding properly. Please try again later.");
    });
}

function SetMessage(string){
  $('#message').html(string); 
}

function clearBoard() {
    document.querySelectorAll(".square").forEach(function(square) {
        square.innerHTML = "";
        square.classList.remove("unclickable");
    });
    SetMessage();
}


function disableAllBlocks(){
  document.querySelectorAll(".square").forEach(function(square) {
    square.classList.add("unclickable");
});
}

var basrUrl = "http://localhost:12121/";
var clientSymbol = "X";
var serverSymbol = "O";

function ClientMove(x, y) {
    UpdateBoard(x, y, true);
    var clientMove = ClientMovePromise(x,y);
    clientMove.then((response) => {
        
        console.log(response);
        if (response.GameMove != null){
          UpdateBoard(response.GameMove.row, response.GameMove.col, false);
        }
        if (response.IsGameOver){
          SetMessage(response.ResponseMessage);
          disableAllBlocks();
        }
    })
        .catch((error) => {
            console.log(error);
        });
  }

function Unclickable(x, y)
{
    var idPrefix = "square";
    elementId = idPrefix + "_"  + x + "_" + y; 
    var element = document.getElementById(elementId);
    element.classList.add("unclickable");
}


function UpdateBoard(x, y, isClient) 
{
    var symboleVal = serverSymbol;
    if (isClient)
    {
        symboleVal = clientSymbol;
    }
    var idPrefix = "square";
    elementId = idPrefix + "_"  + x + "_" + y; 
    var element = document.getElementById(elementId);
    element.innerHTML = `<span class=${symboleVal}>${symboleVal}</span>`;
    Unclickable(x,y);
}


function ClientMovePromise(x,y){
    return new Promise((resolve, reject) => {
        let clientMove = { row: x, col:y, gameId: gameId };
        $.ajax({
            url: basrUrl + 'api/Game/ClientMove',
            type: 'POST',
            data: clientMove,
            success: function (data) {
                //console.log(data);
                resolve(data);
            },
            error: function (error) {
                //console.log(error);
                reject(error);
            },
        })
    })
};


function GameIdPromise(){
    return new Promise((resolve, reject) => {
        
        $.ajax({
            url: basrUrl + 'api/Game/GameId',
            type: 'GET',
            success: function (data) {
                //console.log(data);
                resolve(data);
            },
            error: function (error) {
                //console.log(error);
                reject(error);
            },
        })
    })
};



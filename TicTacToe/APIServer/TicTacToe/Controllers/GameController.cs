using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicTacToe.Models;
using TicTacToe.Utilities;
using System.Web.Http.Cors;
using System.Web;
using System.Web.Services;

namespace TicTacToe.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GameController : ApiController
    {
        static char[,] board ;
        static int counter = 0;
        
        private char[,] GetNewBoard()
        {
            char[,] newBoard = {{ '_', '_', '_' },
                                { '_', '_', '_' },
                                { '_', '_', '_' }};
            return newBoard;
        }

        [HttpGet] 
        public string GameId()
        {
            string uniqueId = Guid.NewGuid().ToString();
            board = GetNewBoard();
            counter = 0;
            return uniqueId;
        }

        [WebMethod(EnableSession = true)]
        [HttpPost]
        public Response ClientMove(Move clientMove)
        {
            Response response = new Response();
            MinimaxUtility.UpdateBoard(clientMove, board, true);
            ++counter;
            string message = DetermineWinner(board);
            if (message != null)
            {
                response = PrepareRespoonse(null, message);
                return response;
            }
            Move serverMove = MinimaxUtility.findBestMove(board);
            MinimaxUtility.UpdateBoard(serverMove, board, false);
            ++counter;
            message =  DetermineWinner(board);
            response = PrepareRespoonse(serverMove, message);
            return response;
        }

        private string DetermineWinner(char[,] board)
        {
            string message = MinimaxUtility.DeterminWinner(board);
            if (message == null && counter == 9)
            {
                message = "Game Over";
            }
            return message;
        }

        private Response PrepareRespoonse(Move move, string msg)
        {
            Response response = new Response();
            response.GameMove = move;
            if (msg == null)
            {
                response.IsGameOver = false;
            }
            else
            {
                response.IsGameOver = true;
            }
            response.ResponseMessage = msg;
            return response;
        }

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSign
{
    X,
    O,
    NONE
}

public enum GameState
{
    LISTENING,
    PLAYING
}
public class GameManager : Singleton<GameManager>
{

    PlayerSign _mySign;
    PlayerSign _activeSign;
    GameState _currentGameState;
    PlayerSign[] _gameBoard;

    public GameState CurrentGameState { get => _currentGameState; set => _currentGameState = value; }



    // Start is called before the first frame update
    void Start()
    {
        _gameBoard = new PlayerSign[9];
        
        _activeSign = PlayerSign.X; // X always starts the game
        UIManager.Instance.SetStatusText("Waiting for " + _activeSign.ToString() + "...");
        InitBoard();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMySign();
        DetermineState();



    }


    #region Victory Checks
    private PlayerSign CheckVictory()
    {
        if (CheckVictoryForSign(PlayerSign.X)) return PlayerSign.X;
        else if (CheckVictoryForSign(PlayerSign.O)) return PlayerSign.O;
        else { return PlayerSign.NONE; }

    }

    private bool CheckVictoryForSign(PlayerSign sign)
    {
        //[0][1][2]
        //[3][4][5]
        //[6][7][8]

        if (CheckDiagonalsForSign(sign) || CheckRowsForSign(sign) || CheckColumnsForSign(sign))
        {
            return true;
        }

        return false;

    }

    private bool CheckRowsForSign(PlayerSign sign)
    {
        for (int i = 0; i < 3; i += 3)
        {
            if (_gameBoard[i] == sign &&
                _gameBoard[i + 1] == sign &&
                    _gameBoard[i + 2] == sign)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckColumnsForSign(PlayerSign sign)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_gameBoard[i] == sign &&
                _gameBoard[i + 3] == sign &&
                    _gameBoard[i + 6] == sign)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckDiagonalsForSign(PlayerSign sign)
    {

        if (_gameBoard[0] == sign &&
            _gameBoard[4] == sign &&
            _gameBoard[8] == sign)
        {
            return true;
        }
        else if (_gameBoard[2] == sign &&
                 _gameBoard[4] == sign &&
                 _gameBoard[6] == sign)
        {
            return true;
        }
        return false;
    }
    #endregion

    public PlayerSign OppsiteSign(PlayerSign sign)
    {
        if (sign == PlayerSign.X)
        {
            return PlayerSign.O;
        }
        else
        {
            return PlayerSign.X;
        }
    }

    private void InitBoard()
    {
        for (int i = 0; i < _gameBoard.Length; i++)
        {
            _gameBoard[i] = PlayerSign.NONE;
        }
    }

    public bool PlaceSignOnBoard(int pos, PlayerSign sign)
    {
        if(_gameBoard[pos] == PlayerSign.NONE)
        {
            _gameBoard[pos] = sign;
            UIManager.Instance.PlaceSignInUISlot(pos, sign);
            return true;
        }

        return false;
    }

    public void UpdateMySign()
    {
        if (NetworkManager.Instance.MyPortConfig == PortConfig.Port1)
        {
            _mySign = PlayerSign.X;
        }
        else 
        {
            _mySign = PlayerSign.O;
        }

        UIManager.Instance.SetPlayerIndicatorText(_mySign);
    }

    private void DetermineState()
    {
        if(_mySign == _activeSign)
        {
            CurrentGameState = GameState.PLAYING;
        }
        else
        {
            CurrentGameState = GameState.LISTENING;
        }
    }


    private void swapTurn()
    {
        if (_activeSign == PlayerSign.X)
        {
            _activeSign = PlayerSign.O;
        }

        else
        {
            _activeSign = PlayerSign.X;
        }
    }

    public void OnSlotClicked(int pos)
    {
        if (CurrentGameState == GameState.PLAYING)
        {
            if (PlaceSignOnBoard(pos, _mySign))
            {
                //NetworkManager.SendMove(pos,sign);

                swapTurn();
            }
        }
    }



    public void OnMessageRecieved(int pos)
    {
        if (CurrentGameState == GameState.LISTENING)
        {
            if (PlaceSignOnBoard(pos, OppsiteSign(_mySign)))
            {
                swapTurn();
            }
        }
    }

}

using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour
{
    int[,] board;
    int currentTurn;
    [SerializeField] LayerMask layer;
    [SerializeField] GameObject redChip;
    [SerializeField] GameObject yellowChip;
    [SerializeField] float dropY;
    [SerializeField] float dropZ;
    [SerializeField] float aiWait = 0.75f;

    void Start()
    {
        board = Connect4Board.CreateBoard();
        currentTurn = 1;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                GameObject clickedObject = hit.collider.gameObject;
                ClickBox clickBox = clickedObject.GetComponent<ClickBox>();
                GameObject chip = currentTurn == 1 ? redChip : yellowChip;
                if (Connect4Board.PlaceChip(board, clickBox.Column, currentTurn)) {
                    Instantiate(chip, new Vector3(clickBox.DropPos, dropY, dropZ), Quaternion.identity);
                    int winner = Connect4Board.CheckWin(board);
                    currentTurn = -1;
                    StartCoroutine(MakeAIMove());
                }
            }
        }
    }

    private IEnumerator MakeAIMove()
    {
        yield return new WaitForSeconds(aiWait);
        int aiMove = FindAIMove();
        Connect4Board.PlaceChip(board, aiMove, -1);
        Instantiate(yellowChip, new Vector3((aiMove - 3) * 1.1f, dropY, dropZ), Quaternion.identity);
        currentTurn = 1;
    }

    private int FindAIMove()
    {
        int best = 10000000;
        int choice = 0;
        int[,] b = Connect4Board.CopyBoard(board);
        for (int i = 0; i < 7; i++)
        {
            if (Connect4Board.PlaceChip(b, i, -1))
            {
                int score = Connect4Board.MiniMax(b, 1, 5, 10000000, -10000000);
                if (score < best)
                {
                    best = score;
                    choice = i;
                }
                Connect4Board.PopChip(b, i);
            }
            //Debug.Log(best);
        }
        return choice;
    }
}

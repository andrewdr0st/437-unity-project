using UnityEngine;

public class BoardManager : MonoBehaviour
{
    int[,] board;
    int currentTurn;
    [SerializeField] LayerMask layer;
    [SerializeField] GameObject redChip;
    [SerializeField] GameObject yellowChip;
    [SerializeField] float dropY;
    [SerializeField] float dropZ;

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
                    currentTurn = currentTurn == 1 ? -1 : 1;
                    int winner = Connect4Board.CheckWin(board);
                    Debug.Log(Connect4Board.Evaluate(board));
                }
            }
        }
    }
}

using UnityEngine;

public class GridTest : MonoBehaviour
{
    private Grid<int> grid;

    private void Start()
    {
        grid = new Grid<int>(10, 10, 10f, new Vector3(-5f, -5f), () => 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameDebug.LogMessage(grid.GetGridObject(Utils.GetMouseWorldPosition()).ToString());
        }

        if (Input.GetMouseButtonDown(0))
        {
            grid.SetGridObject(Utils.GetMouseWorldPosition(), 1);
        }
    }
}

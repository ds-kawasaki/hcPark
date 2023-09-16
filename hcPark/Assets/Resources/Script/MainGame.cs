using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //インスペクタにて、シーンのメインカメラを割り当てる
    public Camera mainCamera;
    //インスペクタにて、マーカー表示用プレファブを割り当てる
    public GameObject markPrefab;
    private List<GameObject> line = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        this.line.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.ClearLine();
        }
        if (Input.GetMouseButton(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var raycastHitList = Physics.RaycastAll(ray).ToList();
            if (raycastHitList.Any()) {
                var distance = Vector3.Distance(mainCamera.transform.position, raycastHitList.First().point);
                var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                var currentPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                currentPosition.y = 0;
                this.AddLine(currentPosition);
            }
        }
    }

    private void ClearLine()
    {
        foreach (var n in this.line)
        {
            Destroy(n);
        }
        this.line.Clear();
    }

    private void AddLine(Vector3 pos)
    {
        GameObject mark = Instantiate(this.markPrefab);
        mark.transform.position = pos;
        this.line.Add(mark);
    }
}

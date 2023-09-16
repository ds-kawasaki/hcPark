using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private const float MIN_DISTANCE = 0.7f;

    // インスペクタでまとめて割り当てる用車セット
    [System.Serializable] private class CarSet
    {
        //インスペクタにて、carを割り当てる
        public GameObject car;
        //インスペクタにて、goalを割り当てる
        public GameObject goal;
        //インスペクタにて、マーカー表示用プレファブを割り当てる
        public GameObject markPrefab;

        private List<GameObject> line = new List<GameObject>();

        public void Clear()
        {
            foreach (var n in this.line)
            {
                Destroy(n);
            }
            this.line.Clear();
        }

        public void AddLine(Vector3 pos)
        {
            GameObject mark = Instantiate(this.markPrefab);
            mark.transform.position = pos;
            this.line.Add(mark);
        }

        public void Move(float delta)
        {
            var rb = this.car.GetComponent<Rigidbody>();
            if (rb == null) { return; }
            while (this.line.Count > 0)
            {
                var top = this.line[0];
                var sa = top.transform.position;
                sa -= rb.position;
                sa.y = 0; //高さは無視
                if (sa.sqrMagnitude > delta * delta)
                {
                    sa = sa.normalized;
                    sa *= delta;
                    sa += rb.position;
                    rb.MovePosition(sa);
                    break;
                }
                else
                {
                    Destroy(top);
                    this.line.RemoveAt(0);
                }
            }
        }

        public bool IsMoveEnd()
        {
            return this.line.Count == 0;
        }

        public bool NearCar(Vector3 pos)
        {
            if (this.line.Count > 0) { return false; } // 既にラインひいている車はパス
            var sa = this.car.transform.position;
            sa -= pos;
            return sa.sqrMagnitude < MIN_DISTANCE * MIN_DISTANCE;
        }

        public bool IsGoal()
        {
            if (this.line.Count > 0) { return false; } // ライン残っている場合まだ
            var sa = this.car.transform.position;
            sa -= this.goal.transform.position;
            return sa.sqrMagnitude < MIN_DISTANCE * MIN_DISTANCE;
        }
    }

    private enum Status
    {
        Input,
        Moving,
        Failed,
        Success,
    }

    //インスペクタにて、シーンのメインカメラを割り当てる
    [SerializeField] private Camera mainCamera;
    //インスペクタにて、車セットを割り当てる
    [SerializeField] private List<CarSet> carSets;
    //インスペクタにて、車の速度を割り当てる
    [SerializeField] private float speed;

    private Status status = Status.Input;
    private CarSet nowSet = null;
    private int lineNum = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.status)
        {
            case Status.Input:
                this.ActInput();
                break;
            case Status.Moving:
                this.ActMoving();
                break;
            default:
                break;
        }
    }


    private void ChangeState(Status newState)
    {
        this.status = newState;
    }

    private void ActInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pos = this.ScreenPointToGroundPosition(Input.mousePosition);
            this.nowSet = this.FindCar(pos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (this.nowSet != null)
            {
                this.lineNum++;
                if (this.lineNum >= this.carSets.Count)
                {
                    this.ChangeState(Status.Moving);
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (this.nowSet != null)
            {
                var pos = this.ScreenPointToGroundPosition(Input.mousePosition);
                this.nowSet.AddLine(pos);

            }
        }
    }

    private void ActMoving()
    {
        var delta = this.speed *Time.deltaTime;
        foreach (var n in this.carSets)
        {
            n.Move(delta);
        }

        if (this.carSets.All(n => n.IsMoveEnd()))
        {
            if (this.carSets.All(n => n.IsGoal()))
            {
                this.ChangeState(Status.Success);
            }
            else
            {
                this.ChangeState(Status.Failed);
            }
        }
    }




    private CarSet FindCar(Vector3 pos)
    {
        foreach (var n in this.carSets)
        {
            if (n.NearCar(pos))
            {
                return n;
            }
        }
        return null;
    }
    private Vector3 ScreenPointToGroundPosition(Vector3 point)
    {
        var ray = mainCamera.ScreenPointToRay(point);
        var raycastHitList = Physics.RaycastAll(ray).ToList();
        if (raycastHitList.Any())
        {
            var distance = Vector3.Distance(mainCamera.transform.position, raycastHitList.First().point);
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            var currentPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            currentPosition.y = 0;
            return currentPosition;
        }
        return Vector3.zero;
    }
}

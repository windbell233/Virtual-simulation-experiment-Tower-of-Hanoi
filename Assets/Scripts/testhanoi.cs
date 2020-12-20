using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class testhanoi : MonoBehaviour
{
    public GameObject[] plates;
    //一共有五个盘子
    public int platesNumber;
    private bool isAllowMove;
    private bool isMovedPlate = false;
    private Stack<GameObject> platesStackA = new Stack<GameObject>();
    private Stack<GameObject> platesStackB = new Stack<GameObject>();
    private Stack<GameObject> platesStackC = new Stack<GameObject>();
    //一共有三个柱子，所以，它当成了三个栈
    private List<GameObject> list = new List<GameObject>();
    public Material[] color;//三个颜色
    public GameObject a;
    public GameObject b;
    public GameObject c;
    //这三个，其实都只是代表的是柱子的位置信息，其实可以换成transfrom
    public GameObject whole;
    //这个是整体的位置吗
    [SerializeField]
    private GameObject aSign;
    [SerializeField]
    private GameObject bSign;
    [SerializeField]
    private GameObject cSign;
    [SerializeField]
    private Transform aTransform;
    [SerializeField]
    private Transform bTransform;
    [SerializeField]
    private Transform cTransform;
    [SerializeField]
    private GameObject colliderA;
    [SerializeField]
    private GameObject colliderB;
    [SerializeField]
    private GameObject colliderC;
    private GameObject movedGameObject;
    private string movedcolumn;
    private Vector3 positionPlate;
    private bool isMoveEnd = true;
    public Vector3 wholeposition = new Vector3();
    private int hanoistepcount;
    private bool isSignle = false;
    private bool isManual = false;
    public GameObject test;
    public Text text;
    private Transform tempTrans;
    public Vector3 offset;
    public Vector3 trans;
    private LayerMask mask = 1 << 8;
    private int completeState = 0;
    public Button AUTO;
    public Button Manua;
    private int Move_sum=0;
    public Light[] lights = new Light[3];
    private bool stepRight=true;
    //0表示，没有移动，
    //1表示，移动到顶部
    //2表示，移动到另一个柱子上
    //3表示，移动到另一个柱子上,并，完成下降
    class HanoiStep
    {
        public string first;
        public string end;
    }
    private List<HanoiStep> hanoistep = new List<HanoiStep>();
    // Use this for initialization

    HanoiStep hanoiStepManual = new HanoiStep();
    //这个是手动情况下的
    public  void SetplatesNumber(float num)
    {
        platesNumber = (int)num;
        RetryPlates();        
    }
  
    void Start()
    {
        positionPlate = new Vector3();
        wholeposition = whole.transform.position;
        colliderA.GetComponent<CapsuleCollider>().enabled = false;
        colliderB.GetComponent<CapsuleCollider>().enabled = false;
        colliderC.GetComponent<CapsuleCollider>().enabled = false;
        InitPlates(platesNumber);           
    }

    /// <summary>
    /// 1 如果自己仍然放在自己的位置上
    /// 2 如果，第一步移动正确，第二部移动错误
    /// </summary>
    private void Update()
    {
        if (isManual == false)
            return;
        if (isMoveEnd == false)
            return;
        //hanoistep[hanoistepcount].first
        //    hanoistepcount < hanoistep.Count
        if (Input.GetMouseButtonDown(0))
        {
           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 撞击到了哪个3D物体
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer == 8) //A柱子
                {
                    if (completeState == 0)//未移动
                    {
                        if (platesStackA.Count > 0)
                        {
                            hanoiStepManual.first = "A";
                            MovePlateToTop(hanoiStepManual.first,hanoiStepManual.first.Equals(hanoistep[Move_sum].first));
                            
                        }
                        else
                            return;
                        Debug.Log("....." + hanoiStepManual.first);
                        Debug.Log("....." + hanoistep[Move_sum].first);
                        Debug.Log("MOVE_SUM" + Move_sum);
                        Debug.Log(hanoiStepManual.first.Equals(hanoistep[Move_sum].first) );
                        if (hanoiStepManual.first.Equals(hanoistep[Move_sum].first) == false&&hanoiStepManual.first.Equals(hanoiStepManual.end)==false)
                        {
                            Debug.Log("123123123123123123");
                            ReturnPosition();
                            StartCoroutine(DelayLight(hanoiStepManual.first, 2));
                            return;
                        }
                        if (hanoiStepManual.first.Equals(hanoistep[Move_sum].first) == true)
                            completeState = 1;
                    }
                    else if (completeState == 1) //已经移动到一根柱子的顶部
                    {
                        hanoiStepManual.end = "A";
                        MovePlateFromXToY(hanoiStepManual.end,hanoiStepManual.end.Equals(hanoistep[Move_sum].end), hanoiStepManual.first);
                        //ReturnPosition();
                        
                        
                        Debug.Log("....." + hanoiStepManual.end);
                        Debug.Log("....." + hanoistep[Move_sum].end);
                        Debug.Log("MOVE_SUM" + Move_sum);
                        stepRight = hanoiStepManual.end.Equals(hanoistep[Move_sum].end) ;
                        if (hanoiStepManual.end.Equals(hanoistep[Move_sum].end) == false&& hanoiStepManual.end.Equals(hanoiStepManual.first) == false)
                        {
                            //StartCoroutine(DelayReturn(5));
                            
                            StartCoroutine(DelayLight(hanoiStepManual.end, 2));
                            return;
                        }
                        if(stepRight==true)
                        {
                            Move_sum++;
                        }
                        hanoiStepManual.first = null;
                        
                        completeState = 0;
                    }

                }
                else if (hit.collider.gameObject.layer ==9) //B柱子
                {
                    if (completeState == 0)//未移动
                    {
                        if (platesStackB.Count > 0)
                        {
                            hanoiStepManual.first = "B";
                            MovePlateToTop(hanoiStepManual.first, hanoiStepManual.first.Equals(hanoistep[Move_sum].first));

                        }
                        else
                            return;
                        Debug.Log("....." + hanoiStepManual.first);
                        Debug.Log("....." + hanoistep[Move_sum].first);
                        Debug.Log("MOVE_SUM" + Move_sum);
                        Debug.Log(hanoiStepManual.first.Equals(hanoistep[Move_sum].first));
                        if (hanoiStepManual.first.Equals(hanoistep[Move_sum].first) == false && hanoiStepManual.first.Equals(hanoiStepManual.end) == false)
                        {
                            ReturnPosition();
                            StartCoroutine(DelayLight(hanoiStepManual.first, 2));
                            return;
                        }
                        if(hanoiStepManual.first.Equals(hanoistep[Move_sum].first)==true)
                            completeState = 1;
                    }
                    else if (completeState == 1) //已经移动到一根柱子的顶部
                    {
                        hanoiStepManual.end = "B";
                        MovePlateFromXToY(hanoiStepManual.end, hanoiStepManual.end.Equals(hanoistep[Move_sum].end), hanoiStepManual.first);
                        //ReturnPosition();


                        Debug.Log("....." + hanoiStepManual.end);
                        Debug.Log("....." + hanoistep[Move_sum].end);
                        Debug.Log("MOVE_SUM" + Move_sum);
                        stepRight = hanoiStepManual.end.Equals(hanoistep[Move_sum].end);
                        if (hanoiStepManual.end.Equals(hanoistep[Move_sum].end) == false && hanoiStepManual.end.Equals(hanoiStepManual.first) == false)
                        {
                            ReturnPosition();
                            StartCoroutine(DelayLight(hanoiStepManual.end, 2));
                            return;
                        }
                        if (stepRight == true)
                        {
                            Move_sum++;
                        }
                        hanoiStepManual.first = null;

                        completeState = 0;
                    }

                }
                else if (hit.collider.gameObject.layer == 10) //C柱子
                {
                    if (completeState == 0)//未移动
                    {
                        if (platesStackC.Count > 0)
                        {
                            hanoiStepManual.first = "C";
                            MovePlateToTop(hanoiStepManual.first, hanoiStepManual.first.Equals(hanoistep[Move_sum].first));

                        }
                        else
                            return;
                        Debug.Log("....." + hanoiStepManual.first);
                        Debug.Log("....." + hanoistep[Move_sum].first);
                        Debug.Log("MOVE_SUM" + Move_sum);
                        Debug.Log(hanoiStepManual.first.Equals(hanoistep[Move_sum].first));
                        if (hanoiStepManual.first.Equals(hanoistep[Move_sum].first) == false && hanoiStepManual.first.Equals(hanoiStepManual.end) == false)
                        {
                            ReturnPosition();
                            StartCoroutine(DelayLight(hanoiStepManual.first, 2));
                            return;
                        }
                        if (hanoiStepManual.first.Equals(hanoistep[Move_sum].first) == true)
                            completeState = 1;
                    }
                    else if (completeState == 1) //已经移动到一根柱子的顶部
                    {
                        hanoiStepManual.end = "C";
                        MovePlateFromXToY(hanoiStepManual.end, hanoiStepManual.end.Equals(hanoistep[Move_sum].end), hanoiStepManual.first);
                        //ReturnPosition();


                        Debug.Log("....." + hanoiStepManual.end);
                        Debug.Log("....." + hanoistep[Move_sum].end);
                        Debug.Log("MOVE_SUM" + Move_sum);
                        stepRight = hanoiStepManual.end.Equals(hanoistep[Move_sum].end) ;
                        if (hanoiStepManual.end.Equals(hanoistep[Move_sum].end) == false && hanoiStepManual.end.Equals(hanoiStepManual.first) == false)
                        {
                            ReturnPosition();
                            StartCoroutine(DelayLight(hanoiStepManual.end, 2));
                            return;
                        }
                        if (stepRight == true)
                        {
                            Move_sum++;
                        }
                        hanoiStepManual.first = null;

                        completeState = 0;
                    }

                }
                //    else if (hit.collider.gameObject.layer == 9) //B柱子
                //    {
                //        if (completeState == 0)//未移动
                //        {
                //            if (platesStackB.Count > 0)
                //            {
                //                hanoiStepManual.first = "B";
                //                MovePlateToTop(hanoiStepManual.first, hanoiStepManual.first.Equals(hanoistep[Move_sum].first));

                //            }
                //            else
                //                return;
                //            Debug.Log("....." + hanoiStepManual.first);
                //            Debug.Log("....." + hanoistep[Move_sum].first);
                //            Debug.Log(hanoiStepManual.first.Equals(hanoistep[Move_sum].first));
                //            if (hanoiStepManual.first.Equals(hanoistep[Move_sum].first) == false)
                //            {

                //                Debug.LogError("错误");
                //                //ReturnPosition();

                //                StartCoroutine(DelayLight(hanoiStepManual.first, 2));
                //                return;
                //            }
                //            completeState = 1;
                //        }
                //        else if (completeState == 1) //已经移动到一根柱子的顶部
                //        {
                //            hanoiStepManual.end = "B";
                //            MovePlateFromXToY(hanoiStepManual.end, hanoiStepManual.end.Equals(hanoistep[Move_sum].end), hanoiStepManual.first);
                //            //ReturnPosition();


                //            Debug.Log("....." + hanoiStepManual.end);
                //            Debug.Log("....." + hanoistep[Move_sum].end);
                //            if (hanoiStepManual.end.Equals(hanoistep[Move_sum].end) == false && hanoiStepManual.end.Equals(hanoiStepManual.first) == false)
                //            {
                //                ReturnPosition();
                //                StartCoroutine(DelayLight(hanoiStepManual.end, 2));
                //                return;
                //            }
                //            hanoiStepManual.first = null;
                //            Move_sum++;
                //            completeState = 0;
                //        }
                //    }
                //    else if (hit.collider.gameObject.layer == 10) //C柱子
                //    {
                //        if (completeState == 0)//未移动
                //        {
                //            if (platesStackC.Count > 0)
                //            {
                //                hanoiStepManual.first = "C";
                //                MovePlateToTop(hanoiStepManual.first, hanoiStepManual.first.Equals(hanoistep[Move_sum].first));

                //            }
                //            else
                //                return;
                //            Debug.Log("....."+hanoiStepManual.first);
                //            Debug.Log("....."+hanoistep[Move_sum].first);
                //            if (hanoiStepManual.first.Equals(hanoistep[Move_sum].first) == false)
                //            {
                //                ReturnPosition();
                //                StartCoroutine(DelayLight(hanoiStepManual.first, 2));
                //                return;
                //            }
                //            completeState = 1;
                //        }
                //        else if (completeState == 1) //已经移动到一根柱子的顶部
                //        {
                //            hanoiStepManual.end = "C";
                //            MovePlateFromXToY(hanoiStepManual.end,hanoiStepManual.end.Equals(hanoistep[Move_sum].end), hanoiStepManual.first);
                //            //ReturnPosition();


                //            Debug.Log("......"+hanoiStepManual.end);
                //            Debug.Log("......"+hanoistep[Move_sum].end);
                //            if (hanoiStepManual.end.Equals(hanoistep[Move_sum].end) == false && hanoiStepManual.end.Equals(hanoiStepManual.first) == false)
                //            {
                //                ReturnPosition();
                //                StartCoroutine(DelayLight(hanoiStepManual.end, 2));
                //                return;
                //            }
                //            hanoiStepManual.first = null;
                //            Move_sum++;
                //            completeState = 0;
                //        }
                //    }

            }
        }
        //test.GetComponent<TextMesh>().text ="list:"+list.Count+ whole.name;
    }
    /// <summary>
    /// 生成plate
    /// </summary>
    /// <param name="index"></param>
    public void InitPlates(int index)
    {
        colliderA.GetComponent<CapsuleCollider>().enabled = false;
        colliderB.GetComponent<CapsuleCollider>().enabled = false;
        colliderC.GetComponent<CapsuleCollider>().enabled = false;
        // HanoiIndicateController.Instance.Init();
        platesNumber = index;
        platesStackA.Clear();
        platesStackB.Clear();
        platesStackC.Clear();
        whole.transform.position = wholeposition;
        isAllowMove = true;
        isMoveEnd = true;
        movedGameObject = null;
        movedcolumn = "";
        isMovedPlate = false;
        if (list != null)
        {
            foreach (GameObject child in list)
            {
                Destroy(child);
            }
            list.Clear();
        }
        for (int i = 0; i < index; i++)
        {
            GameObject plate = Instantiate(plates[index - i - 1], new Vector3(plates[4 - i].transform.position.x, plates[4 - i].transform.position.y, plates[4 - i].transform.position.z), plates[4 - i].transform.rotation, this.transform);
           
            list.Add(plate);
            plate.name = (i + 1).ToString();
            plate.SetActive(true);
            platesStackA.Push(plate);
        }
    }
    /// <summary>
    /// 重置
    /// </summary>
    public void RetryPlates()
    {
        Move_sum = 0;
        StopAllCoroutines();
        // HanoiIndicateController.Instance.moveCount.text = "0";
        if (whole.GetComponentsInChildren<Transform>().Length > 1)
        {
            InitPlates(platesNumber);

            ChangeToWhole();
        }
        else
        {
            InitPlates(platesNumber);
        }
        Manua.interactable = true;
        AUTO.interactable = true;
    }
    /// <summary>
    /// 关闭汉诺塔
    /// </summary>
    public void Close()
    {
        StopAllCoroutines();
        platesStackA.Clear();
        platesStackB.Clear();
        platesStackC.Clear();
        movedGameObject = null;
        movedcolumn = "";
        isMovedPlate = false;
        whole.transform.position = wholeposition;
        if (list != null)
        {
            foreach (GameObject child in list)
            {
                Destroy(child);
            }
            list.Clear();
        }
    }
    /// <summary>
    /// 判断是否获胜
    /// </summary>
    /// <returns></returns>
    public bool IsWin()
    {
        return platesNumber == platesStackC.Count;
    }
    /// <summary>
    /// 判断是否允许移动盘子
    /// </summary>
    /// <param name="start">开始移动的柱子</param>
    /// <param name="end">最终移动的柱子</param>
    /// <returns></returns>
    public bool IsAllowMove(string end)
    {
        int startname = 0;
        int endname = 0;
        switch (movedcolumn)
        {
            case "A": startname = int.Parse(platesStackA.Peek().name); break;
            case "B": startname = int.Parse(platesStackB.Peek().name); break;
            case "C": startname = int.Parse(platesStackC.Peek().name); break;
        }
        switch (end)
        {
            case "A":
                if (platesStackA.Count > 0)
                {
                    endname = int.Parse(platesStackA.Peek().name);
                }
                else
                {
                    endname = -1;
                }
                break;
            case "B":
                if (platesStackB.Count > 0)
                {
                    endname = int.Parse(platesStackB.Peek().name);
                }
                else
                {
                    endname = -1;
                }
                break;
            case "C":
                if (platesStackC.Count > 0)
                {
                    endname = int.Parse(platesStackC.Peek().name);
                }
                else
                {
                    endname = -1;
                }
                break;
        }
        if (startname > endname)
        {
            isAllowMove = true;
            return true;
        }
        else
        {
            isAllowMove = false;
            return false;
        }
    }
    /// <summary>
    /// 移动盘子从X-Y
    /// </summary>
    /// <param name="end"></param>
    public void MovePlateFromXToY(string end,bool Right = true,string errorStack=null)
    {
        if (isAllowMove && isMoveEnd)
        {
            //  AudioController.Instance.PlayMove();
            switch (movedcolumn)
            {
                case "A":
                    switch (end)
                    {
                        case "A":
                            ReturnPosition();
                            Right = true;
                            break;
                        case "B":
                            //执行A-B
                            movedGameObject.transform.DOMove(bTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(bTransform.position.x, plates[4 - platesStackB.Count].transform.position.y, bTransform.position.z), 1f, 1f));
                            platesStackB.Push(platesStackA.Pop());
                            //将A的栈顶元素弹出栈，放入B中
                            break;
                        case "C":
                            //执行A-C
                            movedGameObject.transform.DOMove(cTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(cTransform.position.x, plates[4 - platesStackC.Count].transform.position.y, cTransform.position.z), 1f, 1f));
                            platesStackC.Push(platesStackA.Pop());
                            break;
                    }
                    break;
                case "B":
                    switch (end)
                    {
                        case "A":
                            //执行B-A
                            movedGameObject.transform.DOMove(aTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(aTransform.position.x, plates[4 - platesStackA.Count].transform.position.y, aTransform.position.z), 1f, 1f));
                            platesStackA.Push(platesStackB.Pop());
                            break;
                        case "B":
                            ReturnPosition();
                            Right = true;
                            break;
                        case "C":
                            //执行B-C
                            movedGameObject.transform.DOMove(cTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(cTransform.position.x, plates[4 - platesStackC.Count].transform.position.y, cTransform.position.z), 1f, 1f));
                            platesStackC.Push(platesStackB.Pop());
                            break;
                    }
                    break;
                case "C":
                    switch (end)
                    {
                        case "B":
                            //执行C-B
                            movedGameObject.transform.DOMove(bTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(bTransform.position.x, plates[4 - platesStackB.Count].transform.position.y, bTransform.position.z), 1f, 1f));
                            platesStackB.Push(platesStackC.Pop());
                            break;
                        case "A":
                            //执行C-A
                            movedGameObject.transform.DOMove(aTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(aTransform.position.x, plates[4 - platesStackA.Count].transform.position.y, aTransform.position.z), 1f, 1f));
                            platesStackA.Push(platesStackC.Pop());
                            break;
                        case "C":
                            ReturnPosition();
                            Right = true;
                            break;
                    }
                    break;
            }
            StartCoroutine(IsMoveEnd(2, Right,errorStack,end));
            //HanoiIndicateController.Instance.Add();
            //movedGameObject = null;
            isMovedPlate = false;
            if (IsWin())
            {
                // AudioController.Instance.PlaySuccess();
            }
        }
    }

    /// <summary>
    /// 移动盘子到Top
    /// </summary>
    /// <param name="column"></param>
    /// 
    //movedGameObject.transform.DOMove(bTransform.position, 1f);
    //StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(bTransform.position.x, plates[4 - platesStackB.Count].transform.position.y, bTransform.position.z), 1f, 1f));
    //platesStackB.Push(platesStackA.Pop());
    //END
    public void MovePlatToEnd(string end)
    {
        if (isAllowMove && isMoveEnd)
        {
            //  AudioController.Instance.PlayMove();
            switch (movedcolumn)
            {
                case "A":
                    switch (end)
                    {
                        case "A":
                            movedGameObject.transform.DOMove(new Vector3(aTransform.position.x, plates[4 - platesStackB.Count].transform.position.y, aTransform.position.z), 1f);

                            break;
                        case "B":
                            //执行A-B
                            movedGameObject.transform.DOMove(bTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(bTransform.position.x, plates[4 - platesStackB.Count].transform.position.y, bTransform.position.z), 1f, 1f));
                            platesStackB.Push(platesStackA.Pop());
                            //将A的栈顶元素弹出栈，放入B中
                            break;
                        case "C":
                            //执行A-C
                            movedGameObject.transform.DOMove(cTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(cTransform.position.x, plates[4 - platesStackC.Count].transform.position.y, cTransform.position.z), 1f, 1f));
                            platesStackC.Push(platesStackA.Pop());
                            break;
                    }
                    break;
                case "B":
                    switch (end)
                    {
                        case "A":
                            //执行B-A
                            movedGameObject.transform.DOMove(aTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(aTransform.position.x, plates[4 - platesStackA.Count].transform.position.y, aTransform.position.z), 1f, 1f));
                            platesStackA.Push(platesStackB.Pop());
                            break;
                        case "C":
                            //执行B-C
                            movedGameObject.transform.DOMove(cTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(cTransform.position.x, plates[4 - platesStackC.Count].transform.position.y, cTransform.position.z), 1f, 1f));
                            platesStackC.Push(platesStackB.Pop());
                            break;
                    }
                    break;
                case "C":
                    switch (end)
                    {
                        case "B":
                            //执行C-B
                            movedGameObject.transform.DOMove(bTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(bTransform.position.x, plates[4 - platesStackB.Count].transform.position.y, bTransform.position.z), 1f, 1f));
                            platesStackB.Push(platesStackC.Pop());
                            break;
                        case "A":
                            //执行C-A
                            movedGameObject.transform.DOMove(aTransform.position, 1f);
                            StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(aTransform.position.x, plates[4 - platesStackA.Count].transform.position.y, aTransform.position.z), 1f, 1f));
                            platesStackA.Push(platesStackC.Pop());
                            break;
                    }
                    break;
            }
            StartCoroutine(IsMoveEnd(2));
            //HanoiIndicateController.Instance.Add();
            //movedGameObject = null;
            isMovedPlate = false;
            if (IsWin())
            {
                // AudioController.Instance.PlaySuccess();
            }
        }
    }
    //TOP
    public void MovePlateToTop(string column,bool Right=true)
    {
        if (isMoveEnd)
        {
            switch (column)
            {
                case "A":
                    if (platesStackA.Count > 0)
                    {
                        positionPlate = platesStackA.Peek().transform.position;
                        platesStackA.Peek().transform.DOMove(aTransform.position, 1f, false);
                        movedGameObject = platesStackA.Peek();//这个就是要移动的物体
                        movedcolumn = "A";
                    }
                    break;
                case "B":
                    if (platesStackB.Count > 0)
                    {
                        positionPlate = platesStackB.Peek().transform.position;
                        platesStackB.Peek().transform.DOMove(bTransform.position, 1f);
                        movedGameObject = platesStackB.Peek();
                        movedcolumn = "B";
                    }
                    break;
                case "C":
                    if (platesStackC.Count > 0)
                    {
                        positionPlate = platesStackC.Peek().transform.position;
                        platesStackC.Peek().transform.DOMove(cTransform.position, 1f);
                        movedGameObject = platesStackC.Peek();
                        movedcolumn = "C";
                    }
                    break;
            }
            isMovedPlate = true;
            StartCoroutine(IsMoveEnd(1,Right));
        }

    }
    /// <summary>
    /// 返回原来的位置
    /// </summary>
    public void ReturnPosition()
    {
        Debug.Log(isMoveEnd);
        if (isMoveEnd)
        {
            //AudioController.Instance.PlayFailed();
            movedGameObject.transform.DOMove(positionPlate, 1f);
            isMovedPlate = false;
            StartCoroutine(IsMoveEnd(1));
        }
    }
    /// <summary>
    /// 设置颜色
    /// </summary>
    /// <param name="column"></param>
    /// <param name="colorcount"></param>
    public void SetSignColor(string column, int colorcount)
    {
        switch (column)
        {
            case "A":
                aSign.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                aSign.GetComponent<MeshRenderer>().material = color[colorcount];
                break;
            case "B":
                bSign.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                bSign.GetComponent<MeshRenderer>().material = color[colorcount];
                break;
            case "C":
                cSign.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                cSign.GetComponent<MeshRenderer>().material = color[colorcount];
                break;
        }
    }
    /// <summary>
    /// 将颜色设为空
    /// </summary>
    /// <param name="column"></param>
    public void SetSignNoColor(string column)
    {
        switch (column)
        {
            case "A":
                aSign.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                break;
            case "B":
                bSign.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                break;
            case "C":
                cSign.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                break;
        }
    }
    /// <summary>
    /// 改变状态到整体模式
    /// </summary>
    public void ChangeToWhole()
    {
        isSignle = false;
        int count = platesStackA.Count - 1;
        for (int i = 0; i < count; i++)
        {
            platesStackA.Pop().transform.parent = whole.transform;
        }
        platesStackA.Push(whole);
    }
    /// <summary>
    /// 改变状态到单个模式
    /// </summary>
    public void ChangeToSignle()
    {
        isSignle = true;
    }
    /// <summary>
    /// 返回是否移动盘子
    /// </summary>
    /// <returns></returns>
    public bool GetIsMovedPlate()
    {
        return isMovedPlate;
    }
    /// <summary>
    /// 自动播放汉诺塔
    /// </summary>
    public void AutoPlayHanoi()
    {
        Manua.interactable = false;
        isManual = false;
        hanoistep.Clear();
        hanoi(platesNumber, "A", "B", "C");
        //if (isSignle)
        //{
        //    hanoi(platesNumber, "A", "B", "C");
        //}
        //else
        //{
        //    hanoi(2, "A", "B", "C");
        //}
        hanoistepcount = 0;
        StartCoroutine(AutoPlay());
        colliderA.GetComponent<CapsuleCollider>().enabled = false;
        colliderB.GetComponent<CapsuleCollider>().enabled = false;
        colliderC.GetComponent<CapsuleCollider>().enabled = false;
    }
    /// <summary>
    /// 手动调整汉诺塔
    /// </summary>
    public void ManualPlayHanoi()
    {
        //123
        hanoistep.Clear();
        hanoi(platesNumber, "A", "B", "C");
        AUTO.interactable = false;
        isManual = true;
        InitPlates(platesNumber);
        colliderA.GetComponent<CapsuleCollider>().enabled = true;
        colliderB.GetComponent<CapsuleCollider>().enabled = true;
        colliderC.GetComponent<CapsuleCollider>().enabled = true;
    }

    /// <summary>
    /// 延迟移动
    /// </summary>
    /// <param name="target"></param>
    /// <param name="endValue"></param>
    /// <param name="time"></param>
    /// <param name="delaytime"></param>
    /// <returns></returns>
    /// StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(bTransform.position.x, plates[4 - platesStackB.Count].transform.position.y, bTransform.position.z), 1f, 1f));

    IEnumerator DelayMove(Transform target, Vector3 endValue, float time, float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        target.DOMove(endValue, time);
    }

    IEnumerator Delay(string first, GameObject temp)
    {
        
        yield return new WaitForSeconds(1f);
        switch (first)
        {
            case "A":
                movedGameObject.transform.DOMove(aTransform.position, 1f, false);
                platesStackA.Push(temp);
                Debug.Log(".......A.........");
                break;
            case "B":
                movedGameObject.transform.DOMove(bTransform.position, 1f, false);
                platesStackB.Push(temp);
                Debug.Log("......B.........");
                break;
            case "C":
                 movedGameObject.transform.DOMove(cTransform.position, 1f, false);                   
                platesStackC.Push(temp);
                Debug.Log("......C........");
                break;
        }
    }
    IEnumerator DelayLight(string t,float delaytime)
    {
        Debug.LogError("亮灯");
        light_reminder(t);
        yield return new WaitForSeconds(delaytime);
        light_close(t);
    }

    /// <summary>
    /// 二次移动判断
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator IsMoveEnd(float time, bool right = true, string first = null,string errorStack=null)
    {
        //错误的情况
        //A-B 平移 下落
        //反过来 B-A 上升 平移
        //positionPlate = platesStackA.Peek().transform.position;
        //platesStackA.Peek().transform.DOMove(aTransform.position, 1f, false);
        //movedGameObject = platesStackA.Peek();//这个就是要移动的物体
        //movedcolumn = "A";

        //movedGameObject.transform.DOMove(bTransform.position, 1f);
        //StartCoroutine(DelayMove(movedGameObject.transform, new Vector3(bTransform.position.x, plates[4 - platesStackB.Count].transform.position.y, bTransform.position.z), 1f, 1f));
        //platesStackB.Push(platesStackA.Pop());
        GameObject temp=null;
        isMoveEnd = false;
        yield return new WaitForSeconds(time);
        isMoveEnd = true;
        
        if(right==false&& errorStack!=null&& first!=null)
        {
            switch (errorStack)
            {
                case "A":
                    movedGameObject.transform.DOMove(aTransform.position, 1f, false);
                    temp=platesStackA.Pop();
                    Debug.Log("A");
                    break;
                case "B":
                    movedGameObject.transform.DOMove(bTransform.position, 1f, false);
                    temp = platesStackB.Pop();
                    Debug.Log("B");
                    break;
                case "C":
                    movedGameObject.transform.DOMove(cTransform.position, 1f, false);
                    temp = platesStackC.Pop();
                    Debug.Log("C");
                    break;
            }
            Debug.LogError("等待");
            StartCoroutine(Delay(first,temp));
         

            //Debug.LogError("执行");
            
        }
        else if(right == false&&first==null&&errorStack==null)
        {
            ReturnPosition();
        }
    }
    /// <summary>
    /// 自动播放动画
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoPlay()
    {        
        light_reminder(hanoistep[hanoistepcount].first.ToString());
        yield return new WaitForSeconds(0.5f);
        if (hanoistepcount < hanoistep.Count)
        {
            MovePlateToTop(hanoistep[hanoistepcount].first);
            //Debug.LogError(hanoistep[hanoistepcount].end.ToString());
            light_close(hanoistep[hanoistepcount].first.ToString());
            light_reminder(hanoistep[hanoistepcount].end.ToString());
            yield return new WaitForSeconds(2f);
            isAllowMove = true;
            MovePlateFromXToY(hanoistep[hanoistepcount].end);
            yield return new WaitForSeconds(2f);
            light_close(hanoistep[hanoistepcount].end.ToString());
            hanoistepcount++;
            StartCoroutine(AutoPlay());
        }
        else
        {
            yield return null;
        }
    }

    void light_close(string t)
    {
        switch (t)
        {
            case "A":
                lights[0].gameObject.SetActive(false);
                //lights[0].color = new Color(100, 110, 100);
                break;
            case "B":
                lights[1].gameObject.SetActive(false);
                break;
            case "C":
                lights[2].gameObject.SetActive(false);
                break;
        }
    }
    void light_reminder(string t)
    {
        switch(t)
        {
            case "A":
                lights[0].gameObject.SetActive(true);
                //lights[0].color = new Color(100, 110, 100);
                break;
            case "B":
                lights[1].gameObject.SetActive(true);
                break;
            case "C":
                lights[2].gameObject.SetActive(true);
                break;
        }
    }
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="disks"></param>
    /// <param name="N"></param>
    /// <param name="M"></param>
    public void move(int disks, string N, string M)
    {
        HanoiStep hanoiStep = new HanoiStep();
        hanoiStep.first = N;
        hanoiStep.end = M;
        hanoistep.Add(hanoiStep);
        Debug.Log(" 把 " + disks + " 号圆盘从 " + N + " ->移到->  " + M);
    }
    /// <summary>
    /// 递归实现汉诺塔的函数
    /// </summary>
    /// <param name="n"></param>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <param name="C"></param>
    public void hanoi(int n, string A, string B, string C)
    {
        if (n == 1)//圆盘只有一个时，只需将其从A塔移到C塔
        {
            move(1, A, C);//将编号为1的圆盘从A移到C
        }
        else
        {//否则
            hanoi(n - 1, A, C, B);//递归，把A塔上编号1~n-1的圆盘移到B上，以C为辅助塔
            move(n, A, C);//把A塔上编号为n的圆盘移到C上
            hanoi(n - 1, B, A, C);//递归，把B塔上编号1~n-1的圆盘移到C上，以A为辅助塔
        }
    }


}

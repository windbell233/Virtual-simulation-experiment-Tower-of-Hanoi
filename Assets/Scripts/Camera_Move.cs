using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Camera_Move : MonoBehaviour
{
    public testhanoi Testhanoi;
    public GameObject anim_left;
    public GameObject anim_right;
    public Gradient gradient;
    public GameObject temp_hit;
    public DOTweenPath path1;
    public DOTweenPath path2;
    public DOTweenPath[] path=new DOTweenPath[2];
    private Vector3 anim_left_pos;
    private Vector3 anim_right_pos;
    public Transform left;
    public Transform right;

    private void Awake()
    {
        anim_left_pos  = anim_left.transform.position;
        anim_right_pos = anim_right.transform.position;
      
    }
    public void Update()
    {
        /*
            if (Input.GetMouseButtonDown(0))
            {
        #if UNITY_ANDROID || UNITY_IPHONE
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        #else
                if (EventSystem.current.IsPointerOverGameObject())
        #endif
                    Debug.Log("当前触摸在UI上");
                else
                    Debug.Log("当前没有触摸在UI上");
            }
         */
       
        if (Input.GetMouseButtonDown(0))
        {
           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 撞击到了哪个3D物体
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                temp_hit = hit.collider.gameObject;
                Debug.Log("NAME====" + temp_hit.name);
                
                if (hit.collider.gameObject.layer == 11)
                {
                   switch (temp_hit.name)
                    {
                        case "shiyan":
                            path[0].DOPlayForward();
                            StartCoroutine(IsMoveEnd(path1.duration)); //这个时动画播放完时间之后，弹出菜单
                            break;
                        case "shugui":
                            path[1].DOPlayForward();
                            break;

                    }                                                                            
                }
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            Debug.LogError("退出退出退出退出退出退出退出退出退出退出退出退出退出");
            if (temp_hit == null)
                return;
            Debug.LogWarning(temp_hit.name);
             switch (temp_hit.name)
             {                 
                case "shugui":
                    path[1].DOPlayBackwards();
                    temp_hit = null;
                    break;

             }            
                            
        }
        
        
    }
    IEnumerator IsMoveEnd(float time)
    {        
        yield return new WaitForSeconds(time);
        Anim_Left();
        Anim_Right();
    }
    public void ExitHanni()
    {
        
        temp_hit = null;
        Testhanoi.Close();
        //anim_left.transform.DOLocalMove(new Vector3(550, 0, 0), 1);
        anim_left.transform.DOMove(anim_left_pos, 1);
        anim_right.transform.DOMove(anim_right_pos, 1);
        path1.DOPlayBackwards();

    }
    

    public void Anim_Left()
    {
        Tweener tweener = anim_left.transform.DOMove(left.position,1);
        tweener.SetEase(Ease.OutBounce);//动画曲线
        //tweener.OnComplete(OnTweenComplete);//动画结束事件
        //tweener.SetLoops(10);//循环10次
    }
    public void Anim_Right()
    {
        Tweener tweener = anim_right.transform.DOMove(right.position,1);

        tweener.SetEase(Ease.OutBounce);//动画曲线
        //tweener.OnComplete(OnTweenComplete);//动画结束事件
        //tweener.SetLoops(10);//循环10次
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Building : MonoBehaviour
{

    public GameObject cube;

    private Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.Find("Base").transform;
    }

    // Update is called once per frame

    float startTime;
    Vector3 fingerUpPos;
    Vector3 fingerDownPos;
    void Update()
    {

        if (Input.touchCount > 0) {
           
            Touch touch = Input.GetTouch(0);

          

            if (touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                fingerUpPos = touch.position;
                fingerDownPos = touch.position;
            }


            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPos = touch.position;
                if (Time.time - startTime < 0.2f) {
                    Vector3 touchPos = touch.position;
                    Ray ray = Camera.main.ScreenPointToRay(touchPos);


                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo)) {
                        Vector3 hitWorldPosition = new Vector3(Mathf.Round(hitInfo.point.x + hitInfo.normal.x / 2), Mathf.Round(hitInfo.point.y + hitInfo.normal.y / 2), Mathf.Round(hitInfo.point.z + hitInfo.normal.z / 2));

                        Vector3 localHitPosition = parent.InverseTransformPoint(hitWorldPosition);

                        Vector3 localBuildPosition = new Vector3(Mathf.Round(localHitPosition.x * 5) / 5, Mathf.Round(localHitPosition.y), Mathf.Round(localHitPosition.z * 5) / 5);

                        Instantiate(cube, parent.TransformPoint(localBuildPosition), transform.rotation, parent);
                    }
                } else if ((VerticalMoveValue() + HorizontalMoveValue()) < 40) {
                    Vector3 touchPos = touch.position;
                    Ray ray = Camera.main.ScreenPointToRay(touchPos);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        Debug.Log(hitInfo.transform.name);
                        if (hitInfo.transform.parent.name == "Base") {
                            Destroy(hitInfo.transform.gameObject);
                        }                        
                    }
                }
            }
        }
    }
    float VerticalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
    }
    float HorizontalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
    }

}

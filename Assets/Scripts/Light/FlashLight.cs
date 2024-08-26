using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashLight : MonoBehaviour
{
    //组件
    private Light2D flashLight;//SpotLight

    //bool
    public bool isSpotLight;//是否聚光

    //数据
    private float originalPointLightOuterAngle;
    private float currentPointLightOuterAngle;

    public float raduisInner;
    public float raduisOuter;

    public LayerMask layermask;

    private float timer;
    public float triggerTime;

    private void Awake()
    {
        flashLight = GetComponent<Light2D>();

        originalPointLightOuterAngle = flashLight.pointLightOuterAngle;
        currentPointLightOuterAngle = originalPointLightOuterAngle;

        raduisInner = flashLight.pointLightInnerRadius;
        raduisOuter = flashLight.pointLightOuterRadius;
    }

    private void Update()
    {
        //Debug.DrawRay(this.transform.position, this.transform.up * flashLight.pointLightOuterRadius, Color.red);
        SpotLight();
        ChangeFlashLight();
        RaycastCheck();
    }

    //是否为聚光状态
    public void SpotLight()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            isSpotLight = !isSpotLight;
            flashLight.enabled = isSpotLight;
        }
    }

    //改变手电筒的焦距
    public void ChangeFlashLight()
    {
        if (!isSpotLight) return;//不是聚光状态

        flashLight.pointLightInnerAngle = 0;

        if (Input.GetKey(KeyCode.Y))
        {
            //增加扇形范围
            if(currentPointLightOuterAngle < 80f)
            {
                currentPointLightOuterAngle += 1f;

                //调整Raduis
                raduisInner -= 0.05f;
                raduisOuter -= 0.05f;
            }
        }

        if (Input.GetKey(KeyCode.I))
        {
            //缩小扇形范围
            if(currentPointLightOuterAngle > 2f)
            {
                currentPointLightOuterAngle -= 1f;

                //调整Raduis
                raduisInner += 0.05f;
                raduisOuter += 0.05f;
            }
        }

        //更新范围
        flashLight.pointLightOuterAngle = Mathf.Clamp(currentPointLightOuterAngle, 2f, 80f);
        flashLight.pointLightInnerRadius = Mathf.Clamp(raduisInner, 0f, 4f);
        flashLight.pointLightOuterRadius = Mathf.Clamp(raduisOuter, 6f, 10f);
    }

    //射线检测
    public void RaycastCheck()
    {
        if (!isSpotLight) return;

        //TODO:根据人物的朝向修改方向

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.up, 
            flashLight.pointLightOuterRadius, layermask);

        if (hit.collider != null && hit.collider.CompareTag("organs"))
        {
            timer += Time.deltaTime;
            if(timer >= triggerTime)
            {
                //触发机关
                Debug.Log("organs");
            }
        }
    }
}

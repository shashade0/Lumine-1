using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashLight : MonoBehaviour
{
    [Header("�����ȡ")]
    private Light2D flashLight;//SpotLight
    public ShadowTargetData shadowTargetData;

    [Header("״̬���")]
    public bool isSpotLight;//�Ƿ�۹�

    [Header("��������")]
    private float originalPointLightOuterAngle;
    private float currentPointLightOuterAngle;

    public float raduisInner;
    public float raduisOuter;

    //public LayerMask layermask;

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

    /// <summary>
    /// �Ƿ�Ϊ�۹�״̬
    /// </summary>
    public void SpotLight()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            isSpotLight = !isSpotLight;
            flashLight.enabled = isSpotLight;
        }
    }

    /// <summary>
    /// �ı��ֵ�Ͳ�Ľ���
    /// </summary>
    public void ChangeFlashLight()
    {
        if (!isSpotLight) return;//���Ǿ۹�״̬

        flashLight.pointLightInnerAngle = 0;

        if (Input.GetKey(KeyCode.Y))
        {
            //�������η�Χ
            if(currentPointLightOuterAngle < 80f)
            {
                currentPointLightOuterAngle += 1f;

                //����Raduis
                raduisInner -= 0.05f;
                raduisOuter -= 0.05f;
            }
        }

        if (Input.GetKey(KeyCode.I))
        {
            //��С���η�Χ
            if(currentPointLightOuterAngle > 2f)
            {
                currentPointLightOuterAngle -= 1f;

                //����Raduis
                raduisInner += 0.05f;
                raduisOuter += 0.05f;
            }
        }

        //���·�Χ
        flashLight.pointLightOuterAngle = Mathf.Clamp(currentPointLightOuterAngle, 2f, 80f);
        flashLight.pointLightInnerRadius = Mathf.Clamp(raduisInner, 0f, 4f);
        flashLight.pointLightOuterRadius = Mathf.Clamp(raduisOuter, 6f, 10f);
    }

    /// <summary>
    /// ���߼��
    /// </summary>
    public void RaycastCheck()
    {
        if (!isSpotLight) return;

        //TODO:��������ĳ����޸ķ���

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.up, 
            flashLight.pointLightOuterRadius);

        //������
        if (hit.collider != null && hit.collider.CompareTag("Organ"))
        {
            timer += Time.deltaTime;
            if(timer >= triggerTime)
            {
                //��������
                Debug.Log("organs");
            }
        }

        //���Ŀ�������Ƿ��ھ۹����(������ӰShadow
        if(hit.collider != null && hit.collider.CompareTag("ShadowTarget"))
        {
            //�ڱ�����
            Vector3 tmp_Dir_1 = new Vector3(hit.collider.gameObject.transform.GetChild(0).position.x, this.transform.position.y) 
                - this.transform.position;
            //б������
            Vector3 tmp_Dir_2 = hit.collider.gameObject.transform.GetChild(0).position - this.transform.position;

            if (Mathf.Cos(flashLight.pointLightOuterAngle / 2 * Mathf.Deg2Rad) <= tmp_Dir_1.magnitude / tmp_Dir_2.magnitude)
            {
                //��ʾ��Ӱ
                if(hit.collider.gameObject.transform.childCount == 3 && 
                    hit.collider.gameObject.transform.GetChild(2).GetComponent<Shadow>() != null)
                {

                    hit.collider.transform.GetChild(2).gameObject.SetActive(true);
                }
                else
                {
                    //����
                    CreateShadow(hit.collider.gameObject);
                }
            }
            //��������Ӱ�ӷ�Χ��
            else
            {
                //Debug.Log("������Ӱ");
                if(hit.collider.gameObject.transform.childCount == 3 && 
                    hit.collider.gameObject.transform.GetChild(2).GetComponent<Shadow>() != null)
                {
                    hit.collider.transform.GetChild(2).gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// ���ɵ�ǰ�����Ӱ��
    /// </summary>
    public void CreateShadow(GameObject _shadowTargetGameObject)
    {
        for(int i = 0; i < shadowTargetData.shadowTargetsList.Count; i++)
        {   
            //ƥ������
            if(_shadowTargetGameObject.name == shadowTargetData.shadowTargetsList[i].shadowTargetName)
            {
                //�����Դ��ShadowTarget�ľ���
                float tmp_Distance = Vector2.Distance(this.transform.position, 
                    _shadowTargetGameObject.transform.GetChild(1).transform.position);

                //Shadow��ʼλ��(TODO:���λ�û���Ҫ�޸�
                Vector2 tmp_ShadowBornPosition = new Vector2(Mathf.Abs(tmp_Distance), -this.transform.position.y);

                //ʵ��������Shadow
                GameObject tmp_Shadow = GameObject.Instantiate(shadowTargetData.shadowTargetsList[i].shadowTargetPrefab,
                    _shadowTargetGameObject.transform);

                tmp_Shadow.transform.localPosition = tmp_ShadowBornPosition;

                //���Shadow�ű�
                //Shadow tmp_shadow = tmp_Obj.AddComponent<Shadow>(); 
                tmp_Shadow.AddComponent<Shadow>();
            }
        }
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering.Universal;

public class FlashLight : MonoBehaviour
{
    [Header("�����ȡ")]
    private Light2D flashLight;//SpotLight
    private CircleCollider2D coll;
    public ShadowTargetData shadowTargetData;

    [Header("״̬���")]
    public LightState currrentLightState = LightState.LightDown; //�Ƿ�۹�

    [Header("��������")]
    private float originalPointLightOuterAngle; //ԭʼ��Ȧ�Ƕ�
    private float currentPointLightOuterAngle; //��ǰ��Ȧ�Ƕ�
    private float originalPointLightInnerAngle; //ԭʼ��Ȧ�Ƕ�
    private float currentPointLightInnerAngle; //��ǰ��Ȧ�Ƕ�


    public float raduisInner;
    public float raduisOuter;

    public LayerMask shadow;

    private float timer;
    public float triggerTime;

    private List<GameObject> currentSceneShadowList = new List<GameObject>();//��ǰ��������Ӱ����


    private void Awake()
    {
        flashLight = GetComponent<Light2D>();
        coll = GetComponent<CircleCollider2D>();

        originalPointLightOuterAngle = flashLight.pointLightOuterAngle;
        currentPointLightOuterAngle = originalPointLightOuterAngle;

        raduisInner = flashLight.pointLightInnerRadius;
        raduisOuter = flashLight.pointLightOuterRadius;

        //�ر�����Ӱ��
        CloseAllShadow();
        flashLight.enabled = false;
        coll.enabled = false;
    }

    private void Update()
    {
        //Debug.DrawRay(this.transform.position, this.transform.up * flashLight.pointLightOuterRadius, Color.red);

        //Quaternion quaternion_Top1 = Quaternion.AngleAxis(flashLight.pointLightOuterAngle / 4, new Vector3(0, 0, 1));
        //Vector2 tmp_Dir_Top_1 = quaternion_Top1 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        //Debug.DrawRay(this.transform.position, tmp_Dir_Top_1, Color.green);

        //Quaternion quaternion_Top2 = Quaternion.AngleAxis(flashLight.pointLightOuterAngle / 8, new Vector3(0, 0, 1));
        //Vector2 tmp_Dir_Top_2 = quaternion_Top2 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        //Debug.DrawRay(this.transform.position, tmp_Dir_Top_2, Color.green);

        //Quaternion quaternion_Top3 = Quaternion.AngleAxis(flashLight.pointLightOuterAngle / 8 * 3 , new Vector3(0, 0, 1));
        //Vector2 tmp_Dir_Top_3 = quaternion_Top3 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        //Debug.DrawRay(this.transform.position, tmp_Dir_Top_3, Color.green);


        Quaternion quaternion_Bottom_1 = Quaternion.AngleAxis(-flashLight.pointLightOuterAngle / 4, new Vector3(0, 0, 1));
        Vector2 tmp_Dir_Bottom_1 = quaternion_Bottom_1 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        Debug.DrawRay(this.transform.position, tmp_Dir_Bottom_1, Color.red);

        Quaternion quaternion_Bottom_2 = Quaternion.AngleAxis(-flashLight.pointLightOuterAngle / 8, new Vector3(0, 0, 1));
        Vector2 tmp_Dir_Bottom_2 = quaternion_Bottom_2 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        Debug.DrawRay(this.transform.position, tmp_Dir_Bottom_2, Color.red);

        //Quaternion quaternion_Bottom_3 = Quaternion.AngleAxis(-flashLight.pointLightOuterAngle / 8 * 3, new Vector3(0, 0, 1));
        //Vector2 tmp_Dir_Bottom_3 = quaternion_Bottom_3 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        //Debug.DrawRay(this.transform.position, tmp_Dir_Bottom_3, Color.red);

        CheckLightState();
        ChangeFlashLight();
        RaycastCheck();
        ChangeLightUp(); 
    }

    /// <summary>
    /// ������ĵƹ�״̬
    /// </summary>
    public void CheckLightState()
    {
        //���ؾ۹�״̬
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (currrentLightState == LightState.LightDown)
                currrentLightState = LightState.SpotLight;
            else if (currrentLightState == LightState.SpotLight)
                currrentLightState = LightState.LightDown;

            if (currrentLightState == LightState.LightDown)
            {
                //�ر�����Ӱ��
                CloseAllShadow();
                flashLight.enabled = false;
                coll.enabled = true;
            }
            else
            {
                flashLight.enabled = true;
                coll.enabled = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            if (currrentLightState == LightState.LightDown)
                currrentLightState = LightState.LightUp;
            else if (currrentLightState == LightState.LightUp)
                currrentLightState = LightState.LightDown;

            if (currrentLightState == LightState.LightDown)
            {
                //�ر�����Ӱ��
                CloseAllShadow();
                flashLight.enabled = false;
                coll.enabled = true;
            }
            else
            {
                flashLight.enabled = true;
                coll.enabled = true;
            }
        }
    }

    #region �ƹ�۽�״̬
    /// <summary>
    /// �ı��ֵ�Ͳ�Ľ���
    /// </summary>
    public void ChangeFlashLight()
    {
        if (currrentLightState != LightState.SpotLight) return;//���Ǿ۹�״̬

        flashLight.pointLightInnerAngle = 0;

        if (Input.GetKey(KeyCode.Y))
        {
            //�������η�Χ
            if (currentPointLightOuterAngle < 80f)
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
            if (currentPointLightOuterAngle > 2f)
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
        if (currrentLightState != LightState.SpotLight) return;

        //TODO:��������ĳ����޸ķ���

        RaycastHit2D hit_Center = Physics2D.Raycast(this.transform.position, this.transform.up,
            flashLight.pointLightOuterRadius, shadow);

        //Quaternion quaternion_Top_1 = Quaternion.AngleAxis(flashLight.pointLightOuterAngle / 4, new Vector3(0, 0, 1));
        //Vector2 tmp_Dir_Top_1 = quaternion_Top_1 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        //RaycastHit2D hit_Top_1 = Physics2D.Raycast(this.transform.position, tmp_Dir_Top_1.normalized, 
        //    flashLight.pointLightOuterRadius);

        //Quaternion quaternion_Top_2 = Quaternion.AngleAxis(flashLight.pointLightOuterAngle / 8, new Vector3(0, 0, 1));
        //Vector2 tmp_Dir_Top_2 = quaternion_Top_2 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        //RaycastHit2D hit_Top_2 = Physics2D.Raycast(this.transform.position, tmp_Dir_Top_2.normalized, 
        //    flashLight.pointLightOuterRadius);

        //Quaternion quaternion_Top_3 = Quaternion.AngleAxis(flashLight.pointLightOuterAngle / 8 * 3, new Vector3(0, 0, 1));
        //Vector2 tmp_Dir_Top_3 = quaternion_Top_3 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        //RaycastHit2D hit_Top_3 = Physics2D.Raycast(this.transform.position, tmp_Dir_Top_3.normalized, 
        //    flashLight.pointLightOuterRadius);

        Quaternion quaternion_Bottom_1 = Quaternion.AngleAxis(-flashLight.pointLightOuterAngle / 4, new Vector3(0, 0, 1));
        Vector2 tmp_Dir_Bottom_1 = quaternion_Bottom_1 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        RaycastHit2D hit_Bottom_1 = Physics2D.Raycast(this.transform.position, tmp_Dir_Bottom_1.normalized,
            flashLight.pointLightOuterRadius, shadow);

        Quaternion quaternion_Bottom_2 = Quaternion.AngleAxis(-flashLight.pointLightOuterAngle / 8, new Vector3(0, 0, 1));
        Vector2 tmp_Dir_Bottom_2 = quaternion_Bottom_2 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        RaycastHit2D hit_Bottom_2 = Physics2D.Raycast(this.transform.position, tmp_Dir_Bottom_2.normalized,
            flashLight.pointLightOuterRadius, shadow);

        //Quaternion quaternion_Bottom_3 = Quaternion.AngleAxis(-flashLight.pointLightOuterAngle / 8 * 3, new Vector3(0, 0, 1));
        //Vector2 tmp_Dir_Bottom_3 = quaternion_Bottom_3 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
        //RaycastHit2D hit_Bottom_3 = Physics2D.Raycast(this.transform.position, tmp_Dir_Bottom_3.normalized, 
        //    flashLight.pointLightOuterRadius);

        //������
        if (hit_Center.collider != null && hit_Center.collider.CompareTag("Organ") && currrentLightState == LightState.SpotLight)
        {
            timer += Time.deltaTime;
            if (timer >= triggerTime)
            {
                //��������
                Debug.Log("organs");
            }
        }

        //���Ŀ�������Ƿ��ھ۹����(������ӰShadow
        //if(hit_Center.collider != null && hit_Center.collider.CompareTag("ShadowTarget"))

        //if (hit_Center.collider != null && hit_Center.collider.CompareTag("ShadowTarget") ||
        //   hit_Bottom_1.collider != null && hit_Bottom_1.collider.CompareTag("ShadowTarget") ||
        //   hit_Bottom_2.collider != null && hit_Bottom_2.collider.CompareTag("ShadowTarget") ||
        //   hit_Bottom_3.collider != null && hit_Bottom_3.collider.CompareTag("ShadowTarget"))
        //{
        //    //�ڱ߾���
        //    //float tmp_Distance = (new Vector3(hit.collider.gameObject.transform.GetChild(0).position.x, this.transform.position.y)
        //    //    - this.transform.position).magnitude;

        //    //Debug.Log("tmp_Distance: " + tmp_Distance);

        //    //�ڱ�����
        //    Vector3 tmp_Dir_1 = new Vector3(hit_Center.collider.gameObject.transform.GetChild(0).position.x, this.transform.position.y) 
        //        - this.transform.position;
        //    //б������
        //    Vector3 tmp_Dir_2 = hit_Center.collider.gameObject.transform.GetChild(0).position - this.transform.position;

        //    if (Mathf.Cos(flashLight.pointLightOuterAngle / 2 * Mathf.Deg2Rad) <= tmp_Dir_1.magnitude / tmp_Dir_2.magnitude)
        //    {
        //        //��ʾ��Ӱ
        //        if(hit_Center.collider.gameObject.transform.childCount == 3 && 
        //            hit_Center.collider.gameObject.transform.GetChild(2).GetComponent<Shadow>() != null)
        //        {

        //            hit_Center.collider.transform.GetChild(2).gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            //����
        //            CreateShadow(hit_Center.collider.gameObject);
        //        }
        //    }
        //    //��������Ӱ�ӷ�Χ��
        //    else
        //    {
        //        //Debug.Log("������Ӱ");
        //        if(hit_Center.collider.gameObject.transform.childCount == 3 && 
        //            hit_Center.collider.gameObject.transform.GetChild(2).GetComponent<Shadow>() != null)
        //        {
        //            Debug.Log("������Ӱ");

        //            hit_Center.collider.transform.GetChild(2).gameObject.SetActive(false);
        //        }
        //    }
        //}
        //else
        //{
        //    for(int i = 0; i < currentSceneShadowList.Count; i++)
        //    {
        //        currentSceneShadowList[i].gameObject.SetActive(false);
        //    }
        //}
        //}

        if (hit_Center.collider != null && hit_Center.collider.CompareTag("ShadowTarget") && currrentLightState == LightState.SpotLight)
        {
            AboutShadow(hit_Center);
        }
        else if (hit_Bottom_1.collider != null && hit_Bottom_1.collider.CompareTag("ShadowTarget") && currrentLightState == LightState.SpotLight)
        {
            AboutShadow(hit_Bottom_1);
        }
        else if (hit_Bottom_2.collider != null && hit_Bottom_2.collider.CompareTag("ShadowTarget") && currrentLightState == LightState.SpotLight)
        {
            AboutShadow(hit_Bottom_2);
        }
        //else if (hit_Bottom_3.collider != null && hit_Bottom_3.collider.CompareTag("ShadowTarget"))
        //{
        //    AboutShadow(hit_Bottom_3);
        //}
        else
        {
            //�ر�������Ӱ
            CloseAllShadow();
        }
    }

    public void AboutShadow(RaycastHit2D hit)
    {
        //�ڱ߾���
        //float tmp_Distance = (new Vector3(hit.collider.gameObject.transform.GetChild(0).position.x, this.transform.position.y)
        //    - this.transform.position).magnitude;

        //Debug.Log("tmp_Distance: " + tmp_Distance);

        //�ڱ�����
        Vector3 tmp_Dir_1 = new Vector3(hit.collider.gameObject.transform.GetChild(0).position.x, this.transform.position.y)
            - this.transform.position;
        //б������
        Vector3 tmp_Dir_2 = hit.collider.gameObject.transform.GetChild(0).position - this.transform.position;

        if (Mathf.Cos(flashLight.pointLightOuterAngle / 2 * Mathf.Deg2Rad) <= tmp_Dir_1.magnitude / tmp_Dir_2.magnitude)
        {
            //��ʾ��Ӱ
            if (hit.collider.gameObject.transform.childCount == 3 &&
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
            if (hit.collider.gameObject.transform.childCount == 3 &&
                hit.collider.gameObject.transform.GetChild(2).GetComponent<Shadow>() != null)
            {
                Debug.Log("������Ӱ");

                hit.collider.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ���ɵ�ǰ�����Ӱ��
    /// </summary>
    public void CreateShadow(GameObject _shadowTargetGameObject)
    {
        for (int i = 0; i < shadowTargetData.shadowTargetsList.Count; i++)
        {
            //ƥ������
            if (_shadowTargetGameObject.name == shadowTargetData.shadowTargetsList[i].shadowTargetName)
            {
                //�����Դ��ShadowTarget�ľ���
                float tmp_Distance = Vector2.Distance(this.transform.position,
                    _shadowTargetGameObject.transform.GetChild(1).transform.position);

                //Shadow��ʼλ��(TODO:���λ�û���Ҫ�޸�
                Vector2 tmp_ShadowBornPosition = new Vector2(Mathf.Abs(tmp_Distance), -this.transform.position.y);

                //ʵ��������Shadow
                GameObject tmp_Shadow = GameObject.Instantiate(shadowTargetData.shadowTargetsList[i].shadowTargetPrefab,
                    _shadowTargetGameObject.transform);

                //��ӽ���Ӱ�ļ���
                currentSceneShadowList.Add(tmp_Shadow);

                tmp_Shadow.transform.localPosition = tmp_ShadowBornPosition;

                //���Shadow�ű�
                //Shadow tmp_shadow = tmp_Obj.AddComponent<Shadow>(); 
                tmp_Shadow.AddComponent<Shadow>();
            }
        }
    }

    /// <summary>
    /// �ر����е���Ӱ
    /// </summary>
    public void CloseAllShadow()
    {
        for (int i = 0; i < currentSceneShadowList.Count; i++)
        {
            currentSceneShadowList[i].gameObject.SetActive(false);
        }
    }
    #endregion

    #region ��ͨ�ƹ�״̬
    public void ChangeLightUp()
    {
        if (currrentLightState == LightState.LightUp)
        {
            flashLight.pointLightOuterAngle = 360;
            flashLight.pointLightInnerAngle = 360;
        }
    }
    #endregion
}

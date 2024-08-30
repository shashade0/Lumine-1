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
    private Transform playerTransform;
    public Player player;

    [Header("״̬���")]
    public LightState currrentLightState = LightState.LightDown; //�Ƿ�۹�

    [Header("��������")]
    private float originalPointLightOuterAngle; //ԭʼ��Ȧ�Ƕ�
    private float currentPointLightOuterAngle; //��ǰ��Ȧ�Ƕ�
    private float originalPointLightInnerAngle; //ԭʼ��Ȧ�Ƕ�
    private float currentPointLightInnerAngle; //��ǰ��Ȧ�Ƕ�

    public float raduisInner;
    public float raduisOuter;

    public LayerMask shadow; //��Ӱ�����ͼ�㣬�����⵽����ͼ��

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

        if (player.facingDir == 1)
        {
            Quaternion quaternion_Bottom_1 = Quaternion.AngleAxis(-flashLight.pointLightOuterAngle / 4, new Vector3(0, 0, 1));
            Vector2 tmp_Dir_Bottom_1 = quaternion_Bottom_1 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
            Debug.DrawRay(this.transform.position, tmp_Dir_Bottom_1, Color.red);

            Quaternion quaternion_Bottom_2 = Quaternion.AngleAxis(-flashLight.pointLightOuterAngle / 8, new Vector3(0, 0, 1));
            Vector2 tmp_Dir_Bottom_2 = quaternion_Bottom_2 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
            Debug.DrawRay(this.transform.position, tmp_Dir_Bottom_2, Color.red);
        }
        else
        {
            Quaternion quaternion_Bottom_1 = Quaternion.AngleAxis(flashLight.pointLightOuterAngle / 4, new Vector3(0, 0, 1));
            Vector2 tmp_Dir_Bottom_1 = quaternion_Bottom_1 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
            Debug.DrawRay(this.transform.position, tmp_Dir_Bottom_1, Color.red);

            Quaternion quaternion_Bottom_2 = Quaternion.AngleAxis(flashLight.pointLightOuterAngle / 8, new Vector3(0, 0, 1));
            Vector2 tmp_Dir_Bottom_2 = quaternion_Bottom_2 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
            Debug.DrawRay(this.transform.position, tmp_Dir_Bottom_2, Color.red);
        }
            
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
            if (currrentLightState == LightState.LightDown || currrentLightState == LightState.LightUp)
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
            if (currrentLightState == LightState.LightDown || currrentLightState == LightState.SpotLight)
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

        RaycastHit2D hit_Center = Physics2D.Raycast(this.transform.position, this.transform.up,
            flashLight.pointLightOuterRadius, shadow);

        RaycastHit2D hit_Bottom_1;
        RaycastHit2D hit_Bottom_2;
        if (player.facingDir == 1)
        {
            Quaternion quaternion_Bottom_1 = Quaternion.AngleAxis(-flashLight.pointLightOuterAngle / 4, new Vector3(0, 0, 1));
            Vector2 tmp_Dir_Bottom_1 = quaternion_Bottom_1 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
            hit_Bottom_1 = Physics2D.Raycast(this.transform.position, tmp_Dir_Bottom_1.normalized,
                flashLight.pointLightOuterRadius, shadow);

            Quaternion quaternion_Bottom_2 = Quaternion.AngleAxis(-flashLight.pointLightOuterAngle / 8, new Vector3(0, 0, 1));
            Vector2 tmp_Dir_Bottom_2 = quaternion_Bottom_2 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
            hit_Bottom_2 = Physics2D.Raycast(this.transform.position, tmp_Dir_Bottom_2.normalized,
                flashLight.pointLightOuterRadius, shadow);
        }
        else
        {
            Quaternion quaternion_Bottom_1 = Quaternion.AngleAxis(flashLight.pointLightOuterAngle / 4, new Vector3(0, 0, 1));
            Vector2 tmp_Dir_Bottom_1 = quaternion_Bottom_1 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
            hit_Bottom_1 = Physics2D.Raycast(this.transform.position, tmp_Dir_Bottom_1.normalized,
                flashLight.pointLightOuterRadius, shadow);

            Quaternion quaternion_Bottom_2 = Quaternion.AngleAxis(flashLight.pointLightOuterAngle / 8, new Vector3(0, 0, 1));
            Vector2 tmp_Dir_Bottom_2 = quaternion_Bottom_2 * this.transform.up * flashLight.pointLightOuterRadius;//��ת�������
            hit_Bottom_2 = Physics2D.Raycast(this.transform.position, tmp_Dir_Bottom_2.normalized,
                flashLight.pointLightOuterRadius, shadow);
        }

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
        else
        {
            //�ر�������Ӱ
            CloseAllShadow();
        }
    }

    public void AboutShadow(RaycastHit2D hit)
    {
        ////�ڱ�����
        //Vector3 tmp_Dir_1 = new Vector3(hit.collider.gameObject.transform.GetChild(0).position.x, this.transform.position.y)
        //    - this.transform.position;
        ////б������
        //Vector3 tmp_Dir_2 = hit.collider.gameObject.transform.GetChild(0).position - this.transform.position;

        Vector3 tmp_Dir_1;
        Vector3 tmp_Dir_2;
        if (player.facingDir == 1)
        {
            tmp_Dir_1 = new Vector3(hit.collider.gameObject.transform.GetChild(0).position.x, this.transform.position.y)
                - this.transform.position;

            tmp_Dir_2 = hit.collider.gameObject.transform.GetChild(0).position - this.transform.position;
        }
        else
        {
            tmp_Dir_1 = new Vector3(hit.collider.gameObject.transform.GetChild(1).position.x, this.transform.position.y)
                - this.transform.position;

            tmp_Dir_2 = hit.collider.gameObject.transform.GetChild(1).position - this.transform.position;
        }

        if (Mathf.Cos(flashLight.pointLightOuterAngle / 2 * Mathf.Deg2Rad) <= tmp_Dir_1.magnitude / tmp_Dir_2.magnitude)
        {
            //��ʾ��Ӱ
            if (hit.collider.gameObject.transform.childCount == 4 &&
                hit.collider.gameObject.transform.GetChild(3).GetComponent<Shadow>() != null)
            {

                hit.collider.transform.GetChild(3).transform.GetComponent<Shadow>().ChangeShadowPosition();
                hit.collider.transform.GetChild(3).gameObject.SetActive(true);
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
            if (hit.collider.gameObject.transform.childCount == 4 &&
                hit.collider.gameObject.transform.GetChild(3).GetComponent<Shadow>() != null)
            {
                Debug.Log("������Ӱ");

                hit.collider.transform.GetChild(3).gameObject.SetActive(false);
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

                //�ҵ�Player
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

                //����Player������ľ���
                float distance = (this.transform.parent.transform.GetChild(1).transform.position - playerTransform.position).magnitude;

                Vector2 tmp_ShadowBornPosition;
                //Shadow��ʼλ��
                if (playerTransform.position.x <= this.transform.position.x)
                {
                    tmp_ShadowBornPosition = new Vector2(Mathf.Abs(tmp_Distance), -this.transform.position.y);
                }
                else
                {
                    tmp_ShadowBornPosition = new Vector2(-Mathf.Abs(tmp_Distance), -this.transform.position.y);
                }

                //ʵ��������Shadow
                GameObject tmp_Shadow = GameObject.Instantiate(shadowTargetData.shadowTargetsList[i].shadowTargetPrefab,
                    _shadowTargetGameObject.transform);

                //��ӽ���Ӱ�ļ���
                currentSceneShadowList.Add(tmp_Shadow);

                tmp_Shadow.transform.localPosition = tmp_ShadowBornPosition;

                //���Shadow�ű�
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

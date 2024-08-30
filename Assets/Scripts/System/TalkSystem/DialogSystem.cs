using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("�����ȡ")]
    public Text textLable;
    public Image faceImage;

    public TextAsset textFile;//�ı��ļ�

    private Sprite currentFaceSprite;//��ǰ�����ͷ��

    public HeadShotSpriteData headShotSpriteData;
    public TextFileData textFileData;

    [Header("״̬���")]
    private bool textFinished;
    private bool cancelTyping;

    [Header("��������")]
    public string targetCharacterName;

    public int index;
    public float textSpeed;//�ı�����ٶ�

    List<string> textList = new List<string>();//�и���ı�

    private void Awake()
    {
        //��ֵ�Ի���ɫ�ı�
        GetTextAseetByName(targetCharacterName);

        GetTextFormFile(textFile);//��ȡ�ı�����
    }

    private void OnEnable()
    {
        textFinished = true;
        //��ȡ��һ������
        StartCoroutine(SetTextUI());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (textFinished)
            {
                StartCoroutine(SetTextUI());

                if (index == textList.Count)
                {
                    gameObject.SetActive(false);
                    index = 0;
                }
            }
            else if (!textFinished && !cancelTyping)
            {
                cancelTyping = true;
            }
        }
    }

    /// <summary>
    /// ��ȡText�ļ�������
    /// </summary>
    /// <param name="file"></param>
    private void GetTextFormFile(TextAsset file)
    {
        textList.Clear();//���
        index = 0;

        var lineData = file.text.Split('\n');//�����з��и������

        foreach(var line in lineData)
        {
            textList.Add(line);
        }
    }

    /// <summary>
    /// �ı��������
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetTextUI()
    {
        textFinished = false;
        textLable.text = "";//����ı���

        //TODO:
        switch (textList[index].Trim())
        {
            case "A":
                //��ȡ����ͷ��
                //GetSpriteByName();

                //��ֵ����ͷ��


                index++;
                break;

            case "B":
                //��ȡ����ͷ��
                //GetSpriteByName();

                //��ֵ����ͷ��


                index++;
                break;

            default:
                break;
        }

        int letter = 0;
        while (!cancelTyping && letter < textList[index].Length -1)
        {
            textLable.text += textList[index][letter];
            letter++;

            yield return new WaitForSeconds(textSpeed);
        }

        textLable.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
    }

    /// <summary>
    /// ��ȡ��ǰ�����ͷ��
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public Sprite GetSpriteByName(string _name)
    {
        foreach(var sprite in headShotSpriteData.headShotSpritesList)
        {
            if(sprite.name == _name)
            {
                return sprite.headShotSprite;
            }
        }

        return null;
    }

    /// <summary>
    /// Ѱ�ҶԻ�������ı��ļ�
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public TextAsset GetTextAseetByName(string _name)
    {
        foreach(var file in textFileData.textFilesList)
        {
            if(file.fileName == _name)
            {
                return file.textAsset;
            }
        }

        return null;
    }
}

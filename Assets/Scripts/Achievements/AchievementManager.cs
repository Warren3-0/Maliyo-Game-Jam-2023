using Lean.Gui;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public List<ObjectivesSO> Objectives = new List<ObjectivesSO>();


    [Header("UI")]
    public GameObject m_achievementObj;
    public Transform acheivementParentTransform;
    public LeanWindow achPanel;
    public Scrollbar scrollbar;
    public List<Button> achievementObjs = new List<Button>();

   

    public static AchievementManager instance;
    public int m_notiAmount
    {
        get { return PlayerPrefs.GetInt("AchievementNoti", 0); }
        set { PlayerPrefs.SetInt("AchievementNoti", value); }
    }
    public int m_achievemntDone
    {
        get { return PlayerPrefs.GetInt("achievementDone", 0); }
        set { PlayerPrefs.SetInt("achievementDone", value); }
    }

    [Space]
    public TextMeshProUGUI m_notificationText;
    public LeanPulse NotificationPulse;

    public List<string> messageQueue = new List<string>();


    public static Action onObjectiveCompleted;


    [Header("Game Events")]
    public GameEvent PendingAchievement;
    public GameEvent NoAchievement;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }

        else
        {

            Destroy(gameObject);
            return;
        }


        DontDestroyOnLoad(gameObject);
       

        NotificationPulse.OnPulse.AddListener((x) =>
        {
            m_notificationText.text = messageQueue[0];
            messageQueue.RemoveAt(0);
        });
       

      
    }
    public void OnObjectiveComp()
    {
        m_notiAmount++;
        NotificationPulse.RemainingPulses++;
        m_achievemntDone++;
      //  m_amountText.text = m_notiAmount.ToString();
       // m_notificationIcon.SetActive(true);
       // NotificationPulse.TryPulse();
      
        onObjectiveCompleted?.Invoke();
    }
    public void OnApplicationPause(bool pause)
    {
        if (pause == false)
        {
            m_notiAmount = 0;

            for (int i = 0; i < Objectives.Count; i++)
            {
                if (PlayerPrefs.GetInt(Objectives[i].m_Name() + "_isClaimed", 0) == 0 && Objectives[i].isObjectiveComplete())
                {
                    m_notiAmount++;
                }

            }
           // Timer.Register(0.1f, () => { AchievementNotification.instance.m_notiAmount += m_notiAmount; });
        }
        m_notiAmount = Mathf.Clamp(m_notiAmount, 0, int.MaxValue);
       // m_amountText.text = m_notiAmount.ToString();
       // if (m_notiAmount == 0)
     //       m_notificationIcon.SetActive(false);



    }

    public void ResetScroll()
    {
        Timer.Register(0.1f, () => { scrollbar.value = 1f; });
    }
    public void OnObjectiveClaimed()
    {
        m_notiAmount--;
       
        m_notiAmount = Mathf.Clamp(m_notiAmount, 0, int.MaxValue);
      //  m_amountText.text = m_notiAmount.ToString();

       // if (m_notiAmount == 0)
         //   m_notificationIcon.SetActive(false);
    }
    void Start()
    {
        
        for (int i = 0; i < Objectives.Count; i++)
        {
            int y = i;
            if (PlayerPrefs.GetInt(Objectives[y].m_Name() + "_isClaimed", 0) != 0) continue;

            Objectives[y].Init();
            GameObject go = Instantiate(m_achievementObj, acheivementParentTransform);
            go.GetComponent<AchievementObj>().InitUI(Objectives[y], () => { OnObjectiveComp(); }, false);
        }
    }
#if UNITY_EDITOR
    [Button]

    public void PopulateObjectives()
    {
        //Object[] data = AssetDatabase.LoadAllAssetsAtPath("Assets/Scriptable Objects/Story Events/Test Event.asset");

        string[] guids = AssetDatabase.FindAssets("t:" + typeof(ObjectivesSO).Name);
        Objectives.Clear();
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            if (!Objectives.Contains(AssetDatabase.LoadAssetAtPath<ObjectivesSO>(path)))
                Objectives.Add(AssetDatabase.LoadAssetAtPath<ObjectivesSO>(path));
        }
    }

    [Button]
    private void ResetData()
    {
        for (int i = 0; i < Objectives.Count; i++)
        {
            PlayerPrefs.SetInt(Objectives[i].m_Name() + "_isClaimed", 0);

        }
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

#endif


    public void RebuildList()
    {
        for (int i = 0; i < Objectives.Count; i++)
        {
            int y = i;
            if (PlayerPrefs.GetInt(Objectives[y].m_Name() + "_isClaimed", 0) != 0) continue;

            Objectives[y].Init();
            GameObject go = Instantiate(m_achievementObj, acheivementParentTransform);
            //  go.GetComponent<AchievementObj>().InitUI(Objectives[y], () => { OnObjectiveComp(); }, false);
            achievementObjs.Add(go.GetComponent<AchievementObj>().claimBtn);
        }

    }
    public void CheckAchievements()
    {
        //if (NotificationDot == null) return;

        foreach (Button achievevmentButton in achievementObjs)
        {
            if (achievevmentButton.interactable)
            {
                PendingAchievement.Raise();
                //NotificationDot.SetActive(true);
                return;
            }
        }
        //NotificationDot.SetActive(false);
        NoAchievement.Raise();
    }

}

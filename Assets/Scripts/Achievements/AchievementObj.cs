using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class AchievementObj : MonoBehaviour
{
	public TextMeshProUGUI TitleText;
	public TextMeshProUGUI DescText;

	public TextMeshProUGUI RewardTxt;
	public Slider ProgressSlider;
	public TextMeshProUGUI ProgressText;
	public Button claimBtn;
	public FloatEvent onMoneyChanged;
	[Space]
	public Color enabledColor;
	public Color disabledColor;

	[Space]
	public ObjectivesSO so;
	bool isDaily = false;

	public void InitUI(ObjectivesSO _so, Action _e, bool _isDaily)
	{
		so = _so;
		if (!so) return;


		DescText.text = so.DisplayName;

		TitleText.text = so.Name;

		RewardTxt.text = so.rewardAmount + " gems";
		so.AttachAction(() => { UpdateUI(); });
		UpdateUI();
		isDaily = _isDaily;
		so.OnObjectiveCompleted.AddListener(() =>
		{
			AchievementManager.instance.messageQueue.Add("Completed: " + DescText.text);
			
			_e?.Invoke();


		});

	}

	public void UpdateUI()
	{
		ProgressSlider.value = so.ObjectiveCompleteRate();
		ProgressText.text = so.ObjectiveCompleteProgress();

		claimBtn.interactable = so.isObjectiveComplete();
		claimBtn.image.color = so.isObjectiveComplete() ? enabledColor : disabledColor;


		if (claimBtn.interactable)
		{
			claimBtn.transform.parent.SetAsFirstSibling();
		}
	}

	public void Claim()
	{
		GameStateManager.EconomyManager.AddMoney(so.rewardAmount);
		onMoneyChanged.Raise(so.rewardAmount);
		PlayerPrefs.SetInt(so.m_Name() + "_isClaimed", 1);
		this.gameObject.SetActive(false);
		if (!isDaily)
			AchievementManager.instance.OnObjectiveClaimed();
		//else
		//DailyAcheivementManager.instance.OnObjectiveClaimed();

	}

	public void ClearAchievement()
	{
		this.GetComponent<RectTransform>().DOScale(0, .5f).SetEase(Ease.InBack);

		Timer.Register(.5f, () =>
		{
			AchievementManager.instance.achievementObjs.Remove(claimBtn);
			 AchievementManager.instance.CheckAchievements();
			this.gameObject.SetActive(false);
			this.GetComponent<RectTransform>().localScale = Vector3.one;
		});
	}

}

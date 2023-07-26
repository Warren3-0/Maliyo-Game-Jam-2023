using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Objective", menuName = "New Objective")]
public class ObjectivesSO : ScriptableObject
{

	public string Name;
	public string DisplayName;
	public int rewardAmount;
  [Space]
	public bool isBuyableData;
	
	
	
	[ShowIf("isBuyableData")] public string identifier;
	[Space]

	public DataType data;


	[Space] [ShowIf("IsFloat")] private float floatValue;
	[Space] [ShowIf("IsInt")] private int intValue;
	[Space] [ShowIf("IsBool")] private bool booleanValue;
	

	public Conditions CompleteCondition;

	[ShowIf("IsFloat")] public float floatTargetValue;
	[ShowIf("IsInt")] public int IntTargetValue;
	[ShowIf("IsBool")] public bool boolTargetValue;


	[Header("Events for initalization")]
	[Label("")] [ReadOnly] public Empty m_empty;

	[ReorderableList] public List<GameEvent> GameEvents = new List<GameEvent>();
	[ShowIf("HasGE")] public UnityVoidEvent Attached_GameEvent;

	[ShowIf("IsInt")] public IntEvent unityIntEvent;
	[ShowIf("HasIE")] public UnityIntEvent Attached_IntEvent;

	[ShowIf("IsFloat")] public FloatEvent unityFloatEvent;
	[ShowIf("HasFE")] public UnityFloatEvent Attached_FloatEvent;

	[InfoBox("Instructions \nYou feed the objective so with game events or respective data type events that " +
		"is required to called the objective\n\n" +
		"The Unity Event beneath its coresponding EventSO takes this object and defines how the event influences the value")]
	[Label("")] [ReadOnly] public Empty mm_empty;

	[Space] public UnityEvent OnObjectiveCompleted;
	[Space] public UnityEvent OnSpent;
	

	public int IntValue
	{
		get => intValue; set
		{
			PlayerPrefs.SetInt(m_Name() + "intValue", value);
			intValue = value;
		}
	}

	public bool IsFloat() => data == DataType.Float;
	public bool IsInt() => data == DataType.Integer;
	public bool IsBool() => data == DataType.Bool;
	

	public bool HasGE() => GameEvents.Count > 0;
	public bool HasIE() => unityIntEvent != null;
	public bool HasFE() => unityFloatEvent != null;
	


	public void IncrementValue(int val = 1)
	{
		if (IsInt())
			IntValue += val;
	}
	public void IncrementByOne() { IncrementValue(1); }
	public void IncrementValue(float val = 1)
	{
		if (IsFloat())
			floatValue += val;
	}


	public void SetValue(int i = 0)
	{
		if (IsInt())
			IntValue = i;
	}
	public void SetTargetValue(int i = 0)
	{
		if (IsInt())
			IntTargetValue = i;
	}
	public void SetValue(float i = 0)
	{
		if (IsFloat())
			floatValue = i;
	}
	public void SetTargetValue(float i = 0)
	{
		if (IsFloat())
			floatTargetValue = i;
	}
	public void SetValue(bool i)
	{
		booleanValue = i;
	}
	
	public string m_Name()
	{
		if (isBuyableData)
			return Name + identifier;
		else
			return Name;
	}
	public void Init()
	{

		
		IntValue = PlayerPrefs.GetInt(m_Name() + "intValue", 0);
		if (GameEvents.Count > 0) GameEvents.ForEach((x) => x.RegisterListener(Attached_GameEvent));
		if (unityFloatEvent) unityFloatEvent.RegisterListener(Attached_FloatEvent);
		if (unityIntEvent) unityIntEvent.RegisterListener(Attached_IntEvent);
		


	}

	
	
	
	public void AttachAction(Action _a)
	{
		if (GameEvents.Count > 0) GameEvents.ForEach((x) => x.OnRaise.AddListener((x) => { _a.Invoke(); }));
		if (unityFloatEvent) unityFloatEvent.OnRaise.AddListener((x) => { _a.Invoke(); });
		if (unityIntEvent) unityIntEvent.OnRaise.AddListener((x) => { _a.Invoke(); });
		
	}
	public void Reset()
	{
		if (IsFloat())
			floatValue = 0;
		if (IsInt())
			IntValue = 0;
	
		if (IsBool())
			booleanValue = false;

		if (GameEvents.Count > 0)
			GameEvents.ForEach((x) => x.UnregisterListener(Attached_GameEvent));
		if (unityFloatEvent) unityFloatEvent.UnregisterListener(Attached_FloatEvent);
		if (unityIntEvent) unityIntEvent.UnregisterListener(Attached_IntEvent);
	
	}

	public bool isObjectiveComplete()
	{
		if (PlayerPrefs.GetInt(m_Name() + "_isCompleted", 0) == 1)
			return true;

		bool comp = false;
		if (IsFloat())
		{
			if (CompleteCondition == Conditions.GreaterOrEqual)
				comp = floatValue >= floatTargetValue;
			else if (CompleteCondition == Conditions.LessOrEqual)
				comp = floatValue <= floatTargetValue;
		}
		else if (IsInt())
		{
			if (CompleteCondition == Conditions.GreaterOrEqual)
				comp = IntValue >= IntTargetValue;
			else if (CompleteCondition == Conditions.LessOrEqual)
				comp = IntValue <= IntTargetValue;
		}
		else if (IsBool())
			comp = booleanValue == boolTargetValue;

		if (comp)
		{
			PlayerPrefs.SetInt(m_Name() + "_isCompleted", 1);
			OnObjectiveCompleted?.Invoke();

		}
		return comp;
	}
	[Button]
	public void FakeObjectiveComplete()
	{
		OnObjectiveCompleted?.Invoke();

	}
	public float ObjectiveCompleteRate()
	{
		float comp = 0;
		if (IsFloat())
		{
			if (CompleteCondition == Conditions.GreaterOrEqual)
				comp = Mathf.Clamp01(floatValue / floatTargetValue);
			else if (CompleteCondition == Conditions.LessOrEqual)
				comp = Mathf.Clamp01(floatValue / floatTargetValue);
		}
		else if (IsInt())
		{
			if (CompleteCondition == Conditions.GreaterOrEqual)
				comp = Mathf.Clamp01((float)IntValue / (float)IntTargetValue);
			else if (CompleteCondition == Conditions.LessOrEqual)
				comp = Mathf.Clamp01((float)IntValue / (float)IntTargetValue);
		}
		else if (IsBool())
			comp = booleanValue == boolTargetValue ? 1 : 0;

		return comp;
	}
	public string ObjectiveCompleteProgress()
	{
		string comp = "";
		if (IsFloat())
		{
			floatValue = Mathf.Clamp(floatValue, 0, floatTargetValue);
			if (CompleteCondition == Conditions.GreaterOrEqual)
				comp = $" {floatValue}/{floatTargetValue}";
			else if (CompleteCondition == Conditions.LessOrEqual)
				comp = $" {floatValue}/{floatTargetValue}";
		}
		else if (IsInt())
		{
			IntValue = Mathf.Clamp(IntValue, 0, IntTargetValue);
			if (CompleteCondition == Conditions.GreaterOrEqual)
				comp = $" {IntValue}/{IntTargetValue}";
			else if (CompleteCondition == Conditions.LessOrEqual)
				comp = $" {IntValue}/{IntTargetValue}";
		}
		else if (IsBool())
			comp = $" {(booleanValue == boolTargetValue ? 1 : 0)}/{1}";

		return comp;
	}
	public enum Conditions
	{
		GreaterOrEqual = 0, LessOrEqual = 1
	}
}

public enum DataType
{
	Integer, Float, Bool, BigDouble
}
[System.Serializable] public class Empty { }
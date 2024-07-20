using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class LoginGui : IDisposable {
	[SerializeField]
	private GameObject loginRoot;
	public TextMeshProUGUI playerName;
	[SerializeField]
	private Button loginBtn;
	public void AddListner(UnityAction action) {
		loginBtn.onClick.AddListener(action);
	}
	public void Dispose() {
		loginBtn.onClick.RemoveAllListeners();
	}
	public void Show(bool show) {
		loginRoot.SetActive(show);
	}
}

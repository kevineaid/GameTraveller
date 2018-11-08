using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { set; get; }

	private bool isGameStarted = false;
	private PlayerMotor motor;
	//UI and text
	public Text ScoreText, coinText, modifierText;
	private float score, coinScore, modifierScore;
	// Use this for initialization
	private void Awake () {
		Instance = this;
		UpdateScores();
		motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>(); 
	}
	
	// Update is called once per frame
	private void Update () {
		if (!isGameStarted && Input.GetKeyDown(KeyCode.Return)) {
			isGameStarted = true;
			motor.StartRunning();
		}
	}

	private void UpdateScores(){
		ScoreText.text = score.ToString();
		coinText.text = coinScore.ToString();
		modifierText.text = modifierScore.ToString();
	}
}

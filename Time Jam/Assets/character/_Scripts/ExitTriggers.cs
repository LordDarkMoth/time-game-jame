using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTriggers : MonoBehaviour {

	public bool north = false;
	public bool east = false;
	public bool south = false;
	public bool west = false;

	public GameObject _player;
	private Collider _playerCollider;
	void Start(){
		
	}

	void OnTriggerEnter(){
        Debug.Log("entered Exit Zone");
		//if (north) {
  //          HistoryFabricator.playersYCoordinates++;
  //          HistoryFabricator.playersTravelDirection = "N";

		//} else if (east) {
  //          HistoryFabricator.playersXCoordinates++;
  //          HistoryFabricator.playersTravelDirection = "E";
		//} else if (south) {
  //          HistoryFabricator.playersYCoordinates--;
  //          HistoryFabricator.playersTravelDirection = "S";
		//} else if (west) {
  //          HistoryFabricator.playersXCoordinates--;
  //          HistoryFabricator.playersTravelDirection = "W";
		//} else {
		//	print ("Direction not set!");
		//}
        SceneManager.LoadScene("StageLoader");

	}
}

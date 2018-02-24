using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheCamera : MonoBehaviour {
	public float rotSpeed;
	public Camera mainCamera;
	public GameObject nextSceneObj;
	public int sceneNum;

	public bool toNextScene = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(SceneManager.GetActiveScene().name == "Menu")
		{
			//if (Input.anyKeyDown)
			//	GoToScene ();
			
			if (toNextScene) {
				mainCamera.transform.position = Vector3.MoveTowards (mainCamera.transform.position, nextSceneObj.transform.position, 1f);
				mainCamera.transform.rotation = Quaternion.RotateTowards (mainCamera.transform.rotation, nextSceneObj.transform.rotation, 1f);

				if (mainCamera.transform.position == nextSceneObj.transform.position)
					SceneManager.LoadScene (1);
			}
		}
//		else if(SceneManager.GetActiveScene().name == "Game"){
//
//		}
	}

	public void GoToScene(int x){
		sceneNum = x;
		toNextScene = true;
	}
}

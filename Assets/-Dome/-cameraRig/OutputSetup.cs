using UnityEngine;
using System.Collections;

public enum OutputConfig {dome_180, dome_210, dome_230, dome_18015, dome_18025};
//public enum OutputAngle {incline_0, incline_15, incline_25};
public enum OutputResolution{_3840x1080, _1920x2160, _2240x2240, OneCam_2240x2240};

[ExecuteInEditMode]
public class OutputSetup : MonoBehaviour {
	
	public OutputConfig config;
	public OutputResolution resolution;
	public int stitcherLayer;
	public Camera stitcherCam_Back;
	public Camera stitcherCam_Front;
	public Camera stitcherCam_One;

	// Stitchers
	public GameObject stitcher180_back;
	public GameObject stitcher180_front;
	public GameObject stitcher180_One;
	public GameObject stitcher210_back;
	public GameObject stitcher210_front;
	public GameObject stitcher210_One;
	public GameObject stitcher230_back;
	public GameObject stitcher230_front;
	public GameObject stitcher230_One;
	public GameObject camrig;
	
	private OutputConfig currentConfig;
	//private OutputAngle currentIncline;
	
	void Start(){

		float stitcherScaling = 1f;
		Vector3 stitcherBackPos = Vector3.zero;
		Vector3 stitcherFrontPos = Vector3.zero;
		Vector3 stitcherOnePos = Vector3.zero;
		
		if(resolution == OutputResolution._1920x2160){
			stitcherCam_Front.rect = new Rect(0f, 0f, 1f, 1f);
			stitcherCam_Back.enabled = false;
			stitcherCam_One.enabled=false;
			stitcherScaling = stitcherCam_Front.orthographicSize  * Screen.width / Screen.height;
			stitcherBackPos = new Vector3(0f, -1f, 0.12f);
			stitcherFrontPos =  new Vector3(0f, 0f, 0.12f);
		}

		else if(resolution == OutputResolution._3840x1080){
			stitcherCam_Front.rect = new Rect(0f, 0f, 0.5f, 1f);
			stitcherCam_Back.enabled = true;
			stitcherCam_One.enabled=false;
			stitcherScaling = stitcherCam_Front.orthographicSize  * Screen.width / 2 / Screen.height;
			stitcherBackPos = new Vector3(0f, -0.5f, 0.12f);
			stitcherFrontPos = new Vector3(0f, 0.5f, 0.12f);
		}
		
		else if(resolution == OutputResolution._2240x2240){
			//stitcherCam_All.enabled = false;
			stitcherCam_Front.rect = new Rect(0f, -0.5f, 1f, 1f);
			stitcherCam_Back.rect = new Rect(0f, 0.5f, 1f, 1f);
			stitcherCam_Back.enabled=true;
			stitcherCam_Front.enabled=true;
			stitcherCam_One.enabled=false;
			stitcherScaling = stitcherCam_Front.orthographicSize  *(Screen.width / Screen.height)*2f;
			stitcherBackPos = new Vector3(0f, -0.5010f, 0.12f);
			stitcherFrontPos = new Vector3(0f, 0.5010f, 0.12f);
		} 
		
		else if(resolution == OutputResolution.OneCam_2240x2240){
			//stitcherCam_All.enabled = false;
			//stitcherCam_Front.rect = new Rect(0f, -0.5f, 1f, 1f);
			//stitcherCam_Back.rect = new Rect(0f, 0.5f, 1f, 1f);
			stitcherCam_One.rect = new Rect(0f, 0f, 1f, 1f);
			stitcherCam_Back.enabled=false;
			stitcherCam_Front.enabled=false;
			stitcherCam_One.enabled=true;
			stitcherScaling = stitcherCam_One.orthographicSize *(Screen.width / Screen.height);
			stitcherBackPos = new Vector3(0f, 0f, 0.12f);
			stitcherFrontPos = new Vector3(0f, 1f, 0.12f);
			stitcherOnePos = new Vector3(0f, 0f, 0.12f);
		}
		
		// Scale and position stitchers to fit screen
		stitcher180_back.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher180_front.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher180_One.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher180_back.transform.localPosition = stitcherBackPos;
		stitcher180_front.transform.localPosition = stitcherFrontPos;
		stitcher180_One.transform.localPosition = stitcherOnePos;
		stitcher210_back.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher210_front.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher210_One.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher210_back.transform.localPosition = stitcherBackPos;
		stitcher210_front.transform.localPosition = stitcherFrontPos;
		stitcher210_One.transform.localPosition = stitcherOnePos;
		stitcher230_back.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher230_front.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher230_One.transform.localScale = new Vector3(stitcherScaling, stitcherScaling, stitcherScaling);
		stitcher230_back.transform.localPosition = stitcherBackPos;
		stitcher230_front.transform.localPosition = stitcherFrontPos;
		stitcher230_One.transform.localPosition = stitcherOnePos;
			
		//Set stitchers culling
		stitcher180_back.layer = stitcherLayer;
		stitcher180_front.layer = stitcherLayer;
		stitcher180_One.layer = stitcherLayer;
		stitcher210_back.layer = stitcherLayer;
		stitcher210_front.layer = stitcherLayer;
		stitcher210_One.layer = stitcherLayer;
		stitcher230_back.layer = stitcherLayer;
		stitcher230_front.layer = stitcherLayer;
		stitcher230_One.layer = stitcherLayer;
		
		stitcherCam_Front.cullingMask = 1 << stitcherLayer;
		stitcherCam_Back.cullingMask = 1 << stitcherLayer;
		stitcherCam_One.cullingMask = 1 << stitcherLayer;
		
		SwitchConfig(config);
	}


	//Switch between 180 210 and 230�
	void SwitchConfig(OutputConfig c){
		
		Debug.Log("Switch Config..");
		
		switch(c){
			case OutputConfig.dome_180:
				//camrig.transform.rotation=Quaternion.Euler(0,0,0);
				stitcher180_back.SetActive(true);
				stitcher180_front.SetActive(true);
				stitcher180_One.SetActive(true);
				stitcher210_back.SetActive(false);
				stitcher210_front.SetActive(false);
				stitcher210_One.SetActive(false);
				stitcher230_back.SetActive(false);
				stitcher230_front.SetActive(false);
				stitcher230_One.SetActive(false);
			break;
				
			case OutputConfig.dome_210:
				//camrig.transform.rotation=Quaternion.Euler(0,0,0);
				stitcher180_back.SetActive(false);
				stitcher180_front.SetActive(false);
				stitcher180_One.SetActive(false);
				stitcher210_back.SetActive(true);
				stitcher210_front.SetActive(true);
				stitcher210_One.SetActive(true);
				stitcher230_back.SetActive(false);
				stitcher230_front.SetActive(false);
				stitcher230_One.SetActive(false);
				
			break;
				
			case OutputConfig.dome_230:
				//camrig.transform.rotation=Quaternion.Euler(0,0,0);
				stitcher180_back.SetActive(false);
				stitcher180_front.SetActive(false);
				stitcher180_One.SetActive(false);
				stitcher210_back.SetActive(false);
				stitcher210_front.SetActive(false);
				stitcher210_One.SetActive(false);
				stitcher230_back.SetActive(true);
				stitcher230_front.SetActive(true);
				stitcher230_One.SetActive(true);
				
			break;
			
			case OutputConfig.dome_18015:
				//camrig.transform.rotation=Quaternion.Euler(0,0,0);
				stitcher180_back.SetActive(true);
				stitcher180_front.SetActive(true);
				stitcher180_One.SetActive(true);
				stitcher210_back.SetActive(false);
				stitcher210_front.SetActive(false);
				stitcher210_One.SetActive(false);
				stitcher230_back.SetActive(false);
				stitcher230_front.SetActive(false);
				stitcher230_One.SetActive(false);
				
			break;
			
			case OutputConfig.dome_18025:
				//camrig.transform.rotation=Quaternion.Euler(245,180,0);
				stitcher180_back.SetActive(true);
				stitcher180_front.SetActive(true);
				stitcher180_One.SetActive(true);
				stitcher210_back.SetActive(false);
				stitcher210_front.SetActive(false);
				stitcher210_One.SetActive(false);
				stitcher230_back.SetActive(false);
				stitcher230_front.SetActive(false);
				stitcher230_One.SetActive(false);
				
			break;
		}
		currentConfig = c;
	}	
}
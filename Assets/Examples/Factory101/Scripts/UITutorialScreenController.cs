using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UITutorialScreenController : MonoBehaviour
{
   
   public static UITutorialScreenController Instance { get; private set; }
   
   public GameObject ScreenZero;
   public GameObject ScreenOne;
   public GameObject ScreenTwo;
   public GameObject ScreenThree;
   public GameObject ScreenFour;
   public GameObject ScreenFive;
   public GameObject ScreenSix;
   public CameraController cameraController;
   public bool hasProgrammed;
   public UnityEvent OnProgrammedButtonPressed;
   public int moneyStart;

   private bool doingTutorial = false;
   public ExcavatorMachine startExcavator;
   public MixerMachine mixerMachine;
   public GameObject toolbarBlocker;
   
   public bool DoingTutorial()
   {
      return doingTutorial;
   }
   void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(this.gameObject); // or Destroy(this);
         return;
      }

      Instance = this;
   }

   [ContextMenu("Clear Tutorial")]
   public void ClearTutorial()
   {
      PlayerPrefs.SetInt("FinishedTutorial", 0);
   }
   
   public IEnumerator Start()
   {
      Debug.Log($"Doing Tutorial: {PlayerPrefs.GetInt("FinishedTutorial")}");
      if (PlayerPrefs.GetInt("FinishedTutorial") == 1)
      {
         ForceStartGame();
      }else{
         ScreenZero.SetActive(true);
         doingTutorial = true;
         InteractionModeController.Instance.SetModeBlocked();
         yield return new WaitForSeconds(3f);
         ScreenZero.SetActive(false);
         cameraController.SetFreezeCamera(true);
         ScreenOne.SetActive(true);
         InteractionModeController.Instance.SetModeDefault();
      }
   }
   

   public void ShowScreenTwo()
   {
      cameraController.SetFreezeCamera(true);
      ScreenTwo.SetActive(true);
   }
   
   public void ShowScreenThree()
   {
      cameraController.SetFreezeCamera(true);
      ScreenThree.SetActive(true);
   }

   public void ShowScreenFour()
   {
      cameraController.SetFreezeCamera(true);
      ScreenFour.SetActive(true);
   }
   
   public void ShowScreenFive()
   {
      mixerMachine.gameObject.SetActive(true);
      cameraController.SetFreezeCamera(true);
      ScreenFive.SetActive(true);
   }

   public void ShowScreenSix()
   {
      cameraController.SetFreezeCamera(true);
      ScreenSix.SetActive(true);
   }
   
   public void FinishTutorial()
   {
      GameInfoManager.Instance.AddMoney(moneyStart);
      doingTutorial = false;
      toolbarBlocker.SetActive(false);
      ScreenZero.SetActive(false);
      cameraController.SetFreezeCamera(false);

      PlayerPrefs.SetInt("FinishedTutorial", 1);
      PlayerPrefs.Save();
   }

   public void ForceStartGame()
   {
      doingTutorial = false;
      toolbarBlocker.SetActive(false);
      ScreenZero.SetActive(false);
      cameraController.SetFreezeCamera(false);
      startExcavator.SetDigRate(0.3f);
      startExcavator.KickCoroutine();
   }
   
   public void ProgramButtonPressed()
   {
      if (!hasProgrammed && DoingTutorial())
      {
         hasProgrammed = true;
         OnProgrammedButtonPressed.Invoke();
         startExcavator.SetDigRate(0.3f);
      }
   }

}

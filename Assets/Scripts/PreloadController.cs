using System.Collections.Generic;
using System.Threading.Tasks;
using HB.Packages.Controllers;
using HB.Packages.StateMachine;
using UnityEngine;

public class PreloadController : Controller
{
    private abstract class GunState : State<PreloadController>
    {
    }

    private class InitializeGun : GunState
    {
            
        private bool _finished;
        protected override async void OnEnter()
        {
            Debug.Log("on initialize state enter");
            base.OnEnter();
            _finished = true;

        }

  

        private async void Temp()
        {
            await Task.Delay(1000);

        }

        

        protected override void OnUpdate(float deltaTime)
        {
            
            base.OnUpdate(deltaTime);
            Debug.Log("1111111111111111111111");
            _finished = true;
            if (_finished)
                Finished();
        }

        protected override void OnExit()
        {
            Debug.Log("1111111111111111111111");
            base.OnExit();
               
        }
    }

    private class GrabState : GunState
    {
        private bool _finished;

        protected override void OnEnter()
        {
            Debug.Log("on next scene enter ");


            base.OnEnter();
            LoadGun();
            _finished = true;
        }

        private void LoadGun()
        {
           
            _loadNextSceneOperation.allowSceneActivation = true;
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (_finished)
                Finished();
        }

        protected override void OnExit()
        {
              

            base.OnExit();
        }
    }

    private class GunConnectState : GunState
    {

        private int _attempt;
        private float _elapsed;

        protected override void OnEnter()
        {
                

            base.OnEnter();

        }

        private void OnSuccess()
        {
      
            Finished();
        }



        private void OnError()
        {
       
            //Debug.Log(error.Message);
      
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            _elapsed = 0;

        }
    }

    private class GunModeState : GunState
    {
          

        protected override void OnEnter()
        {
            
            base.OnEnter();
     
            Finished();
        }

        protected override void OnExit()
        {
            base.OnExit();
           
        }
    }

    private class GunDebugState : GunState
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            LoadNextScene();
        }

        private async void LoadNextScene()
        {
            // SceneManager.LoadScene(Strings.Match3Debug); 
            // await Agent._assetLoader.LoadScene(Strings.Match3Debug);
        }
    }

    #region Private Fields

    private Fsm<PreloadController> _fsm;

    private float _progressTimer;

    private bool splashScreenVideoPassed = false;
    private static AsyncOperation _loadNextSceneOperation;
    #endregion

    #region Unity

    protected override void Awake()
    {


        PlayerPrefs.SetFloat("StartTime", Time.time);
        PlayerPrefs.Save();
        _progressTimer = 3;
            
        splashScreenVideoPassed = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        InitializeGun initialGun = new InitializeGun { Name = "Init  state" };
        GrabState grabState = new GrabState { Name = "grab State state" };
        GunModeState gunState = new GunModeState { Name = "gunState  State" };
        GunDebugState gunDebugState = new GunDebugState() { Name = "Goto  gun Debug State" };
        Transition.CreateAndAssign(initialGun, gunState);
        Transition.CreateAndAssign(gunState, grabState);

        _fsm = new Fsm<PreloadController>(this, initialGun) { Name = "Preload FSM" };

        RunFsm();
          
    }


    private void RunFsm()
    {
         
        _fsm.Start();
    }

    
    private void Update()
    {
        //_fsm?.UnityEngine.PlayerLoop.Update(Time.deltaTime);
        // _progressTimer += Time.deltaTime;
        // if (_loadNextSceneOperation != null)
        // {
        //     _progressTimer = Mathf.Clamp(_progressTimer, _loadNextSceneOperation.allowSceneActivation ? 8 : 0, _loadNextSceneOperation.allowSceneActivation ? 10 : 9);
        // }
        // var progress = _progressTimer / 10f;
           
    }

    #endregion
}
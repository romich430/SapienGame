using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using CartoonFX;
using FrostweepGames.Plugins.GoogleCloud.SpeechRecognition;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BattleController : MonoBehaviour
{
   KeyBordController keyBoard;

    [Header("Person")]
    public float damage;
    public MovingBattlePeron person;
    public Animator animPerson;
    public Slider hpPersonSlider;
    public Text TextPersonHP;
    public int HP_Person;
    int Start_hp_Person;
    [Space]
    //public string[] TaskString;
    private Animator CanvasAnim;
    [Space]
    [Header("Gems")]
    public Animator gems;
    public GameObject[] gem;
    public GameObject ParticleCrashGem;
    public Transform tranfGem;
    public GameObject ParticleUronNumber;
    public Button ClickOnGem;
    [Space]
    [Header("Enemy")]
    public EnemyController EnemyControll;

    public int indexGem = 0;
    public int CurrentUron;

    [Space]
    [Header("Task")]
    public VoiceregonsionForBattle voiceRec;
    public GameObject OneWord,OneFraze;
    public Text TimeText;
    public bool TimeGo;
    string Task;
    public string VoiceTask;
    public int[] RandomTask;
    [SerializeField] private AudioSource _dictorAudio;
    public AudioSource[] _dictorNotInfentlySaid;
    [Space]
    private Camera Cam;
    private Ray RayMouse;
    [Space]
    [Header("Static ui")]
    public Button Bomb;
    public Slider bombSlider;
    public Text RESULT_TEXT;
    public Button GoHome;
    [SerializeField] private AudioSource _mainMusik;
    private float _volumeofMusik;
    [SerializeField] private bool _isPlayMusik;
    [Space]
    [Header("Survive Modes")]
    public bool infinitely;
    public int LerningModeId;

    [SerializeField] private GameObject _bombUI;
    [SerializeField] private Button _infentlyInstrument;
    [SerializeField] private Button _firstButton;
    [SerializeField] private Button _secondButton;
    [SerializeField] private Button _hammerButton;

    [SerializeField] private UIAnimation _animation;
    [SerializeField] private VoicePlaybackForBattle _voicePlayback;
    [SerializeField] private VoiceregonsionForBattle _voiceRecognision;
    [SerializeField] private UIController _uiController;
     int allTime = 45;
     [SerializeField] private int _firstTaskTime;
     [SerializeField] private int _secondTaskTime;
     [SerializeField] private int _thirdTaskTime;
     [SerializeField] private int _fourthTaskTime;

     public int RandomString;
     [SerializeField] private Text _taskText;
     [SerializeField] private GameObject _pharsePanel;
    
     [SerializeField] private Image _firstButtonImage;

     [SerializeField] private int _random;
     [SerializeField] private EnemyController _enemyContoller;
    
     [SerializeField] private int _minDamage;
     [SerializeField] private int _maxDamage;
     [SerializeField] private GameObject _diamond;
     [SerializeField] private GameObject[] _enemies;
     [SerializeField] private GameObject _hammerPanel;
     [SerializeField] private HammerBattle _hammerBattle;
     public bool IsHammerTask;
     


    private void Start()
    {
        
       
       if(!infinitely)
       {
            CreateRandom();
            gem[3].SetActive(false);
       }
        InfentlyMode();
        _mainMusik.Play();
       
        _infentlyInstrument.onClick.AddListener(ClickOnGems);
        _firstButton.onClick.AddListener(bomb);
        _secondButton.onClick.AddListener(ClickOnGems);
        _hammerButton.onClick.AddListener(OnHammerButtonCLick);
        bombSlider.value = 0;
        Bomb.interactable = false;
        Start_hp_Person = HP_Person;
        HP_Person_Controller(0);
        //InitialiseFrase();
        Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        keyBoard = GetComponent<KeyBordController>();
       _volumeofMusik = _mainMusik.GetComponent<AudioSource>().volume;
       // CanvasAnim = GetComponent<Animator>();
        StartCoroutine(StartFight());
        _firstButtonImage = _firstButton.GetComponent<Image>();
        
    
    }

     private void CreateRandom()
    {
        
        
        for (int i = RandomTask.Length - 1; i > 0; i--)
        {
            var r = new System.Random();
            int j = r.Next(i);
            var t = RandomTask[i];
            RandomTask[i] = RandomTask[j];
            RandomTask[j] = t;
        }
    }

    private void Update()
    {
        /*if(LerningModeId == 4)
        {
            _firstButton.transform.DOMoveY(80, 0.5f);
            _firstButtonImage.enabled = true;
            
        }*/
    }
    void FixedUpdate()
    {
       
        if (Input.GetButtonDown("Fire1") && _enemyContoller.IsAttack == true)
        {
            if (Cam != null)
            {
                RaycastHit hit;
                var mousePos = Input.mousePosition;
                RayMouse = Cam.ScreenPointToRay(mousePos);

                if (Physics.Raycast(RayMouse.origin, RayMouse.direction, out hit, 40))
                {
                    if (hit.collider.tag == "Enemy")
                    {   
                        StartCoroutine(person.ClickOnEnemy(hit.collider.gameObject.transform.position));
                        damage = Random.Range(_minDamage, _maxDamage);
                        bombSlider.value = 0;
                        Bomb.interactable = false;
                        _enemyContoller.IsAttack = false;
                    }
                }
            }
            else { Debug.Log("No camera"); }
        }
    }
    /*private void InitialiseFrase()
    {
        PlayerPrefs.SetString("phrase" + 0, "Hello");
        PlayerPrefs.SetString("phrase" + 1, "Hello there");
        PlayerPrefs.SetString("phrase" + 2, "Nice to meet you");
        PlayerPrefs.SetString("phrase" + 3, "Good morning");
   
        int i = 0;
       
        while(PlayerPrefs.GetString("phrase" + i) != "")
        {
           TaskString[i] = PlayerPrefs.GetString("phrase" + i);
            PlayerPrefs.SetInt("phraseLength", i);
            i++;
        }
               
    }*/
      private IEnumerator StartFight()
    {
        yield return new WaitForSeconds(41f);
       StartFightPanel();
    }


      public void StartFightPanel()
      {
          _hammerButton.transform.DOMoveY(80, 0.5f);
          
           if(infinitely)
        {
            _infentlyInstrument.transform.DOMoveY(80, 0.5f);
        }
        else
        {
            _firstButtonImage.enabled = true;
            //_secondButton.transform.DOMoveY(80, 0.5f);
            _secondButton.transform.DOMoveY(80, 0.5f);
        }
        
        EnemyControll.MouseBar.gameObject.SetActive(true);
      }

      public void ClickOnGems()
    {
        if (indexGem == 0)
        {
            for(int i = 0; i < gem.Length - 1; i++)
               {
                   gem[i].GetComponent<MeshRenderer>().material.DOColor(Color.magenta, 0.01f);
                   gem[i].SetActive(true);
                   gem[i].transform.DOScale(new Vector3(1,1,1), 0.5f);
               }
            gems.SetTrigger("Pick");
            StartCoroutine(WaitToStart());
        }
        _infentlyInstrument.interactable = false;
    }

     public IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(4);
        StartCoroutine(WaitForChangeScale());
        if(infinitely)
        {
            _infentlyInstrument.transform.DOMoveY(-70,0.5f);
            _infentlyInstrument.GetComponent<Image>().enabled = false;
        }
        else
        {
            
            _secondButton.transform.DOMoveY(-70,0.5f);
            _firstButton.GetComponent<Image>().enabled = false;
            _secondButton.GetComponent<Image>().enabled = false;
        }
       
        
    }

     private IEnumerator WaitForChangeScale()
    {
        
        Debug.Log("Scale");
       if(infinitely)
       {
          if (indexGem <= 3)
        {
            float i = gem[indexGem].transform.localScale.x;
            for (float q = i; q < i * 2; q += .1f)
            {
                 yield return new WaitForFixedUpdate();
                gem[indexGem].transform.localScale = new Vector3(q, q, q);
               
            }
            
            RandomString = Random.Range(0,_voiceRecognision.TaskNotInfently.Length);
            _taskText.text = voiceRec.TaskNotInfently[RandomString];
            voiceRec.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _mainMusik.Stop();
           _animation.OpenPanel();
            TimeGo = true;
            Debug.Log("NewTask");
            StartCoroutine(InstantTaskLearning());
        }
       }
       else if(!infinitely)
       {
           if (indexGem <= 2)
        {
            float i = gem[indexGem].transform.localScale.x;
            for (float q = i; q < i * 2; q += .1f)
            {
                 yield return new WaitForFixedUpdate();
                gem[indexGem].transform.localScale = new Vector3(q, q, q);
               
            }
            RandomString = Random.Range(0,_voiceRecognision.TaskNotInfently.Length);
            _taskText.text = voiceRec.TaskNotInfently[RandomString];
            voiceRec.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _mainMusik.Stop();
            _animation.OpenPanel();
            TimeGo = true;
            Debug.Log("NewTask");
            StartCoroutine(InstantTaskNotInfently());
                /*if(indexGem == 2)
                {
                    _enemyContoller.IsAttack = true;
                    for(int a = 0; i < gem.Length; i++)
                    {
                        gem[a].SetActive(false);
                    }
                }*/
            
        }
        else 
        {
            _enemyContoller.IsAttack = true;
        }
         
    }
   
    }
        /*if (indexGem <= 3)
        {
            float i = gem[indexGem].transform.localScale.x;
            for (float q = i; q < i * 2; q += .1f)
            {
                 yield return new WaitForFixedUpdate();
                gem[indexGem].transform.localScale = new Vector3(q, q, q);
               
            }
            RandomString = Random.Range(0,_voiceRecognision.TaskNotInfently.Length);
            _taskText.text = voiceRec.TaskNotInfently[RandomString];
            voiceRec.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _mainMusik.Stop();
            
            TimeGo = true;
            Debug.Log("NewTask");
            if (infinitely) 
            {
              _animation.OpenPanel();
               StartCoroutine(InstantTaskLearning());
            }
            else if(!infinitely && indexGem <= 2)
            {
               _animation.OpenPanel();
                StartCoroutine(InstantTaskNotInfently());
                if(indexGem == 2)
                {
                    _enemyContoller.IsAttack = true;
                    for(int a = 0; i < gem.Length; i++)
                    {
                        gem[a].SetActive(false);
                    }
                }
            }
        }
        else 
        {
            _enemyContoller.IsAttack = true;
        }*/
        
    


  
        
      IEnumerator InstantTaskLearning()
    {
        indexGem++;
        StartCoroutine(Time());
        if (LerningModeId < 4f)
        {
            
            yield return new WaitForSeconds(1.5f);
            switch (LerningModeId)
            {
                case 0:
                    StartCoroutine(ListenToDictor());
                    break;
                case 1:
                    _uiController.InterLocutorSaid();
                    StartCoroutine(Speak());
                    break;
                case 2:
                    _uiController.ChooseLettersUI();
                    OneWord.SetActive(true); keyBoard.InstWord(0);
                    break;
                case 3:
                    _uiController.ChooseLettersUI();
                    OneWord.SetActive(true); keyBoard.InstWord(0);
                    break;

            }
            
        }
       
    }

    private IEnumerator InstantTaskNotInfently()
    {
        indexGem++;
        _random = Random.Range(0,2);
        StartCoroutine(Time());
        if(LerningModeId <= 2f)
        {
            yield return new WaitForSeconds(1.5f);
            switch(LerningModeId)
            {
                case 0:
                
                if(_random == 0)
                {
                    _uiController.InterLocutorSaid();
                    StartCoroutine(ListenToDictorNotinfently());
                }
                else
                {
                    _uiController.InterLocutorSaid();
                    StartCoroutine(SpeakIfNotInfently());
                }
                
                break;
                case 1:
                keyBoard.InstWordNotInfenetly(0);
                _uiController.ChooseLettersUI();
                _uiController.ChooseLettersUI();
                OneWord.SetActive(true); 
                
                break;
                case 2:
                 OneWord.SetActive(false);
                 keyBoard.KeyBoard();
                _uiController.DoPharses();
                _pharsePanel.SetActive(true); 
                
                break;
            }
        }
        else 
        {
                Debug.Log("End");
        }
    }


    public IEnumerator Repeat()
    {
        
        yield return new WaitForSeconds(3f);
        _animation.ClosePanel();
        yield return new WaitForSeconds(2f);
        _animation.OpenPanel();
        StartCoroutine(Time());
        
        yield return new WaitForSeconds(1.5f);
        
       switch (LerningModeId)
            {
                case 0:
                if(infinitely)
                    
                    StartCoroutine(ListenToDictor());
                    break;
                case 1:
                if(infinitely)
                    
                    _uiController.InterLocutorSaid();
                    StartCoroutine(Speak());
               
                    break;
                case 2:
                    _uiController.ChooseLettersUI();
                    OneWord.SetActive(true); keyBoard.InstWord(0);
                   
                    break;
                case 3:
                    _uiController.ChooseLettersUI();
                    OneWord.SetActive(true); keyBoard.InstWord(0);
                    
                    break;

            }
       
    }


    public IEnumerator ClosePanel()
    {
       LerningModeId++;
       
        yield return new WaitForSeconds(2);
       _animation.ClosePanel();
        _uiController._dialogPanel.SetActive(false);
        _uiController._microphonePanel.SetActive(false);
        StartCoroutine(WaitForChange());
        if(LerningModeId == 1)
        keyBoard.DestroyInstWord();
    }

    private IEnumerator WaitForChange(){
        yield return new WaitForSeconds(3);
        gem[indexGem - 1].transform.DOScale(new Vector3(1,1,1), 0.5f);
        gem[indexGem - 1].GetComponent<MeshRenderer>().material.DOColor(Color.black, 0.7f);
        StartCoroutine(WaitForChangeScale());
    }

    private IEnumerator ListenToDictor()
    {
        _voicePlayback.OnClickPlayButton();
        yield return new WaitForSeconds(1);
    }

    private IEnumerator Speak()
    {
       yield return new WaitForSeconds(1);
       _uiController.SpeakUI();
       voiceRec.StartRecordButtonOnClickHandler();
    }

    private IEnumerator SpeakIfNotInfently(){
        yield return new WaitForSeconds(1);
        _uiController.SpeakUI();

        voiceRec.StartRecordButtonOnClickHandler();
    }

    private IEnumerator ListenToDictorNotinfently()
    {
     
        _voicePlayback.OnClickPlayButton();
        yield return new WaitForSeconds(1);
    }

      public void CrashGem(int Plus)
    {
        
        CurrentUron += Plus;
        bombSlider.value += 0.3f; if (bombSlider.value >= 1) Bomb.interactable = true;
        StartCoroutine(CrashGemCoroutine());
    }

    IEnumerator CrashGemCoroutine()
    {
        Debug.Log("CrashGem");
        yield return new WaitForSeconds(3);
       _animation.ClosePanel();
        TimeGo = false;
        yield return new WaitForSeconds(0.5f);
        OneWord.SetActive(false); OneFraze.SetActive(false); voiceRec.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        tranfGem.position = gem[indexGem - 1].transform.position;
        gem[indexGem - 1].SetActive(false);
        Instantiate(ParticleCrashGem, tranfGem.transform);

        StartCoroutine(WaitForChangeScale());

    }

    public IEnumerator Time()
    {
        //int allTime = 45;
        switch(LerningModeId)
        {
            case 0:
            allTime = _firstTaskTime;
            break;

            case 1:
            allTime = _secondTaskTime;
            break;

            case 2:
            allTime = _thirdTaskTime;
            break;

            case 3: 
            allTime = _fourthTaskTime;
            break;
        }
        TimeGo = true;
        while (true) {
            yield return new WaitForSeconds(1f);
            
            TimeText.text = allTime.ToString();
            allTime--;
            if(allTime < 0||!TimeGo)
            {
                if (allTime < 0)
                {
                    if(infinitely)
                    {
                        StartCoroutine(Repeat());
                        if(LerningModeId == 2)
                        {
                            keyBoard.DestroyInstWord();
                        }
                    }
                    else
                    {
                        StartCoroutine(ClosePanel());
                        TimeGo = false;
                        if(LerningModeId == 1)
                        {
                            keyBoard.DestroyInstWord();
                        }
                    }
                    
                    
                }
                yield break;
            }
            
        }
        

    }

      public void HP_Person_Controller(int damage)
    {
        hpPersonSlider.maxValue = Start_hp_Person;
        HP_Person -= damage;
        TextPersonHP.text = HP_Person.ToString() + "/" + Start_hp_Person;
        hpPersonSlider.value = HP_Person;
        if (HP_Person <= 0)
        {
            StartCoroutine(FinishGame());
        }
    }

     public void EnemyDie()
    {
        Instantiate(ParticleUronNumber, EnemyControll.transform.position + new Vector3(1,1,1),Quaternion.identity);
        //ParticleUronNumber.GetComponent<CFXR_ParticleText_Runtime>().text = CurrentUron.ToString();
        EnemyControll.hpSlider.value -= CurrentUron;
        if (EnemyControll.hpSlider.value <= 0)
        {
            EnemyControll.hpSlider.value = 0;
            EnemyControll.EnemyAnim.SetTrigger("die");
            StartCoroutine(FinishGame()); 
        }
        else
        {
            EnemyControll.MouseBar.sprite = EnemyControll.attack;
            StartCoroutine(EnemyControll.EnemyGiveUron((int)damage));
        }

    }

     IEnumerator FinishGame()
    {
        Cam.GetComponent<Animator>().SetTrigger("end");
        yield return new WaitForSeconds(7f);
        RESULT_TEXT.gameObject.SetActive(true);
        GoHome.gameObject.SetActive(true);
        GoHome.onClick.AddListener(GoHomeVoid);
        _firstButton.GetComponent<Image>().enabled = true;
        _firstButton.transform.DOMoveY(-70,0.5f);

        if (HP_Person <= 0)
        {
            RESULT_TEXT.text = "LOSE";
            animPerson.SetTrigger("lose");
        }
        else if (EnemyControll.hpSlider.value <= 0)
        {
            RESULT_TEXT.text = "WIN";
            animPerson.SetTrigger("win");
        }
    }

    public void GoHomeVoid()
    {
        StartCoroutine(LoadYourAsyncScene());
    }
    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("LastScene"));
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

     public void bomb()
    {
        if(bombSlider.value >= 1)
        {
            //damage = Random.Range(_minDamage, _maxDamage);
            bombSlider.value = 0;
            Bomb.interactable = false;
            
        }
    }

   private void OnHammerButtonCLick()
   {
       IsHammerTask = true;
       for(int i = 0; i < gem.Length; i++)
       {
           gem[i].SetActive(false);
       }
       for(int  i = 0; i< _enemies.Length; i++)
       {
            _enemies[i].SetActive(false);
       }
       _diamond.transform.DOMove(new Vector3(-33.35f, 5.447f, -85.975f), 1.5f);
       StartCoroutine(WaitForOpenHammerPanel());
       
   }


    public IEnumerator WaitForOpenHammerPanel(){
        yield return new WaitForSeconds(3);
        if(_hammerBattle.index <= 2)
        {
            _uiController._microphonePanel.SetActive(false);
            _uiController._dialogPanel.SetActive(false);
            _hammerPanel.SetActive(true);
            _animation.OpenPanel();
            //_hammerButton.transform.DOMoveY(-70, 0.5f);
            //_secondButton.transform.DOMoveY(-70, 0.5f);
            StartCoroutine(_hammerBattle.DialogPlay());
        }
        else
        {
            _animation.OpenPanel();
            _uiController._dialogPanel.SetActive(true);
            _uiController._microphonePanel.SetActive(true);
            _hammerBattle.OpenVoiceTask();
            voiceRec.StartRecordButtonOnClickHandler();
        }
        
        
       
        
    }
    private void InfentlyMode()
    {
       if(infinitely == true)
        {
            _bombUI.SetActive(false);
            _infentlyInstrument.GetComponent<Image>().enabled = true;
            ClickOnGem.GetComponent<Image>().enabled = false;
            _firstButton.GetComponent<Image>().enabled = false;
            _secondButton.GetComponent<Image>().enabled = false;
        }
        else
        {
            _bombUI.SetActive(true);
            _infentlyInstrument.GetComponent<Image>().enabled = false;
             ClickOnGem.GetComponent<Image>().enabled = false;
            _firstButton.GetComponent<Image>().enabled = false;
            _secondButton.GetComponent<Image>().enabled = true;
        }
    }



    

}

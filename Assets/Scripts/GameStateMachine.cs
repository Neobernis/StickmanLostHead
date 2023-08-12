using UnityEngine;
using TMPro;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine Single;

    public TMP_Text uitLevel;
    public TMP_Text uitShots;
    public TMP_Text uitButton;

    public Vector3 castlePosition;
    public GameObject[] castles;

    private int _level;
    private int _levelMax;
    private int _shotsTaken;
    private GameObject _castle;
    private GameMode _mode = GameMode.idle;
    private string _showing = "Show Slingshot";

    private void Start()
    {
        Single = this;

        _level = 0;
        _levelMax = castles.Length;
        StartLevel();
    }

    private void Update()
    {
        UpdateGUI();

        if (_mode == GameMode.playing && Goal.goalMet)
        {
            _mode = GameMode.levelEnd;

            SwitchView("Show Both");

            Invoke(nameof(NextLevel), 2f);
        }
    }

    public static void ShotFired()
    {
        Single._shotsTaken++;
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }

        _showing = eView;

        switch (_showing)
        {
            case "Show Slingshot":
                FollowCamera.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCamera.POI = Single._castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCamera.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    private void StartLevel()
    {
        if (_castle is not null)
            Destroy(_castle);

        GameObject[] gos = GameObject.FindGameObjectsWithTag(Constants.Projectile);

        foreach (GameObject pTemps in gos)
        {
            Destroy(pTemps);
        }

        _castle = Instantiate(castles[_level], castlePosition, Quaternion.identity);
        _shotsTaken = 0;

        SwitchView("Show Both");
        ProjectileLine.Single.Clear();

        Goal.goalMet = false;

        UpdateGUI();

        _mode = GameMode.playing;
    }

    private void NextLevel()
    {
        _level++;

        if (_level == _levelMax)
            _level = 0;

        StartLevel();
    }

    private void UpdateGUI()
    {
        uitLevel.text = "Level: " + (_level + 1) + " of " + _levelMax;
        uitShots.text = "Shots Taken: " + _shotsTaken;
    }
}
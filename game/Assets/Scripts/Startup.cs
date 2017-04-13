using System;
using Firefly.Unity;
using UnityEngine;
using Firefly.Unity.Stage;
using Firefly.Unity.App;

public class Startup : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {
        GameClient.Instance.Enter += OnEnter;
        GameClient.Instance.Exit += OnExit;
    }

    private void OnExit()
    {
    }

    private void OnEnter()
    {
        StageEngine.Instance.Open<LoginStage>();
    }

    [ContextMenu("Login")]
    void TestLogin()
    {
        AppEngine.Instance.Login();
    }
}

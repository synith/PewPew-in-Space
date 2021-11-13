using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFabManager : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform rowsParent;

    private PlayerID playerIDManager;
    private string _customID;

    private void Awake()
    {
        playerIDManager = GetComponent<PlayerID>();
    }

    private void Start()
    {
        if (!playerIDManager.PlayerHasID())
            playerIDManager.CreatePlayerID();

        _customID = playerIDManager.GetPlayerID();
        Debug.Log(_customID);
        Login();
        
    }
    private void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = _customID,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create!");
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        ScoreManager.Instance.PlayerName = name;
        Debug.Log(name);
    }
    private void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "HighScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result) => Debug.Log("Leaderboard Updated succesfully");

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            GameObject row = Instantiate(rowPrefab, rowsParent);
            TextMeshProUGUI[] textMeshPros = row.GetComponentsInChildren<TextMeshProUGUI>();
            textMeshPros[0].text = (item.Position + 1).ToString();
            textMeshPros[1].text = item.DisplayName;
            textMeshPros[2].text = item.StatValue.ToString();

            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
        }
    }

    public void SetDisplayName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }
    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated Display Name: " + result.DisplayName);
    }
}

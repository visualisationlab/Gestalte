using System.Threading.Tasks;
using UnityEngine;

public class OracleAgent : MonoBehaviour
{
    [SerializeField] private DirectAPI directAPI;

    public async Task<string> SendMessageDirect(string systemMessage, string message)
    {
        string response = await directAPI.SendMessageAsync(systemMessage, message);
        return response;
    }

}
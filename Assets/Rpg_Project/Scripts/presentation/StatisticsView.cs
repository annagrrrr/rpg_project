using UnityEngine;
using UnityEngine.UI;

public class StatisticsView : MonoBehaviour
{
    [SerializeField] private Text kills;
    [SerializeField] private Text dealt;
    [SerializeField] private Text received;
    [SerializeField] private Text time;

    public void Set(RunStatistics s)
    {
        kills.text = s.Kills.ToString();
        dealt.text = s.DamageDealt.ToString();
        received.text = s.DamageReceived.ToString();
        time.text = $"{s.PlayTime:0.0} sec";
    }
}

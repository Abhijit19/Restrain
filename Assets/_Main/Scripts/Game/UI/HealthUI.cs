using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class HealthUI : MonoBehaviourPunCallbacks
{
    public Image healthSlider;
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        //UnityEngine.Debug.Log($"HealthUI Target: {targetPlayer.UserId}, View: {photonView.Owner.UserId}, Owner: {gameObject.transform.parent.name}-{gameObject.name}");
        if (changedProps.ContainsKey(Constants.PLAYERKEYS.HEALTH))
        {
            if(targetPlayer == photonView.Owner)
            {
                float health = (int) changedProps[Constants.PLAYERKEYS.HEALTH];
                health /= 100;
                OnHealthUpdated(health);
                //UnityEngine.Debug.LogWarning($"HealthUI Target: {targetPlayer.UserId}, View: {photonView.Owner.UserId}, Owner: {gameObject.transform.parent.name}-{gameObject.name}");
            }
        }
    }
    
    public void OnHealthUpdated(float value)
    {
        healthSlider.fillAmount = value;
    }

}

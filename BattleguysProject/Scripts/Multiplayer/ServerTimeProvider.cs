using System;
using Photon.Pun;

namespace Multiplayer
{
    public class ServerTimeProvider : MonoBehaviourPunCallbacks, IPunObservable
    {
#pragma warning disable 649

#pragma warning restore 649

        #region Constats
        private const string DateTimeFormat = "yyyy-MM-dd\\THH:mm:ss\\Z";
        #endregion

        public static DateTime ServerTime { get; private set; }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting && PhotonNetwork.IsMasterClient)
            {
                ServerTime = DateTime.Now;
                stream.SendNext(ServerTime.ToString(DateTimeFormat));
            }
            else if (stream.IsReading)
            {
                ServerTime = DateTime.Parse(stream.ReceiveNext().ToString());
            }
        }
    }
}
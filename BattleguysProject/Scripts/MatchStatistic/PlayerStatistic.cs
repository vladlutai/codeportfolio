using System;

namespace MatchStatistic
{
    [Serializable]
    public class PlayerStatistic
    {
        public int photonViewID;
        public int killCount;
        public int deathCount;
        public int captureCount;

        public PlayerStatistic(int photonViewID)
        {
            this.photonViewID = photonViewID;
        }

        public void AddKill()
        {
            killCount++;
        }

        public void AddDeath()
        {
            deathCount++;
        }

        public void AddCapture()
        {
            captureCount++;
        }

        public override string ToString()
        {
            return $"{photonViewID} {killCount} {deathCount} {captureCount}";
        }
    }
}
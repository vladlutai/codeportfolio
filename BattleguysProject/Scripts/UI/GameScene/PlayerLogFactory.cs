using System.Collections.Generic;
using Tools;

namespace UI.GameScene
{
    public class PlayerLogFactory : Factory<LogMessage>
    {
#pragma warning disable 649
    
#pragma warning restore 649

        public readonly List<LogMessage> LogMessagesList = new List<LogMessage>();

        protected override void Push(LogMessage item)
        {
            LogMessagesList.Remove(item);
            base.Push(item);
        }

        public override LogMessage Pull()
        {
            LogMessage logMessage = base.Pull();
            LogMessagesList.Add(logMessage);
            logMessage.Init(() => base.Push(logMessage));
            return logMessage;
        }
    }
}